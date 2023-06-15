using UnityEngine;

public class EnemyHarvester_PlayerDetectedState : PlayerDetectedState
{
    private EnemyHarvester harvester;

    public EnemyHarvester_PlayerDetectedState(Enemy enemy, FiniteStateMachine stateMachine, int animBoolName, D_PlayerDetectedState stateData, EnemyHarvester harvester) : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.harvester = harvester;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (performCloseRangeAction)
        {
            if (Time.time >= harvester.DodgeState.StartTime + harvester.dodgeStateData.dodgeCooldown)
            {
                stateMachine.ChangeState(harvester.DodgeState);
            }
            else
            {
                stateMachine.ChangeState(harvester.MeleeAttackState);
            }
        }
        else if (performLongRangeAction)
        {
            //stateMachine.ChangeState(harvester.chargeState); // Enable this for charge instead of ranged attack.
            stateMachine.ChangeState(harvester.RangedAttackState);
        }
        else if (!isPlayerInMaxAggroRange)
        {
            stateMachine.ChangeState(harvester.LookForPlayerState);
        }
        else if (!isDetectingLedge)
        {
            harvester.Core.GetCoreComponent<Movement>().Flip();
            stateMachine.ChangeState(harvester.MoveState);
        }
    }
}
