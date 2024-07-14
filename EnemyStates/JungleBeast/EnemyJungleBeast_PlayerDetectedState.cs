using UnityEngine;

public class EnemyJungleBeast_PlayerDetectedState : PlayerDetectedState
{
    private readonly EnemyJungleBeast enemyJungleBeast;

    public EnemyJungleBeast_PlayerDetectedState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_PlayerDetectedState stateData)
        : base(enemy, stateMachine, animBoolName, stateData)
    {
        enemyJungleBeast = (EnemyJungleBeast)enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (performLongRangeAction)
        {
            if (Random.Range(0, 2) <= 0.5f)
                stateMachine.ChangeState(enemyJungleBeast.JumpState);
            else
                stateMachine.ChangeState(enemyJungleBeast.ThrowState);
            return;
        }

        if (enemyJungleBeast.CheckPlayerInCloseRangeAction() || enemyJungleBeast.ShouldPerformCloseRangeAction())
        {
            stateMachine.ChangeState(enemyJungleBeast.MeleeAttackState);
            return;
        }

        if (!isDetectingLedge)
        {
            enemyJungleBeast.Flip();
            stateMachine.ChangeState(enemyJungleBeast.MoveState);
        }
    }
}
