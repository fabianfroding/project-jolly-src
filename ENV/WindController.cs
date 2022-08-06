using UnityEngine;

public class WindController : MonoBehaviour
{
    public Material[] materials;
    public float windSpeed;

    void Update()
    {
        foreach (var material in materials)
        {
            material.SetFloat("_WindSpeed", windSpeed);
        }
    }
}
