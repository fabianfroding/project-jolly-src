using UnityEngine;

public class AnimationToStateMachine : MonoBehaviour
{
    public AttackState attackState;
    public TurnState turnState;

    // Also works as end of the time-window in which the enemy can be parried.
    private void TriggerAttack()
    {
        attackState?.TriggerAttack();
    }

    private void FinishAttack()
    {
        attackState?.FinishAttack();
    }

    // Start of the time-window in which the enemy can be parried.
    private void TriggerParriable()
    {
        attackState?.TriggerParriable();
    }

    private void FinishTurn()
    {
        turnState?.FinishTurn();
    }
}
