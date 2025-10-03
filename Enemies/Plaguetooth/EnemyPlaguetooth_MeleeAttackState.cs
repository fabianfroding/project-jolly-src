using UnityEngine;

public class EnemyPlaguetooth_MeleeAttackState : MeleeAttackState
{
    private EnemyPlaguetooth plaguetooth;
    
    public EnemyPlaguetooth_MeleeAttackState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName, GameObject meleeAttackDamageHitBox, D_MeleeAttackState stateData, EnemyPlaguetooth plaguetooth) 
        : base(enemy, stateMachine, animBoolName, meleeAttackDamageHitBox, stateData)
    {
        this.plaguetooth = plaguetooth;
        this.stateData = stateData;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            if (isPlayerInMinAggroRange)
            {
                stateMachine.ChangeState(plaguetooth.PlayerDetectedState);
            }
            else if (!isPlayerInMinAggroRange)
            {
                stateMachine.ChangeState(plaguetooth.IdleState); // Originally LookForPlayerState.
            }
        }
    }
}
