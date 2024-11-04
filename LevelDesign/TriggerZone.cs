using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    [SerializeField] GameObject behaviourGameObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!behaviourGameObject)
        {
            Debug.LogError("TriggerZone::OnTriggerEnter2D: No trigger behaviour game object defined for " + gameObject.name);
            return;
        }

        TriggerBehaviour triggerBehaviour = behaviourGameObject.GetComponent<TriggerBehaviour>();
        if (!triggerBehaviour)
            return;

        triggerBehaviour.RunBehaviour();
    }
}
