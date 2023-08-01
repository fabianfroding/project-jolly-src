public class EnemyMasterHarvester_IdleState : IdleState
{
    public EnemyMasterHarvester_IdleState(Enemy enemy, FiniteStateMachine stateMachine, int animBoolName, D_IdleState stateData) 
        : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.stateMachine = stateMachine;
    }
}
