using System.Collections.Generic;

[System.Serializable]
public class MutableFloat
{
    public float baseValue;
    private readonly Dictionary<string, float> additives = new();
    private readonly Dictionary<string, float> coefficients = new();

    public float GetCurrentValue()
    {
        float totalAdditives = 0f;
        float totalCoefficients = 1f;
        foreach (var additive in additives)
        {
            totalAdditives += additive.Value;
        }
        foreach (var coefficient in coefficients)
        {
            totalCoefficients += coefficient.Value;
        }
        return (baseValue + totalAdditives) * totalCoefficients;
    }

    public void AddMutableFloatAdditive(string key, float additive)
    {
        if (additives.ContainsKey(key))
            additives.Remove(key);
        additives.Add(key, additive);
    }

    public void AddMutableFloatCoefficient(string key, float coefficient)
    {
        if (coefficients.ContainsKey(key))
            coefficients.Remove(key);
        coefficients.Add(key, coefficient);
    }

    public void RemoveMutableFloatAdditive(string key) => additives.Remove(key);
    public void RemoveMutableFloatCoefficient(string key) => coefficients.Remove(key);
}
