using UnityEngine;

public class EnemyJungleBeast_ThrowState : State
{
    private readonly EnemyJungleBeast enemyJungleBeast;

    private Movement Movement { get => movement != null ? movement : core.GetCoreComponent(ref movement); }
    private Movement movement;

    public EnemyJungleBeast_ThrowState(EnemyCharacter enemy, FiniteStateMachine stateMachine, int animBoolName)
        : base(enemy, stateMachine, animBoolName)
    {
        enemyJungleBeast = (EnemyJungleBeast)enemy;
    }

    public void Throw()
    {
        GameObject thrown = GameObject.Instantiate(enemyJungleBeast.throwProjectilePrefab, enemyJungleBeast.throwPoint.transform.position, Quaternion.identity);
        Projectile projectile = thrown.GetComponent<Projectile>();

        Types.DamageData damageData = projectile.GetDamageData();
        damageData.source = enemy.gameObject;
        projectile.SetDamageData(damageData);

        projectile.Init(enemy.gameObject, new Vector2(Movement.FacingDirection, 0));

        GameFunctionLibrary.PlayAudioAtPosition(enemyJungleBeast.throwAudioClip, enemy.transform.position);
    }

    public void FinishThrow()
    {
        stateMachine.ChangeState(enemyJungleBeast.LookForPlayerState);
    }
}
