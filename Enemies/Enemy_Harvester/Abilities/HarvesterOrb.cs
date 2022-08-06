using UnityEngine;

public class HarvesterOrb : Projectile
{
    protected override void Start()
    {
        StopCoroutine(DestroySelf(0f));
        StartCoroutine(DestroySelf(lifetime));
    }
}
