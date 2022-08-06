using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] triggeredEnemies;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(EditorConstants.TAG_PLAYER))
        {
            for (int i = 0; i < triggeredEnemies.Length; i++)
            {
                if (triggeredEnemies[i] != null)
                    triggeredEnemies[i].GetComponent<Enemy>().TriggerBehaviour(collision.gameObject);
            }
        }
    }
}
