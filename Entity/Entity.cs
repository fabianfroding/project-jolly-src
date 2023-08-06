using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public EntityCore Core { get; protected set; }
    public Animator Animator { get; private set; }

    protected SpriteRenderer spriteRenderer;
    protected Vector2 velocityWorkspace;

    protected Combat Combat => combat ? combat : Core.GetCoreComponent(ref combat);
    protected Combat combat;
    protected Movement Movement => movement ? movement : Core.GetCoreComponent(ref movement);
    protected Movement movement;
    public Stats Stats => stats ? stats : Core.GetCoreComponent(ref stats);
    protected Stats stats;

    #region Unity Callback Functions
    protected virtual void Awake()
    {
        Core = GetComponentInChildren<EntityCore>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
    }

    protected virtual void Start() {}

    protected virtual void Update()
    {
        if (Core != null) Core.LogicUpdate();
    }
    #endregion

    #region Other Functions

    protected virtual void Death() {}

    public virtual void Revive() {}

    public int GetFacingDirection() => Movement.FacingDirection;
    #endregion
}
