using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    public bool CanDash { get; protected set; }
    protected float lastDashTime;
    protected Vector2 destination;
    private Vector2 dashDirection;
    private PlayerPawn playerPawn;

    public PlayerDashState(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {
        this.playerPawn = player;
    }

    public override void Enter()
    {
        base.Enter();
        CanDash = false;
        player.InputHandler.UseDashInput();
        dashDirection = Vector2.right * Movement.FacingDirection;
        playerPawn.SetIgnoreEnemyLayerCollisoon(true);
        playerPawn.dashDamageCollider.SetActive(true);

        /*Vector2 chestPos = CollisionSenses.GetChestTransform().position;
        GameObject tempGO = GameObject.Instantiate(playerStateData.dashVFXPrefab);
        tempGO.transform.position = chestPos;
        GameFunctionLibrary.PlayAudioAtPosition(playerStateData.dashAudioClip, player.transform.position);

        RaycastHit2D hit = Physics2D.Raycast(chestPos, Vector2.right * Movement.FacingDirection, playerStateData.dashDistance, playerStateData.groundLayer);
        if (hit.collider && !hit.collider.GetComponent<TimedPlatform>())
        {
            destination = new Vector2(hit.point.x, hit.point.y - (CollisionSenses.GetChestTransform().position.y - player.transform.position.y));
        }
        else
        {
            Vector3 newPos = player.transform.position + (Vector3)(Movement.FacingDirection * playerStateData.dashDistance * Vector2.right);
            destination = new Vector3(newPos.x, player.transform.position.y, player.transform.position.z);
        }*/
    }

    public override void Exit()
    {
        base.Exit();
        playerPawn.SetIgnoreEnemyLayerCollisoon(false);
        playerPawn.dashDamageCollider.SetActive(false);
        isAbilityDone = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isExitingState)
        {
            // Dashing
            Movement.SetVelocity(playerStateData.dashVelocity, dashDirection);
            if (Time.time >= startTime + playerStateData.dashTime)
            {
                Movement.SetDrag(0f);
                isAbilityDone = true;
                lastDashTime = Time.time;
            }
        }

        /*base.LogicUpdate();

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
        }*/

        if (player.InputHandler.BarrierInput)
            player.ActivateBarrier();
    }

    public virtual bool CheckIfCanDash()
    {
        return CanDash && Time.time >= lastDashTime + playerStateData.dashCooldown;
    }

    public virtual void ResetCanDash() => CanDash = true;
}
