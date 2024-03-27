using UnityEngine;

public class PlayerWarpDashState : PlayerDashState
{
    private Vector2 chestPos;

    public PlayerWarpDashState(Player player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
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
        if (hit.collider && !hit.collider.GetComponent<TimedPlatform>())
        {
            destination = new Vector2(hit.point.x, hit.point.y - (CollisionSenses.GetChestTransform().position.y - player.transform.position.y));
        }
        else
        {
            Vector3 newPos = player.transform.position + (Vector3)(Movement.FacingDirection * playerStateData.dashDistance * Vector2.right);
            destination = new Vector3(newPos.x, player.transform.position.y, player.transform.position.z);
        }
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

    private void EndWarp()
    {
        CapsuleCollider2D capsuleCollider2D = player.GetComponent<CapsuleCollider2D>();
        if (capsuleCollider2D) { capsuleCollider2D.enabled = true; }

        BoxCollider2D boxCollider2D = player.GetComponent<BoxCollider2D>();
        if (boxCollider2D) { boxCollider2D.enabled = true; }

        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
        if (spriteRenderer) { spriteRenderer.enabled = true; }

        Rigidbody2D rigidbody2D = player.GetComponent<Rigidbody2D>();
        if (rigidbody2D) { rigidbody2D.WakeUp(); }
    }

    private void WarpBlink()
    {
        GameObject tempGO = GameObject.Instantiate(playerStateData.dashVFXPrefab);
        tempGO.transform.position = chestPos;
        tempGO = GameObject.Instantiate(playerStateData.dashSFXPrefab);
        tempGO.transform.position = player.transform.position;

        // Shoot raycast FROM blink destination. If it hits ground, tp player and play blin sound there and keep ascending.
        // If not hit, just tp player there.

        Vector2 destination;
        Vector2 origin = chestPos + new Vector2(playerStateData.dashDistance * Movement.FacingDirection, player.transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.left * Movement.FacingDirection, playerStateData.dashDistance, playerStateData.groundLayer);
        if (hit.collider && !hit.collider.GetComponent<TimedPlatform>())
        {
            destination = new Vector2(hit.point.x, hit.point.y - (CollisionSenses.GetChestTransform().position.y - player.transform.position.y));
            EndWarp();
        }
        else
        {
            Vector3 newPos = player.transform.position + (Vector3)(Movement.FacingDirection * playerStateData.dashDistance * Vector2.right);
            player.transform.position = new Vector3(newPos.x, player.transform.position.y, player.transform.position.z);

            tempGO = GameObject.Instantiate(playerStateData.dashVFXPrefab);
            tempGO.transform.position = CollisionSenses.GetChestTransform().position;
        }
    }
}
