using System;

[Serializable]
public class FloatReference
{
    public bool useConstant = true;
    public float constantValue;
    public SOFloatVariable variable;

    public float Value
    {
        get => useConstant ? constantValue : variable.Value;
    }
}
