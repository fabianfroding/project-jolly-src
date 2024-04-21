using UnityEngine;

public class RangedAttackState : AttackState
{
    protected D_RangedAttackState stateData;
    protected GameObject projectile;
    protected Projectile projectileScript;
    protected Transform attackPosition;

    public RangedAttackState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, Transform attackPosition, D_RangedAttackState stateData) : base(enemy, stateMachine, animBoolName)
    {
        this.stateData = stateData;
        this.attackPosition = attackPosition;
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        projectile = GameObject.Instantiate(stateData.projectile, attackPosition.position, attackPosition.rotation);
        projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.SetCurrentDirection(Vector2.right * Movement.FacingDirection);
        projectileScript.SetSpeed(5f);
        projectileScript.SetSource(enemy.gameObject);
    }
}
