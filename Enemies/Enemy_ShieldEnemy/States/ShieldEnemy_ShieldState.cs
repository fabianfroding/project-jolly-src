public class ShieldEnemy_ShieldState : State
{
    private ShieldEnemy shieldEnemy;
    private bool isPlayerInMinAggroRange = false;

    protected Combat Combat { get => combat ?? core.GetCoreComponent(ref combat); }
    protected Combat combat;

    public ShieldEnemy_ShieldState(Enemy enemy, FiniteStateMachine stateMachine, int animBoolName, ShieldEnemy shieldEnemy)
        : base(enemy, stateMachine, animBoolName)
    {
        this.shieldEnemy = shieldEnemy;
    }

    public override void Enter()
    {
        base.Enter();
        Combat.OnAttackBlocked += AttackBlocked;
        Combat.blockingEnabled = true;
    }

    public override void Exit()
    {
        Combat.blockingEnabled = false;
        Combat.OnAttackBlocked -= AttackBlocked;
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isPlayerInMinAggroRange)
        {
            stateMachine.ChangeState(shieldEnemy.ShieldState);
        }
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinAggroRange = enemy.CheckPlayerInMinAggroRange();
    }

    private void AttackBlocked()
    {
        stateMachine.ChangeState(shieldEnemy.MeleeAttackState);
    }
}
