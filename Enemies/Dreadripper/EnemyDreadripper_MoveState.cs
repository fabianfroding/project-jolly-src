using UnityEngine;

public class EnemyDreadripper_MoveState : FlyingMoveState
{
    private EnemyDreadripper dreadripper;

    public EnemyDreadripper_MoveState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName, D_MoveState stateData, EnemyDreadripper dreadripper) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.dreadripper = dreadripper;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enemy.AIVision.TargetPlayerCharacter)
        {
            HealthComponent targetStats = enemy.AIVision.TargetPlayerCharacter.HealthComponent;
            if (targetStats != null && targetStats.IsAlive())
            {
                if (isDetectingWall || isDetectingWallUp || isDetectingWallDown) enemy.AIVision.ResetTarget();
                else
                {
                    Vector2 dirToPlayer = (enemy.AIVision.TargetPlayerCharacter.transform.position - enemy.transform.position).normalized;
                    Movement.SetVelocity(stateData.movementSpeed, dirToPlayer);
                }
            }
        }
        else if (!enemy.AIVision.TargetPlayerCharacter)
        {
            if (!IsAtInitialPosition())
            {
                Vector3 dirToInitPos = (enemy.InitialPosition - enemy.transform.position).normalized;
                Movement.SetVelocity(stateData.movementSpeed, dirToInitPos);
            }
            else
            {
                Movement.ResetVelocity();
                if (Movement.FacingDirection == 1 != enemy.InitialFacing)
                {
                    Movement.Flip();
                }
                dreadripper.StateMachine.ChangeState(dreadripper.IdleState);
            }
        }

        if ((Movement.CurrentVelocity.x > 0 && Movement.FacingDirection == -1) ||
            (Movement.CurrentVelocity.x < 0 && Movement.FacingDirection == 1))
        {
            Movement.Flip();
        }
    }
}
