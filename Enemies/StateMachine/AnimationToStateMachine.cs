using UnityEngine;

public class AnimationToStateMachine : MonoBehaviour
{
    public AttackState attackState;

    // Also works as end of the time-window in which the enemy can be parried.
    private void TriggerAttack()
    {
        attackState.TriggerAttack();
    }

    private void FinishAttack()
    {
        attackState.FinishAttack();
    }

    // Start of the time-window in which the enemy can be parried.
    private void TriggerParriable()
    {
        attackState.TriggerParriable();
    }
}
