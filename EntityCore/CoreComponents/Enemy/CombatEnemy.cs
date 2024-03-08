using UnityEngine;

public class CombatEnemy : Combat
{
    [SerializeField] GameObject aggroSFXPrefab;
    [SerializeField] GameObject hurtSFXPrefab;
    [SerializeField] [Range(0f, 1f)] float chanceToPlayHurtSound;

    private FieldOfView FieldOfView => fieldOfView ? fieldOfView : fieldOfView = GetComponentInParent<FieldOfView>();
    private FieldOfView fieldOfView; 

    protected override void Start()
    {
        base.Start();
        fieldOfView = GetComponentInParent<FieldOfView>();
        if (!fieldOfView) { Debug.LogError("CombatEnemy::Start: Could not find FieldOfView component in parent."); }
    }

    private void OnEnable()
    {
        FieldOfView.OnAggro += InstantiateAggroSFX;
    }

    private void OnDisable()
    {
        FieldOfView.OnAggro -= InstantiateAggroSFX;
    }

    protected override void InstantiateTakeDamageVisuals()
    {
        base.InstantiateTakeDamageVisuals();

        if (hurtSFXPrefab && Random.Range(0f, 1f) <= chanceToPlayHurtSound)
        {
            GameObject hurtSFX = GameObject.Instantiate(hurtSFXPrefab);
            hurtSFX.transform.position = transform.position;
        }
    }

    private void InstantiateAggroSFX()
    {
        if (aggroSFXPrefab)
        {
            GameObject aggroSFX = GameObject.Instantiate(aggroSFXPrefab);
            aggroSFX.transform.position = transform.position;
        }
    }
}
