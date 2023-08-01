using UnityEngine;

public class CollisionSenses : CoreComponent
{
    public Transform[] GroundCheck {
        get => GenericNotImplementedError<Transform[]>.TryGet(groundChecks, core.transform.parent.name);
        private set => groundChecks = value;
    }

    public Transform LedgeCheck
    {
        get => GenericNotImplementedError<Transform>.TryGet(ledgeCheck, core.transform.parent.name);
        private set => ledgeCheck = value;
    }

    public Transform WallCheck 
    {
        get => GenericNotImplementedError<Transform>.TryGet(wallCheck, core.transform.parent.name);
        private set => wallCheck = value; 
    }

    public Transform EnemyCheck
    {
        get => GenericNotImplementedError<Transform>.TryGet(wallCheck, core.transform.parent.name);
        private set => wallCheck = value;
    }

    public float GroundCheckRadius { get => groundCheckRadius; private set => groundCheckRadius = value; }
    public float LedgeCheckDistance { get => ledgeCheckDistance; private set => ledgeCheckDistance = value; }
    public float WallCheckDistance { get => wallCheckDistance; private set => wallCheckDistance = value; }
    public float EnemyCheckDistance { get => enemyCheckDistance; private set => enemyCheckDistance = value; }
    public LayerMask GroundLayer { get => groundLayer; private set => groundLayer = value; }
    public LayerMask EnemyLayer { get => enemyLayer; private set => enemyLayer = value; }

    [SerializeField] private Transform[] groundChecks;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private Transform chestTransform;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float ledgeCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float enemyCheckDistance;
    [SerializeField] private LayerMask enemyLayer;

    [Tooltip("Intended primarily for flying entities.")]
    [SerializeField] private float wallUpCheckDistance;
    [Tooltip("Intended primarily for flying entities.")]
    [SerializeField] private float wallDownCheckDistance;

    private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    private Movement movement;

    public Transform GetCeilingCheck() {  return ceilingCheck; }
    public Transform GetChestTransform() { return chestTransform; }
    public Transform GetWallCheckTransform() => wallCheck;

    public bool Ground
    {
        get => CheckIfGrounded();
    }

    public bool Ledge
    {
        get => Physics2D.Raycast(ledgeCheck.position, Vector2.down, ledgeCheckDistance, groundLayer);
    }

    public bool WallFront
    {
        get => Physics2D.Raycast(wallCheck.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, groundLayer);
    }

    public bool WallBack
    {
        get => Physics2D.Raycast(wallCheck.position, Vector2.right * -Movement.FacingDirection, wallCheckDistance, groundLayer);
    }

    public bool WallUp
    {
        get => Physics2D.Raycast(wallCheck.position, Vector2.up, wallUpCheckDistance, groundLayer);
    }

    public bool WallDown
    {
        get => Physics2D.Raycast(wallCheck.position, Vector2.down, wallDownCheckDistance, groundLayer);
    }

    public bool CeilingCheck
    {
        get => Physics2D.Raycast(ceilingCheck.position, Vector2.up, 0.1f, groundLayer);
    }

    public bool EnemyFront
    {
        get => Physics2D.Raycast(wallCheck.position, Vector2.right * Movement.FacingDirection, enemyCheckDistance, enemyLayer);
    }

    private bool CheckIfGrounded()
    {
        for (int i = 0; i < groundChecks.Length; i++)
        {
            if (Physics2D.Raycast(groundChecks[i].position, Vector2.down, groundCheckRadius, groundLayer))
            {
                return true;
            }
        }
        return false;
    }
}
