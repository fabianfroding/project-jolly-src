using UnityEngine;

public class BlockState : State
{
    public D_BlockState stateData;
    protected bool isPlayerInMinAggroRange = false;

    protected Combat Combat => combat ? combat : core.GetCoreComponent(ref combat);
    protected Combat combat;

    public BlockState(Enemy enemy, FiniteStateMachine stateMachine, int animBoolName, D_BlockState stateData)
        : base(enemy, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        Combat.blockingEnabled = true;
    }

    public override void Exit()
    {
        Combat.blockingEnabled = false;
        base.Exit();
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinAggroRange = enemy.CheckPlayerInMinAggroRange();
    }

    protected virtual void AttackBlocked()
    {
        if (stateData.sfxBlockPrefab)
        {
            GameObject sfxBlockInstance = GameObject.Instantiate(stateData.sfxBlockPrefab);
            sfxBlockInstance.transform.position = enemy.transform.position;
        }
    }
}
