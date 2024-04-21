using UnityEngine;

public class BlockState : State
{
    public D_BlockState stateData;
    protected bool isPlayerInMinAggroRange = false;

    protected Combat Combat => combat ? combat : core.GetCoreComponent(ref combat);
    protected Combat combat;

    public BlockState(EnemyPawn enemy, FiniteStateMachine stateMachine, int animBoolName, D_BlockState stateData)
        : base(enemy, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        Combat.blockState = Combat.defaultBlockState;
    }

    public override void Exit()
    {
        Combat.blockState = Types.EBlockState.E_BlockNone;
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
            Debug.Log("Block");
            GameObject sfxBlockInstance = GameObject.Instantiate(stateData.sfxBlockPrefab);
            sfxBlockInstance.transform.position = enemy.transform.position;
        }
    }
}
