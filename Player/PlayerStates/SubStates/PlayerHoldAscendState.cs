using UnityEngine;

public class PlayerHoldAscendState : PlayerAbilityState
{
    private bool isHolding;
    private bool ascendHit;
    private LineRenderer lineRenderer;

    private Vector2 directionInput;

    public PlayerHoldAscendState(PlayerCharacter player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    { }

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
        player.PlayerController.UseHoldWarpInput();
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

                // Get the raw input direction from the player
                Vector2 inputDirection = player.PlayerController.RawWarpDirectionInput;

                // Normalize the direction to ensure consistent direction regardless of input magnitude
                Vector3 direction = inputDirection.normalized;

                // Perform a raycast in the direction of the input
                RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, playerStateData.ascendRayDistance, playerStateData.groundLayer);

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

                if (player.PlayerController.HoldWarpInputStop)
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
