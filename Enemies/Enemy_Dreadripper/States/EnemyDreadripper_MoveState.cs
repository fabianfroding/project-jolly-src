using UnityEngine;

public class EnemyDreadripper_MoveState : FlyingMoveState
{
    private EnemyDreadripper dreadripper;

    public EnemyDreadripper_MoveState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_MoveState stateData, EnemyDreadripper dreadripper) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.dreadripper = dreadripper;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enemy.Target != null)
        {
            HealthComponent targetStats = enemy.Target.GetComponentInChildren<HealthComponent>();
            if (targetStats != null && targetStats.IsAlive())
            {
                if (isDetectingWall || isDetectingWallUp || isDetectingWallDown) enemy.ResetTarget();
                else
                {
                    Vector2 dirToPlayer = (enemy.Target.transform.position - enemy.transform.position).normalized;
                    Movement.SetVelocity(stateData.movementSpeed, dirToPlayer);
                }
            }
        }
        else if (enemy.Target == null)
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
