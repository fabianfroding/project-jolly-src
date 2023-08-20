using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    public bool CanDash { get; protected set; }
    protected float lastDashTime;
    protected Vector2 destination;
    
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        CanDash = false;
        player.InputHandler.UseDashInput();
        startTime = Time.time;

        Vector2 chestPos = CollisionSenses.GetChestTransform().position;
        GameObject tempGO = GameObject.Instantiate(playerStateData.dashVFXPrefab);
        tempGO.transform.position = chestPos;
        tempGO = GameObject.Instantiate(playerStateData.dashSFXPrefab);
        tempGO.transform.position = player.transform.position;

        RaycastHit2D hit = Physics2D.Raycast(chestPos, Vector2.right * Movement.FacingDirection, playerStateData.dashDistance, playerStateData.groundLayer);
        if (hit.collider && !hit.collider.GetComponent<WalkableCloud>())
        {
            destination = new Vector2(hit.point.x, hit.point.y - (CollisionSenses.GetChestTransform().position.y - player.transform.position.y));
        }
        else
        {
            Vector3 newPos = player.transform.position + (Vector3)(Movement.FacingDirection * playerStateData.dashDistance * Vector2.right);
            destination = new Vector3(newPos.x, player.transform.position.y, player.transform.position.z);
        }
    }

    public override void Exit()
    {
        base.Exit();
        isAbilityDone = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (Time.time >= startTime + playerStateData.dashDelay)
            {
                player.transform.position = destination;
                GameObject tempGO = GameObject.Instantiate(playerStateData.dashVFXPrefab);
                tempGO.transform.position = CollisionSenses.GetChestTransform().position;
                lastDashTime = Time.time;
                isAbilityDone = true;
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }

    public virtual bool CheckIfCanDash()
    {
        return CanDash && Time.time >= lastDashTime + playerStateData.dashCooldown;
    }

    public virtual void ResetCanDash() => CanDash = true;
}
