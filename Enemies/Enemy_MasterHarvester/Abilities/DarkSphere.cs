using UnityEngine;

public class DarkSphere : Projectile
{
    private const string STOP_ROTATION_METHOD_NAME = "StopRotation";
    private const string FIRE_SPHERE_METHOD_NAME = "FireSphere";

    [SerializeField] private AudioSource fireSound;

    private bool isRotating = true;
    private bool hasFired = false;
    private Vector2 rotatePoint;

    //==================== PUBLIC ====================//
    public void Initialize(GameObject source)
    {
        Source = source;
        rotatePoint = new Vector2(source.transform.position.x, source.transform.position.y);
    }

    public void StopRotation()
    {
        isRotating = false;
        rb.velocity = Vector2.zero;
        Invoke(FIRE_SPHERE_METHOD_NAME, 0.8f);
    }

    //==================== PRIVATE ====================//
    protected override void Start()
    {
        base.Start();
        Invoke(STOP_ROTATION_METHOD_NAME, lifetime * 0.60f);
    }

    private void FixedUpdate()
    {
        if (Source == null)
        {
            Destroy(gameObject);
        }
        else if (isRotating)
        {
            // Make spheres expand outwards.
            rb.velocity = GetDirectionFromPoint(rotatePoint) * 1.03f;
            // Make spheres rotate around initial casting position.
            transform.RotateAround(rotatePoint, Vector3.forward, speed * Time.deltaTime);
        }
    }

    private void FireSphere()
    {
        if (!hasFired)
        {
            hasFired = true;
            if (CameraManager.IsGameObjectInCameraView(gameObject))
            {
                fireSound.Play();
            }
            speed *= 0.35f;
            /*GameObject player = PlayerManager.GetPlayer();
            Transform playerTransform = player.transform;
            SetDirection(transform, playerTransform);*/
            StopCoroutine(DestroySelf(0f));
            StartCoroutine(DestroySelf(lifetime));
        }
    }
}
