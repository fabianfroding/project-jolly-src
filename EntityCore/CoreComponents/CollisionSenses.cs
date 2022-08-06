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

    public float GroundCheckRadius { get => groundCheckRadius; private set => groundCheckRadius = value; }
    public float LedgeCheckDistance { get => ledgeCheckDistance; private set => ledgeCheckDistance = value; }
    public float WallCheckDistance { get => wallCheckDistance; private set => wallCheckDistance = value; }
    public LayerMask GroundLayer { get => groundLayer; private set => groundLayer = value; }

    [SerializeField] private Transform[] groundChecks;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float ledgeCheckDistance;
    [SerializeField] private float wallCheckDistance;

    [Tooltip("Intended primarily for flying entities.")]
    [SerializeField] private float wallUpCheckDistance;
    [Tooltip("Intended primarily for flying entities.")]
    [SerializeField] private float wallDownCheckDistance;

    private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    private Movement movement;

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
