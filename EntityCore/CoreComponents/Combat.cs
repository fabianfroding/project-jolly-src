using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{
    [Header("Knockback Settings")]
    [SerializeField] protected float maxKnockbackTime = 0.2f;
    protected bool isKnockbackActive = false;
    protected float knockbackStartTime;

    [Header("Stun Settings")]
    [Tooltip("Check to allow the entity to recieve stun damage.")]
    [SerializeField] protected bool stunnable = false;
    [Tooltip("The time-window in which an enity can get stunned after it has become vulnerable to stun damage.")]
    [SerializeField] protected float stunTimeWindow = 2f;

    [Header("Parry Settings")]
    [Tooltip("Check to allow an enemy to be parried. This field is for Enemies only. " +
        "Requires setup of appropriate animation parameters TriggerParriable and Trigger Attack, " +
        "which represents the time-window that the enemy can be parried.")]
    [SerializeField] protected bool canBeParried = false;

    // Time-window between the attacking-part of a melee attack animation.
    public bool IsInTriggeredParriedAnimationFrames { get; set; }

    public bool IsStunned { get; protected set; }
    public float LastStunDamageTime { get; protected set; }
    protected float currentStunResistance;

    [Header("Spawned Objects")]
    public GameObject parriedSoundPrefab;
    [SerializeField] protected GameObject takeDmgSFXPrefab;
    [SerializeField] protected GameObject takeDmgSoundPrefab;

    protected Stats Stats { get => stats ?? core.GetCoreComponent(ref stats); }
    protected CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }
    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }

    protected Stats stats;
    protected CollisionSenses collisionSenses;
    protected Movement movement;

    public override void LogicUpdate() => CheckKnockback();

    public void TakeDamage(GameObject source, Damage damage, GameObject damagingObject = null)
    {
        if (Stats.currentHealth > 0)
        {
            Stats.DecreaseHealth(damage.damageAmount);
            InstantiateTakeDamageVisuals(damage);
            ApplyKnockback(source, damage, damagingObject);
            //Debug.Log(gameObject.name + " took " + damage.amount + " damage from " + source.name);
        }
    }

    protected void InstantiateTakeDamageVisuals(Damage damage)
    {
        if (takeDmgSoundPrefab != null && damage.damageRange != Damage.DAMAGE_RANGE.RANGED)
        {
            GameObject takeDmgSound = Instantiate(takeDmgSoundPrefab, transform.position, Quaternion.identity);
            Destroy(takeDmgSound, takeDmgSound.GetComponent<AudioSource>() != null ? takeDmgSound.GetComponent<AudioSource>().clip.length : 0f);
        }

        if (takeDmgSFXPrefab != null)
        {
            GameObject takeDmgSFX = Instantiate(takeDmgSFXPrefab);
            takeDmgSFX.transform.position = transform.position;
            takeDmgSFX.transform.Rotate(0, 0, 90);
            Destroy(takeDmgSFX, takeDmgSFX.GetComponent<ParticleSystem>() != null ? takeDmgSFX.GetComponent<ParticleSystem>().main.duration : 0f);
        }
    }

    protected bool IsFlyingEnemy()
    {
        Enemy enemy = GetComponentInParent<Enemy>();
        return enemy != null && enemy.enemyData.isFlying;
    }

    protected void ApplyKnockback(GameObject source, Damage damage, GameObject sourceObject)
    {
        if (sourceObject != null)
        {
            DamagingObject damagingObject = sourceObject.GetComponent<DamagingObject>();
            if (damagingObject.IsProjectile())
            {
                Vector2 dir = TrigonometryUtils.GetDirectionBetweenPositions(damagingObject.transform, transform);
                Knockback(damage.knockbackAngle, damage.knockbackStrength, dir.x >= 0 ? 1 : -1);
            }
            else
            {
                Vector2 dir = TrigonometryUtils.GetDirectionBetweenPositions(source.transform, transform);
                Knockback(damage.knockbackAngle, damage.knockbackStrength, dir.x >= 0 ? 1 : -1);
            }
        }
        else
        {
            Vector2 dir = TrigonometryUtils.GetDirectionBetweenPositions(source.transform, transform);
            Knockback(damage.knockbackAngle, damage.knockbackStrength, dir.x >= 0 ? 1 : -1);
        }
    }

    public virtual void Knockback(Vector2 angle, float strength, int direction)
    {
        Movement.SetVelocity(strength, angle, direction);
        Movement.CanSetVelocity = false;
        isKnockbackActive = true;
        knockbackStartTime = Time.time;
    }

    protected void CheckKnockback()
    {
        if (isKnockbackActive && 
            (IsFlyingEnemy() || Movement.CurrentVelocity.y < 0.01f) && 
            (CollisionSenses.Ground || Time.time >= knockbackStartTime + maxKnockbackTime))
        {
            isKnockbackActive = false;
            Movement.CanSetVelocity = true;
        }
    }

    public void SetCurrentStunResistance(float amount) => currentStunResistance = amount;
    public void ApplyStunDamage(float amount)
    {
        if (stunnable)
        {
            LastStunDamageTime = Time.time;
            currentStunResistance -= amount;
            if (currentStunResistance <= 0)
            {
                IsStunned = true;
            }
        }
    }

    public void ResetStunResistance()
    {
        IsStunned = false;
        currentStunResistance = Stats.GetStunResistance();
    }

    public void CheckStunRecoveryTime()
    {
        if (Time.time >= LastStunDamageTime + stunTimeWindow)
        {
            ResetStunResistance();
        }
    }

    public bool CanBeParried()
    {
        IParriable parriable = GetComponentInParent<IParriable>();
        if (parriable != null)
        {
            if (IsInTriggeredParriedAnimationFrames)
            {
                parriable.Parried();
                InstantiateParryVisuals();
                return true;
            }
        }
        return false;
    }

    protected void InstantiateParryVisuals()
    {
        if (parriedSoundPrefab != null)
        {
            GameObject parrySound = Instantiate(parriedSoundPrefab);
            parrySound.transform.position = transform.position;
        }

        // TODO: Instantiate parrySFXPrefab.
    }
}
