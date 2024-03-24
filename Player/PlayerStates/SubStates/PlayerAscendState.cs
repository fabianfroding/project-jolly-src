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

    public PlayerAscendState(Player player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();

        capsuleCollider2D = player.GetComponent<CapsuleCollider2D>();
        boxCollider2D = player.GetComponent<BoxCollider2D>();
        spriteRenderer = player.GetComponent<SpriteRenderer>();
        rigidBody2D = player.GetComponent<Rigidbody2D>();

        direction = Vector3.up;
        if (player.InputHandler.RawWarpDirectionInput.y > 0)
        {
            direction = Vector3.up;
        }
        else if (player.InputHandler.RawWarpDirectionInput.y < 0)
        {
            direction = Vector3.down;
        }

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

        EndWarp();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.transform.position = player.transform.position + direction * (isAscending ? playerStateData.warpInGroundSpeed : playerStateData.warpInAirSpeed);
        bool collisionSensesDirectionCheck = false;
        if (direction == Vector3.up)
        {
            collisionSensesDirectionCheck = CollisionSenses.WallUp;
        }
        else if (direction == Vector3.down)
        {
            collisionSensesDirectionCheck = CollisionSenses.WallDown;
        }
        
        if (collisionSensesDirectionCheck && !isAscending)
        {
            isAscending = true;
            capsuleCollider2D.enabled = false;
            boxCollider2D.enabled = false;
            spriteRenderer.enabled = false;
            rigidBody2D.Sleep();

            GameObject tempGO = GameObject.Instantiate(playerStateData.ascendDiveInVFX);
            tempGO.transform.position = direction == Vector3.down ? player.transform.position : CollisionSenses.GetCeilingCheck().position; 

            tempGO = GameObject.Instantiate(playerStateData.ascendDiveInSFX);
            tempGO.transform.position = player.transform.position;

            warpActiveSFX = GameObject.Instantiate(playerStateData.warpActiveSFX);
            warpActiveSFX.transform.SetParent(player.transform);
        }

        if (isAscending)
        {
            Transform wallCheckOrigin = player.transform;
            if (direction == Vector3.down)
            {
                wallCheckOrigin = CollisionSenses.GetCeilingCheck();
            }

            RaycastHit2D hit = Physics2D.Raycast(wallCheckOrigin.position, direction, playerStateData.ascendDiveInRayDistance, playerStateData.groundLayer);
            if (hit.collider == null)
            {
                GameObject tempGO = GameObject.Instantiate(playerStateData.ascendDiveInVFX);
                tempGO.transform.position = wallCheckOrigin.position;

                tempGO = GameObject.Instantiate(playerStateData.ascendEmergeSFX);
                tempGO.transform.position = player.transform.position;

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
        rigidBody2D.velocity = new Vector2(0f, 10f); // TODO: Make to a design variable and make it negative if traveling downwards.
    }
}
