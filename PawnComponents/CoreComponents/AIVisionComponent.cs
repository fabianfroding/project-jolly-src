using UnityEngine;

public class AIVisionComponent : CoreComponent
{
    [Tooltip("How long before the enemy loses its target and goes back to its previous state. A value of 0 will cause the enemy to follow its target forever.")]
    [SerializeField] private float holdTargetDuration = 2.5f;
    [Tooltip("Enable if the enemy should flip if its target is behind (as long as the target is not lost). The enemy will not flip if the player is on a different y-plane.")]
    [SerializeField] private bool flipIfTargetIsBehind = false;

    private float loseTargetStartTime = -1f;

    private EnemyPawn owningEnemyPawn;
    public PlayerPawn TargetPlayerPawn { get;private set; }

    private Movement Movement => movement ? movement : core.GetCoreComponent(ref movement);
    private Movement movement;

    protected override void Start()
    {
        if (componentOwner)
            owningEnemyPawn = (EnemyPawn)componentOwner;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPawn triggeringPlayerPawn = collision.GetComponent<PlayerPawn>();
        if (triggeringPlayerPawn)
        {
            TargetPlayerPawn = triggeringPlayerPawn;
            loseTargetStartTime = -1f; // If the enemy loses its target and re-aquires it before resetting, we dont want to reset it.
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerPawn triggeringPlayerPawn = collision.GetComponent<PlayerPawn>();
        if (triggeringPlayerPawn)
        {
            loseTargetStartTime = Time.time;
            Debug.Log(componentOwner.name + " lost its target.");
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (loseTargetStartTime > -1f && TargetPlayerPawn && Time.time > loseTargetStartTime + holdTargetDuration)
            ResetTarget();

        FlipIfTargetIsBehind();
    }

    public void ResetTarget()
    {
        Debug.Log(componentOwner.name + " reset its target.");
        TargetPlayerPawn = null;
        loseTargetStartTime = -1f;
    }

    private void FlipIfTargetIsBehind()
    {
        if (!flipIfTargetIsBehind)
            return;
        if (!TargetPlayerPawn)
            return;
        if (!Movement)
            return;

        // Don't flip if target is above or below the enemy.
        if (TargetPlayerPawn.transform.position.y > componentOwner.transform.position.y + owningEnemyPawn.EnemyCollider.bounds.size.y * 2f || 
            TargetPlayerPawn.transform.position.y < componentOwner.transform.position.y * owningEnemyPawn.EnemyCollider.bounds.size.y * 2f)
        {
            return;
        }

        Debug.Log("Facing dir: " + Movement.FacingDirection);
        Debug.Log("X: " + transform.position.x);
        Debug.Log("target X: " + TargetPlayerPawn.transform.position.x);

        if ((Movement.FacingDirection > 0 && TargetPlayerPawn.transform.position.x < transform.position.x) ||
            (Movement.FacingDirection < 0 && TargetPlayerPawn.transform.position.x > transform.position.x))
        {
            Debug.Log("Flip");
            Movement.Flip();
        }
    }
}
