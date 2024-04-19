using System.Collections.Generic;
using UnityEngine;

public class PlayerThunderState : PlayerAbilityState
{
    private LineRenderer lineRenderer;
    private List<GameObject> damagedEnemies;
    private float lastUseThunderTime;

    public PlayerThunderState(PlayerPawn player, PlayerStateMachine stateMachine, Player_StateData playerStateData, int animBoolName) : base(player, stateMachine, playerStateData, animBoolName)
    {}

    public override void Enter()
    {
        base.Enter();
        isAbilityDone = false;
        damagedEnemies = new List<GameObject>();

        lineRenderer = player.GetComponent<LineRenderer>();
        if (lineRenderer)
        {
            // TODO: If lineRenderer.GetPos(0) is colliding with ground, cancel the ability.

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, player.transform.position + Vector3.up * playerStateData.thunderHeight);
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder - 1;
            lineRenderer.material.color = Color.cyan;
            lineRenderer.enabled = true;
            RaycastHit2D hit = Physics2D.Raycast(lineRenderer.GetPosition(0), Vector2.down, playerStateData.thunderHeight, playerStateData.groundLayer);
            if (hit.collider && !hit.collider.gameObject.GetComponent<TimedPlatform>())
            {
                lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                lineRenderer.SetPosition(1, lineRenderer.GetPosition(0) + Vector3.down * playerStateData.thunderHeight);
            }
        }
        else
        {
            Debug.LogError("PlayerThunderState:Enter: Failed to get LineRenderer component.");
        }

        startTime = Time.time;
        player.InputHandler.UseThunderInput();
        player.Animator.SetBool(AnimationConstants.ANIM_PARAM_THUNDER, true);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isAbilityDone && Time.time >= startTime + playerStateData.thunderDelay)
        {
            isAbilityDone = true;
            lastUseThunderTime = Time.time;
            ThunderEffect();
        }
        if (CollisionSenses.Ground)
        {
            Movement.SetVelocityX(0f);
        }
    }

    public bool CheckIfCanuseThunder()
    {
        return Time.time < playerStateData.thunderCooldown || Time.time >= lastUseThunderTime + playerStateData.thunderCooldown;
    }

    private void ThunderEffect()
    {
        if (!isExitingState && lineRenderer)
        {
            lineRenderer.enabled = false;
            isAbilityDone = true;
            player.Animator.SetBool(AnimationConstants.ANIM_PARAM_THUNDER, false);

            if (playerStateData.thunderVFX)
            {
                GameObject tempGO = GameObject.Instantiate(playerStateData.thunderVFX);
                tempGO.transform.position = new Vector3(lineRenderer.GetPosition(1).x, lineRenderer.GetPosition(1).y, player.transform.position.z - 0.05f);
                tempGO.GetComponent<ParticleSystemRenderer>().sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder + 1;
            }

            if (playerStateData.thunderSFX)
            {
                GameObject tempGO = GameObject.Instantiate(playerStateData.thunderSFX);
                tempGO.transform.position = lineRenderer.GetPosition(1);
            }

            DealDamageInLine();
            DealDamageInRadius();

            // TODO: Use raycast to check for breakable ground and destroy it.

            stateMachine.ChangeState(player.IdleState);
        }
    }

    private void DealDamageInLine()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(
            lineRenderer.GetPosition(0), 
            Vector2.down, 
            Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1)), 
            LayerMask.GetMask(EditorConstants.LAYER_ENEMY));

        foreach (RaycastHit2D hit in hits)
        {
            EnemyPawn enemyComponent = hit.collider.GetComponent<EnemyPawn>();
            if (enemyComponent && !damagedEnemies.Contains(enemyComponent.gameObject))
            {
                damagedEnemies.Add(enemyComponent.gameObject);
                //enemyComponent.TakeDamage(playerStateData.thunderDamage);
            }
        }
    }

    private void DealDamageInRadius()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(lineRenderer.GetPosition(1), playerStateData.thunderRadius);
        foreach (Collider2D collider in colliders)
        {
            EnemyPawn enemyComponent = collider.GetComponent<EnemyPawn>();
            if (enemyComponent != null && !damagedEnemies.Contains(enemyComponent.gameObject))
            {
                damagedEnemies.Add(enemyComponent.gameObject);
                //enemyComponent.TakeDamage(playerStateData.thunderDamage);
            }

            if (collider.gameObject.GetComponent<BreakableGround>())
            {
                GameObject.Destroy(collider.gameObject);
            }
        }
    }
}
