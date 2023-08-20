using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(Animator))]
public class ShinyEnemyRandomizer : MonoBehaviour
{
    public static readonly string SHINY_ENEMY_NAME_PREFIX = "Shiny";

    [Range(0.0f, 100.0f)] [Tooltip("Percentage chance that the enemy will be shiny. Setting this value to 0 will prevent it from being shiny.")]
    [SerializeField] private float shinyChance;
    [SerializeField] private GameObject sfxShinySparklesPrefab;
    [SerializeField] private AnimatorController shinyAnimationController;

    protected Stats Stats { get => stats ?? enemy.Core.GetCoreComponent(ref stats); }
    protected Stats stats;

    public bool IsShiny { get; private set; }
    private Enemy enemy;
    private Animator animator;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
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
            sfxShinySparklesPrefab != null &&
            shinyAnimationController != null)
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

    private void ChangeToShinyAnimationController() => 
        animator.runtimeAnimatorController = shinyAnimationController;

    private void SetShinyStats()
    {
        Stats.SetMaxHealth((int)(Stats.GetMaxHealth() * 1.5f));
        Stats.IncreaseHealth(Stats.GetMaxHealth());

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
