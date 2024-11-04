using UnityEngine;

public class TriggerBehaviourDestroyGameObject : TriggerBehaviour
{
    [SerializeField] private GameObject objectToDestroy;

    public override void RunBehaviour()
    {
        base.RunBehaviour();

        if (!objectToDestroy)
        {
            Debug.Log("TriggerBehaviourDestroyGameObject::RunBehaviour: No game object to destroy defined for " + gameObject.name);
            return;
        }

        GameObject.Destroy(objectToDestroy);
    }
}
