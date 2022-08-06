using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RevivePoint : MonoBehaviour, IDamageable
{
    [SerializeField] private string inactiveAnimName;
    [SerializeField] private string activeAnimName;
    [SerializeField] private float reviveDelay = 2.5f;
    [SerializeField] private GameObject activateSFXPrefab;
    [SerializeField] private GameObject hitSoundPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject glowChildObject;

    public static event Action OnActivateRevivePoint;
    public static event Action OnRevivePointSave;

    private static bool revivePlayerInOtherScene = false;
    private static GameObject currentRevivePoint;
    private static GameObject lastRevivePoint;
    private Coroutine revivePlayerCoroutine;

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
        PlayerDeadState.OnPlayerBecomeDead += RevivePlayerDelayed;
    }

    private void Start()
    {
        if (IsCurrentRevivePointInActiveScene() &&
            RevivePointRepository.CurrentRevivePointGOName == name)
        {
            Activate(false);

            if (revivePlayerInOtherScene || FindObjectOfType<Player>() == null)
            {
                revivePlayerInOtherScene = false;
                RevivePlayer();
            }
        }
    }

    private void OnDestroy()
    {
        PlayerDeadState.OnPlayerBecomeDead -= RevivePlayerDelayed;
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
    public void TakeDamage(GameObject source, Damage damage, GameObject damagingObject = null)
    {
        GameObject hitSound = Instantiate(hitSoundPrefab);
        hitSound.transform.position = new Vector2(transform.position.x, transform.position.y);
        hitSound.transform.parent = null;

        Stats stats = source.GetComponent<Player>().Core.GetCoreComponent<Stats>();
        stats.IncreaseHealth(stats.GetMaxHealth());

        OnRevivePointSave?.Invoke();
        EnemyRepository.ResetKilledEnemies();

        Activate(true);
    }

    private void ProcessCollision(GameObject other)
    {
        DamagingObject damagingObject = other.GetComponent<DamagingObject>();
        if (damagingObject != null && 
            !damagingObject.IsProjectile() &&
            damagingObject.Source.CompareTag(EditorConstants.TAG_PLAYER))
        {
            TakeDamage(damagingObject.Source, damagingObject.GetDamage(), damagingObject.gameObject);
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

    public void RevivePlayerDelayed()
    {
        if (revivePlayerCoroutine != null)
        {
            StopCoroutine(revivePlayerCoroutine);
        }
        revivePlayerCoroutine = StartCoroutine(RevivePlayerCR());
    }

    private IEnumerator RevivePlayerCR()
    {
        yield return new WaitForSeconds(reviveDelay);
        RevivePlayer();
    }

    private void RevivePlayer()
    {
        if (IsCurrentRevivePoint())
        {
            if (IsCurrentRevivePointInActiveScene())
            {
                Player player = FindObjectOfType<Player>();
                if (player == null)
                {
                    player = Instantiate(playerPrefab).GetComponent<Player>();
                }

                RevivePointLoadPlayerData(player);
                player.transform.position = GetCurrentRevivePointPosition();
                player.Revive();
            }
            else
            {
                revivePlayerInOtherScene = true;
                SceneManager.LoadScene(RevivePointRepository.CurrentRevivePointSceneName);
            }
        }
    }

    private void RevivePointLoadPlayerData(Player player)
    {
        if (PlayerRepository.HasPlayerMaxHealth())
        {
            player.Core.GetCoreComponent<Stats>().SetMaxHealth(PlayerRepository.PlayerMaxHealth);
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
