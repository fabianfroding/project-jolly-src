using System.Collections;
using UnityEngine;

public class FloatingBubbleSpawner : MonoBehaviour
{
    [SerializeField] private Vector2 bubbleVelocity = Vector2.up;
    [SerializeField] private float spawnFrequency = 6.5f;
    [SerializeField] private float bubbleDuration = 5f;
    [SerializeField] private GameObject bubbleObject; // TODO: Make into scriptable object.

    private void Start()
    {
        if (!bubbleObject)
        {
            Debug.LogError("FloatingBubbleSpawner:Start: bubbleObject is not set.");
            return;
        }

        StartCoroutine(SpawnBubble());
    }

    private void OnDisable()
    {
        StopCoroutine(SpawnBubble());
    }

    private IEnumerator SpawnBubble()
    {
        yield return new WaitForSeconds(spawnFrequency);

        GameObject tempGo = GameObject.Instantiate(bubbleObject);
        tempGo.transform.position = transform.position;
        
        Rigidbody2D rigidbody2D = tempGo.GetComponent<Rigidbody2D>();
        if (rigidbody2D)
        {
            rigidbody2D.linearVelocity = bubbleVelocity;
        }

        Destroy(tempGo, bubbleDuration);

        StartCoroutine(SpawnBubble());
    }
}
