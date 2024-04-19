using UnityEngine;

public class Movement : CoreComponent
{
    public MutableFloat movementSpeed;

    public Rigidbody2D RB { get; private set; }
    public int FacingDirection { get; private set; }
    public bool CanSetVelocity { get; set; }
    public Vector2 CurrentVelocity { get; private set; }
    private Vector2 workspace;

    protected override void Awake()
    {
        base.Awake();
        FacingDirection = 1;
        CanSetVelocity = true;
        RB = GetComponentInParent<Rigidbody2D>();
}

    #region Other Functions
    public override void LogicUpdate() => CurrentVelocity = RB.velocity;
    
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
    #endregion
}
