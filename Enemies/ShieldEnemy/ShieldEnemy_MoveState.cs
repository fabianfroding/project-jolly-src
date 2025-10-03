using UnityEngine;

public class ShieldEnemy_MoveState : MoveState
{
    private ShieldEnemy shieldEnemy;

    public ShieldEnemy_MoveState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName, D_MoveState stateData, ShieldEnemy shieldEnemy)
        : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.shieldEnemy = shieldEnemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isPlayerInMinAggroRange)
        {
            stateMachine.ChangeState(shieldEnemy.PlayerDetectedState);
        }
        else if (isDetectingWall || !isDetectingLedge)
        {
            shieldEnemy.IdleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(shieldEnemy.IdleState);
        }
    }
}
