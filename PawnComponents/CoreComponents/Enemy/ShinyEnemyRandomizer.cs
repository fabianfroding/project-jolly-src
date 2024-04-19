using UnityEngine;

public class ShinyEnemyRandomizer : MonoBehaviour
{
    public static readonly string SHINY_ENEMY_NAME_PREFIX = "Shiny";

    [Range(0.0f, 100.0f)] [Tooltip("Percentage chance that the enemy will be shiny. Setting this value to 0 will prevent it from being shiny.")]
    [SerializeField] private float shinyChance;
    [SerializeField] private GameObject sfxShinySparklesPrefab;
    //[SerializeField] private AnimatorController shinyAnimationController;

    protected HealthComponent HealthComponent { get => healthComponent != null ? healthComponent : enemy.Core.GetCoreComponent(ref healthComponent); }
    protected HealthComponent healthComponent;

    public bool IsShiny { get; private set; }
    private EnemyPawn enemy;
    private Animator animator;

    private void Awake()
    {
        enemy = GetComponent<EnemyPawn>();
        animator = GetComponent<Animator>();
        IsShiny = false;
    }

    private void Start()
    {
        RandomizeShiny();
    }

    private void RandomizeShiny()
    {
        if (Random.Range(0.01f, 100.0f) <= shinyChance &&
            sfxShinySparklesPrefab != null /*&& shinyAnimationController != null*/)
        {
            MakeShiny();
        }
    }

    public void MakeShiny()
    {
        gameObject.name = SHINY_ENEMY_NAME_PREFIX + gameObject.name;
        ChangeToShinyAnimationController();
        SetShinyStats();
        AttachSparklesSFX();
        IsShiny = true;
    }

    private void ChangeToShinyAnimationController()
    {
        // TODO: Find an alternate way to change animations, since AnimationController requires UnityEditor.
        //animator.runtimeAnimatorController = shinyAnimationController;
    }

    private void SetShinyStats()
    {
        HealthComponent.SetMaxHealth((int)(HealthComponent.GetMaxHealth() * 1.5f));
        HealthComponent.IncreaseHealth(HealthComponent.GetMaxHealth());

        // TODO: Find a way to decrease interval between attacks/abilities.

        // TODO: Increase dropped currency amount drastically.
    }

    private void AttachSparklesSFX()
    {
        if (sfxShinySparklesPrefab != null)
        {
            GameObject sfxShinySparkles = Instantiate(sfxShinySparklesPrefab);
            sfxShinySparkles.transform.parent = transform;
            sfxShinySparkles.transform.position = transform.position;
            sfxShinySparkles.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}