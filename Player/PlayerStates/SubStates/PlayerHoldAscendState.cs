using UnityEngine;

public class PlayerHoldAscendState : PlayerAbilityState
{
    private bool isHolding;
    private bool ascendHit;
    private LineRenderer lineRenderer;

    public PlayerHoldAscendState(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();

        lineRenderer = player.GetComponent<LineRenderer>();
        if (lineRenderer)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, CollisionSenses.GetWallCheckTransform().position);
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }
        else
        {
            Debug.LogError("PlayerHoldAscendState:Enter: Failed to get LineRenderer component.");
        }

        isHolding = true;
        ascendHit = false;
        player.InputHandler.UseHoldWarpInput();
        player.Animator.SetBool(AnimationConstants.ANIM_PARAM_HOLD_ASCEND, true);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (isHolding && lineRenderer)
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, CollisionSenses.GetWallCheckTransform().position);

                Vector3 direction = Vector3.up;
                if (player.InputHandler.RawWarpDirectionInput.y > 0)
                {
                    direction = Vector3.up;
                }
                else if (player.InputHandler.RawWarpDirectionInput.y < 0)
                {
                    direction = Vector3.down;
                }

                RaycastHit2D hit = Physics2D.Raycast(player.transform.position, (Vector2)direction, playerStateData.ascendRayDistance, playerStateData.groundLayer);

                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.GetComponent<WarpableObject>())
                    {
                        ascendHit = true;
                        lineRenderer.material.color = Color.green;
                    }
                    else
                    {
                        lineRenderer.material.color = Color.red;
                    }
                    lineRenderer.SetPosition(1, hit.point);
                }
                else
                {
                    ascendHit = false;
                    lineRenderer.material.color = Color.red;
                    lineRenderer.SetPosition(1, player.transform.position + direction * playerStateData.ascendRayDistance);
                }

                if (player.InputHandler.HoldWarpInputStop)
                {
                    lineRenderer.enabled = false;
                    isHolding = false;
                    isAbilityDone = true;
                    player.Animator.SetBool(AnimationConstants.ANIM_PARAM_HOLD_ASCEND, false);

                    if (ascendHit)
                    {
                        stateMachine.ChangeState(player.AscendState);
                    }
                    else
                    {
                        stateMachine.ChangeState(player.IdleState);
                    }
                }
            }
            if (CollisionSenses.Ground)
            {
                Movement.SetVelocityX(0f);
            }
        }
    }
}
