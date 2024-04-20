using UnityEngine;

public class Movement : CoreComponent, IKnockbackable
{
    public MutableFloat movementSpeed;

    [Header("Knockback Settings")]
    [SerializeField] private float maxKnockbackTime = 0.2f;
    [SerializeField][Range(0f, 1f)] private float knockbackResistance = 0f;
    [SerializeField] private bool applyKnockbackOnDeath = true;
    [Tooltip("Define overriden knockback settings if the owner should have hardcoded knockback when being knocked back.")]
    [SerializeField] private bool overrideKnockbackSettings = false;
    [SerializeField] private float overridenKnockbackStrength = 0f;
    [SerializeField] private Vector2 overridenKnockbackAngle = Vector2.zero;

    private bool isKnockbackActive = false;
    private float knockbackStartTime;

    public Rigidbody2D RB { get; private set; }
    public int FacingDirection { get; private set; }
    public bool CanSetVelocity { get; set; }
    public Vector2 CurrentVelocity { get; private set; }
    private Vector2 workspace;

    private CollisionSenses CollisionSenses => collisionSenses ? collisionSenses : core.GetCoreComponent(ref collisionSenses);
    private CollisionSenses collisionSenses;

    protected override void Awake()
    {
        base.Awake();
        FacingDirection = 1;
        CanSetVelocity = true;
        RB = GetComponentInParent<Rigidbody2D>();
}

    #region Other Functions
    public override void LogicUpdate()
    {
        CheckKnockback();
        CurrentVelocity = RB.velocity;
    }
    
    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        SetFinalVelocity();
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        workspace = direction * velocity;
        SetFinalVelocity();
    }

    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        SetFinalVelocity();
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        SetFinalVelocity();
    }

    public void ResetVelocity()
    {
        workspace.Set(0f, 0f);
        CanSetVelocity = true;
        SetFinalVelocity();
    }

    public void SetDrag(float value) => RB.drag = value;

    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    public void Flip()
    {
        FacingDirection *= -1;
        RB.transform.Rotate(0.0f, 180f, 0.0f);
    }

    // Function to prevent velocity from chaing during knockback.
    private void SetFinalVelocity()
    {
        if (CanSetVelocity)
        {
            RB.velocity = workspace;
            CurrentVelocity = workspace;
        }
    }

    private bool IsFlyingEnemy()
    {
        EnemyPawn enemy = GetComponentInParent<EnemyPawn>();
        return enemy != null && enemy.enemyData.isFlying;
    }
    #endregion

    #region Knockback
    public void ApplyKnockback(Types.DamageData damageData)
    {
        if (damageData.knockbackStrength <= 0f)
            return;

        if (damageData.source != null)
        {
            Projectile damagingObject = damageData.source.GetComponent<Projectile>();
            if (damageData.ranged)
            {
                Vector2 dir = GameFunctionLibrary.GetDirectionBetweenPositions(damagingObject.transform, transform);
                Knockback(damageData.knockbackAngle, damageData.knockbackStrength, dir.x >= 0 ? 1 : -1);
            }
            else
            {
                Vector2 dir = GameFunctionLibrary.GetDirectionBetweenPositions(damageData.source.transform, transform);
                Knockback(damageData.knockbackAngle, damageData.knockbackStrength, dir.x >= 0 ? 1 : -1);
            }
        }
        else
        {
            Vector2 dir = GameFunctionLibrary.GetDirectionBetweenPositions(damageData.source.transform, transform);
            Knockback(damageData.knockbackAngle, damageData.knockbackStrength, dir.x >= 0 ? 1 : -1);
        }
    }

    public virtual void Knockback(Vector2 angle, float strength, int direction)
    {
        if (isKnockbackActive)
            return;
        if (!applyKnockbackOnDeath && !componentOwner.IsAlive())
            return;

        if (overrideKnockbackSettings)
            SetVelocity(overridenKnockbackStrength, overridenKnockbackAngle, direction);
        else
            SetVelocity(strength * Mathf.Max(0f, 1f - knockbackResistance), angle, direction);

        CanSetVelocity = false;
        isKnockbackActive = true;
        knockbackStartTime = Time.time;
    }

    private void CheckKnockback()
    {
        if (isKnockbackActive &&
            (IsFlyingEnemy() || CurrentVelocity.y < 0.01f) &&
            (CollisionSenses.Ground || Time.time >= knockbackStartTime + maxKnockbackTime))
        {
            isKnockbackActive = false;
            CanSetVelocity = true;
        }
    }
    #endregion
}
