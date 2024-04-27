using System;

[Serializable]
public class IntReference
{
    public bool useConstant = true;
    public int constantValue;
    public SOIntVariable variable;

    public int Value
    {
        get => useConstant ? constantValue : variable.Value;
        set
        {
            if (useConstant)
            {
                constantValue = value;
            }
            else
            {
                variable.Value = value;
            }
        }
    }
}
