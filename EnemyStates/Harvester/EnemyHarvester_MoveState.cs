using UnityEngine;

public class EnemyHarvester_MoveState : MoveState
{
    private EnemyHarvester harvester;

    public EnemyHarvester_MoveState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_MoveState stateData, EnemyHarvester harvester) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.harvester = harvester;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isPlayerInMinAggroRange)
        {
            stateMachine.ChangeState(harvester.PlayerDetectedState);
        }
        else if (isDetectingWall || !isDetectingLedge)
        {
            harvester.IdleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(harvester.IdleState);
        }
    }
}
