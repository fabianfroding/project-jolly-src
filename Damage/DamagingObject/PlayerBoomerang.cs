using UnityEngine;

public class PlayerBoomerang : Projectile
{
    private bool isReturning = false;
    [SerializeField] private GameObject sfxBoomerangBirthPrefab;
    [SerializeField] private GameObject sfxBoomerangCatchPrefab;

    protected override void Awake()
    {
        base.Awake();
        OnHit += Return;
    }

    protected override void Start()
    {
        base.Start();
        if (sfxBoomerangBirthPrefab)
        {
            GameObject sfxBoomerangBirth = GameObject.Instantiate(sfxBoomerangBirthPrefab);
            sfxBoomerangBirth.transform.position = transform.position;
        }
    }

    private void OnDisable()
    {
        OnHit -= Return;
    }

    private void Return()
    {
        InvertDirection();
        rb.velocity = new Vector2(rb.velocity.x * 0.67f, rb.velocity.y * 0.67f);
        isReturning = true;
    }

    protected override void ProcessCollision(GameObject other)
    {
        if (isReturning)
        {
            Player player = other.GetComponent<Player>();
            if (player)
            {
                player.InvokeOnPlayerCatchBoomerang();
                if (sfxBoomerangCatchPrefab)
                {
                    GameObject sfxBoomerangCatch = GameObject.Instantiate(sfxBoomerangCatchPrefab);
                    sfxBoomerangCatch.transform.position = transform.position;
                }
                Destroy(gameObject);
            }
        }
        else
        {
            base.ProcessCollision(other);
        }
    }
}
