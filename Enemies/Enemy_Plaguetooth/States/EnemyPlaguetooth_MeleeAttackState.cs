using UnityEngine;

public class EnemyPlaguetooth_MeleeAttackState : MeleeAttackState
{
    private EnemyPlaguetooth plaguetooth;
    
    public EnemyPlaguetooth_MeleeAttackState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, Transform attackPosition, D_MeleeAttackState stateData, EnemyPlaguetooth plaguetooth) 
        : base(enemy, stateMachine, animBoolName, attackPosition, stateData)
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

    public override void TriggerAttack()
    {
        base.TriggerAttack();
        CreateSlash();
    }

    private void CreateSlash()
    {
        if (stateData.slashHorizontalPrefab != null)
        {
            Vector2 spawnPos = new Vector2(attackPosition.position.x + Movement.FacingDirection * stateData.spawnPosOffsetX, attackPosition.position.y);
            GameObject slash = GameObject.Instantiate(stateData.slashHorizontalPrefab, spawnPos, Quaternion.identity);
            slash.transform.localScale = new Vector2(slash.transform.localScale.x * Movement.FacingDirection, slash.transform.localScale.y);
            DamagingObject damagingObject = slash.GetComponent<DamagingObject>();
            if (damagingObject)
                damagingObject.SetSource(plaguetooth.gameObject);
            GameObject.Destroy(slash, 0.04f);
        }
    }
}
