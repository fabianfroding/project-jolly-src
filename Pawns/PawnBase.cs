using UnityEngine;

public class PawnBase : Actor
{
    #region Component Variables
    protected HealthComponent healthComponent;
    protected MovementComponent movementComponent;
    #endregion

    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    #region Unity Callbacks
    protected override void Awake()
    {
        base.Awake();

        healthComponent = GetComponentInChildren<HealthComponent>();
        movementComponent = GetComponentInChildren<MovementComponent>();

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();
    }
    #endregion

    public HealthComponent GetHealthComponent()
    {
        if (healthComponent)
            return healthComponent;
        healthComponent = GetComponentInChildren<HealthComponent>();
        if (healthComponent)
            return healthComponent;
        Debug.LogWarning("PawnBase::GetHealthComponent: Could not find a HealthComponent in pawn " + gameObject.name);
        return null;
    }

    public MovementComponent GetMovementComponent()
    {
        if (movementComponent)
            return movementComponent;
        movementComponent = GetComponentInChildren<MovementComponent>();
        if (movementComponent)
            return movementComponent;
        Debug.LogWarning("PawnBase::GetMovementComponent: Could not find a MovementComponent in pawn " + gameObject.name);
        return null;
    }
}
