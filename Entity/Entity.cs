using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public EntityCore Core { get; protected set; }
    public Animator Animator { get; private set; }

    protected SpriteRenderer spriteRenderer;
    protected Vector2 velocityWorkspace;
    protected Material matDefault;
    protected Material matWhite;

    protected Combat Combat { get => combat ?? Core.GetCoreComponent(ref combat); }
    protected Combat combat;
    protected Movement Movement { get => movement ?? Core.GetCoreComponent(ref movement); }
    protected Movement movement;
    protected Stats Stats { get => stats ?? Core.GetCoreComponent(ref stats); }
    protected Stats stats;

    #region Unity Callback Functions
    protected virtual void Awake()
    {
        Core = GetComponentInChildren<EntityCore>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        matWhite = Resources.Load(EditorConstants.RESOURCE_WHITE_FLASH, typeof(Material)) as Material;
    }

    protected virtual void Start()
    {
        matDefault = spriteRenderer.material;
    }

    protected virtual void Update()
    {
        if (Core != null) Core.LogicUpdate();
    }
    #endregion

    #region Other Functions
    public virtual void TakeDamage(Types.DamageData damageData)
    {
        if (Stats.currentHealth > 0)
        {
            StartCoroutine(FlashWhiteMaterial(0.1f));
            Combat.TakeDamage(damageData);
            if (stats.currentHealth <= 0)
            {
                Death();
                return;
            }

            if (damageData.source.CompareTag(EditorConstants.TAG_PLAYER) && !gameObject.CompareTag(EditorConstants.TAG_PLAYER))
            {
                Entity entity = damageData.source.GetComponent<Entity>();
                if (entity)
                {
                    entity.Stats.OnDealtDamage();
                }
            }
        }
    }

    protected virtual void Death() {}

    public virtual void Revive() {}

    public int GetFacingDirection() => Movement.FacingDirection;

    protected virtual IEnumerator FlashWhiteMaterial(float delay = 0f)
    {
        spriteRenderer.material = matWhite;
        yield return new WaitForSeconds(delay);
        spriteRenderer.material = matDefault;
    }
    #endregion
}
