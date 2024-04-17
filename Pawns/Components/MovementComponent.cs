using UnityEngine;

public class MovementComponent : ActorComponent, ILogicUpdate
{
    private Rigidbody2D rb2D;
    private Vector2 currentVelocity;

    #region Unity Callbacks
    protected override void Awake()
    {
        base.Awake();
        rb2D = GetComponentInParent<Rigidbody2D>();
    }
    #endregion

    public void LogicUpdate() => currentVelocity = rb2D.velocity;
}
