using UnityEngine;

public class PlayerAscendState : PlayerAbilityState
{
    private bool isAscending;
    private CapsuleCollider2D capsuleCollider2D;
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody2D;
    private Vector3 direction = Vector3.up;
    private GameObject warpActiveSFX;

    private Quaternion originalRotation; // Store the player's original rotation

    public PlayerAscendState(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();

        capsuleCollider2D = player.GetComponent<CapsuleCollider2D>();
        boxCollider2D = player.GetComponent<BoxCollider2D>();
        spriteRenderer = player.GetComponent<SpriteRenderer>();
        rigidBody2D = player.GetComponent<Rigidbody2D>();

        direction = player.InputHandler.RawWarpDirectionInput.normalized;

        RotatePlayerToDirection(direction);
        isAscending = false;
        player.Animator.SetBool(AnimationConstants.ANIM_PARAM_ASCEND, true);
    }

    public override void Exit()
    {
        base.Exit();

        if (warpActiveSFX)
        {
            GameObject.Destroy(warpActiveSFX);
        }

        ResetPlayerRotation();
        EndWarp();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.transform.position = player.transform.position + direction * (isAscending ? playerStateData.warpInGroundSpeed : playerStateData.warpInAirSpeed);

        RaycastHit2D hit1 = Physics2D.Raycast(player.transform.position, direction, 1.0f, playerStateData.groundLayer);

        bool collisionSensesDirectionCheck = hit1.collider != null;
        
        if (collisionSensesDirectionCheck && !isAscending)
        {
            isAscending = true;
            capsuleCollider2D.enabled = false;
            boxCollider2D.enabled = false;
            spriteRenderer.enabled = false;
            rigidBody2D.Sleep();

            GameObject tempGO = GameObject.Instantiate(playerStateData.ascendDiveInVFX);
            tempGO.transform.position = direction == Vector3.down ? player.transform.position : CollisionSenses.GetCeilingCheck().position; 

            GameFunctionLibrary.PlayAudioAtPosition(playerStateData.ascendDiveInAudioClip, player.transform.position);

            warpActiveSFX = GameObject.Instantiate(playerStateData.warpActiveSFX);
            warpActiveSFX.transform.SetParent(player.transform);
        }

        if (isAscending)
        {
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, direction, playerStateData.ascendDiveInRayDistance, playerStateData.groundLayer);
            if (hit.collider == null)
            {
                GameObject tempGO = GameObject.Instantiate(playerStateData.ascendDiveInVFX);
                tempGO.transform.position = player.transform.position;

                GameFunctionLibrary.PlayAudioAtPosition(playerStateData.ascendEmergeAudioClip, player.transform.position);

                EndWarp();
            }
        }
    }

    private void EndWarp()
    {
        isAscending = false;
        isAbilityDone = true;
        capsuleCollider2D.enabled = true;
        boxCollider2D.enabled = true;
        spriteRenderer.enabled = true;
        rigidBody2D.WakeUp();
        player.Animator.SetBool(AnimationConstants.ANIM_PARAM_ASCEND, false);
        rigidBody2D.linearVelocity = direction * 10f;
    }

    private void RotatePlayerToDirection(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= 90f;
        player.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void ResetPlayerRotation()
    {
        player.transform.rotation = originalRotation;
    }
}
