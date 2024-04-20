using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RevivePoint : MonoBehaviour, IDamageable
{
    [SerializeField] private string inactiveAnimName;
    [SerializeField] private string activeAnimName;
    [SerializeField] private bool isActivatedByDefault = false;
    [SerializeField] private GameObject activateSFXPrefab;
    [SerializeField] private GameObject hitSoundPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject glowChildObject;

    public static event Action OnActivateRevivePoint;
    public static event Action OnRevivePointSave;

    private static bool revivePlayerInOtherScene = false;
    private static GameObject currentRevivePoint;
    private static GameObject lastRevivePoint;

    #region Components
    private Animator animator;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        PlayerPawn.OnPlayerRevive += RevivePlayer;
    }

    private void Start()
    {
        if (IsCurrentRevivePointInActiveScene() && RevivePointRepository.CurrentRevivePointGOName == name)
        {
            Activate(false);
            if (revivePlayerInOtherScene || FindObjectOfType<PlayerPawn>() == null)
            {
                revivePlayerInOtherScene = false;
                RevivePlayer();
            }
        }
        else if (isActivatedByDefault)
        {
            Activate(false);
        }
    }

    private void OnDestroy()
    {
        PlayerPawn.OnPlayerRevive -= RevivePlayer;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ProcessCollision(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ProcessCollision(collision.gameObject);
    }
    #endregion

    #region Other Functions
    public void TakeDamage(Types.DamageData damageData)
    {
        GameObject hitSound = Instantiate(hitSoundPrefab);
        hitSound.transform.position = new Vector2(transform.position.x, transform.position.y);
        hitSound.transform.parent = null;

        HealthComponent healthComponent = damageData.source.GetComponent<PlayerPawn>().Core.GetCoreComponent<HealthComponent>();
        healthComponent.IncreaseHealth(healthComponent.GetMaxHealth());

        OnRevivePointSave?.Invoke();
        EnemyRepository.ResetKilledEnemies();

        Activate(true);
    }

    private void ProcessCollision(GameObject other)
    {
        Projectile damagingObject = other.GetComponent<Projectile>();
        if (damagingObject != null && 
            !damagingObject.IsProjectile() &&
            damagingObject.GetDamageData().source.CompareTag(EditorConstants.TAG_PLAYER))
        {
            Types.DamageData damageData = damagingObject.GetDamageData();
            damageData.source = damagingObject.Source;
            TakeDamage(damageData);
        }
    }

    private void Activate(bool showVisuals)
    {
        if (currentRevivePoint != gameObject)
        {
            RevivePointRepository.CurrentRevivePointSceneName = SceneManager.GetActiveScene().name;
            RevivePointRepository.CurrentRevivePointGOName = name;

            animator.Play(activeAnimName);
            ActivateGlow(true);

            // Deactivate last revive point.
            if (lastRevivePoint != null && lastRevivePoint != gameObject)
            {
                lastRevivePoint.GetComponent<Animator>().Play(inactiveAnimName);
                ActivateGlow(false);
            }

            currentRevivePoint = gameObject;
            lastRevivePoint = gameObject;

            OnActivateRevivePoint?.Invoke();

            if (showVisuals)
            {
                audioSource.Play();

                GameObject sfx = Instantiate(activateSFXPrefab);
                sfx.transform.position = new Vector2(transform.position.x, transform.position.y + 3);
                sfx.GetComponent<ParticleSystemRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
                Destroy(sfx, sfx.GetComponent<ParticleSystem>().main.duration);
            }
        }
    }

    public void ActivateGlow(bool val)
    {
        glowChildObject.SetActive(val);
        if (val)
        {
            glowChildObject.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
        }
    }

    private void RevivePlayer()
    {
        if (IsCurrentRevivePoint())
        {
            if (IsCurrentRevivePointInActiveScene())
            {
                PlayerPawn player = FindObjectOfType<PlayerPawn>();
                if (player == null)
                {
                    player = Instantiate(playerPrefab).GetComponent<PlayerPawn>();
                }

                //RevivePointLoadPlayerData(player);
                player.transform.position = GetCurrentRevivePointPosition();
                player.RevivePlayer();
            }
            else
            {
                revivePlayerInOtherScene = true;
                //SceneManager.LoadScene(RevivePointRepository.CurrentRevivePointSceneName);
            }
        }
    }

    private bool IsCurrentRevivePoint() => 
        GameObject.Find(RevivePointRepository.CurrentRevivePointGOName) == gameObject;

    private bool IsCurrentRevivePointInActiveScene() =>
        RevivePointRepository.CurrentRevivePointSceneName == SceneManager.GetActiveScene().name;

    private Vector3 GetCurrentRevivePointPosition()
    {
        GameObject revivePoint = GameObject.Find(RevivePointRepository.CurrentRevivePointGOName);
        if (revivePoint != null)
        {
            return revivePoint.transform.position;
        }
        // TODO: What to return if it fails to find loaded revive point?
        return currentRevivePoint.transform.position;
    }
    #endregion
}
