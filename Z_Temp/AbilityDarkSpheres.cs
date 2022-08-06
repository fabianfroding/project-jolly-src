using System.Collections;
using UnityEngine;

public class AbilityDarkSpheres : Ability
{
    [SerializeField] private string animName = "MasterHarvester_Cast";
    //[SerializeField] private int minNumSpheres = 2;
    [SerializeField] private float initialRadius = 7f;

    [SerializeField] private GameObject darkSpherePrefab;
    [SerializeField] private AudioSource birthSound;

    private int numSpheres;
    private GameObject source;
    private GameObject[] spheres;
    private bool isActive = false;

    //==================== PUBLIC ====================//
    public bool Initialize(GameObject source)
    {
        this.source = source;
        return StartAbility();
    }

    public override bool StartAbility()
    {
        if (base.StartAbility())
        {
            isActive = true;
            GetComponent<Animator>().Play(animName);
            if (CameraManager.IsGameObjectInCameraView(gameObject))
            {
                birthSound.Play();
            }
            //float percentageHealth = (float)source.GetHealth() / (float)source.GetMaxHealth() * 100f;
            numSpheres = 5; // (int)(minNumSpheres + (100 - percentageHealth) / 20);
            spheres = new GameObject[numSpheres];
            for (int i = 0; i < numSpheres; i++)
            {
                var radians = 2 * Mathf.PI / numSpheres * i;
                var vertical = Mathf.Sin(radians);
                var horizontal = Mathf.Cos(radians);
                var spawnDir = new Vector3(horizontal, vertical).normalized;
                Vector2 spawnPos = source.transform.position + spawnDir * initialRadius;

                spheres[i] = Instantiate(darkSpherePrefab, spawnPos, Quaternion.identity);
                spheres[i].GetComponent<DarkSphere>().Initialize(source.gameObject);
            }
            StartCoroutine(SetActive(spheres[0].GetComponent<Projectile>().GetLifetime(), false));
            return true;
        }
        return false;
    }

    public IEnumerator SetActive(float delay, bool val)
    {
        yield return new WaitForSeconds(delay);
        if (val == false)
        {
            GetComponent<Animator>().Play("MasterHarvester_Idle");
        }
        isActive = val;
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void FireSpheres()
    {
        for (int i = 0; i < spheres.Length; i++)
        {
            if (spheres[i] != null)
                spheres[i].GetComponent<DarkSphere>().StopRotation();
        }
    }
}
