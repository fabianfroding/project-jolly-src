using UnityEngine;

public class EnemyStonetusk_PlayerDetectedState : PlayerDetectedState
{
    EnemyStonetusk stonetusk;

    public EnemyStonetusk_PlayerDetectedState(Enemy enemy, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetectedState stateData, EnemyStonetusk stonetusk) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.stonetusk = stonetusk;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if (performLongRangeAction)
        {
            if (Time.time >= stonetusk.ChargeState.StartTime + stonetusk.chargeStateData.chargeCooldown)
            {
                stateMachine.ChangeState(stonetusk.ChargeState);
            }
        }
        else if (!isPlayerInMaxAggroRange)
        {
            stateMachine.ChangeState(stonetusk.LookForPlayerState);
        }
        else if (!isDetectingLedge)
        {
            stonetusk.Core.GetCoreComponent<Movement>().Flip();
            stateMachine.ChangeState(stonetusk.MoveState);
        }
    }
}
