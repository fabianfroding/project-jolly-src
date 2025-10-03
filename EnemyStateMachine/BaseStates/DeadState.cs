using UnityEngine;

public class DeadState : State
{
    protected D_DeadState stateData;

    protected Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
    protected Movement movement;

    public DeadState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName, D_DeadState stateData) : base(enemy, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        InstantiateDeathVFX();
        InstantiateDeathSound();
        InstantiateCorpse();
        InstantiateCurrencyDrop();

        enemy.gameObject.SetActive(false);
    }

    private void InstantiateDeathVFX()
    {
        if (stateData.deathVFXPrefab != null)
        {
            GameObject deathVFX = GameObject.Instantiate(stateData.deathVFXPrefab, enemy.transform.position, Quaternion.identity);
            deathVFX.transform.parent = null;
            deathVFX.transform.Rotate(0, 0, 90);
            GameObject.Destroy(deathVFX, deathVFX.GetComponent<ParticleSystem>() != null ? deathVFX.GetComponent<ParticleSystem>().main.duration : 0f);
        }
    }

    private void InstantiateDeathSound() =>
        GameFunctionLibrary.PlayAudioAtPosition(stateData.deathAudioClip, enemy.transform.position);

    private void InstantiateCorpse()
    {
        if (stateData.corpsePrefab != null)
        {
            GameObject corpse = GameObject.Instantiate(stateData.corpsePrefab);
            corpse.transform.position = enemy.transform.position;
            if (Movement.FacingDirection == 1) corpse.transform.Rotate(0, 180f, 0);
            corpse.GetComponent<SpriteRenderer>().sortingOrder = enemy.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }
    }

    private void InstantiateCurrencyDrop()
    {
        if (stateData.currencyDropPrefab != null)
        {
            int drops = Random.Range(1, 3);
            for (int i = 0; i < drops; i++)
            {
                GameObject currencyDrop = GameObject.Instantiate(stateData.currencyDropPrefab);
                currencyDrop.transform.position = new Vector2(
                    enemy.transform.position.x + Random.Range(0, 3),
                    enemy.transform.position.y + Random.Range(0, 3));
            }
        }
    }
}
