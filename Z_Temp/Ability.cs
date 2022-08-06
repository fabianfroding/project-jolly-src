using UnityEngine;

public class Ability : MonoBehaviour
{
    protected static readonly string RESET_CD_METHOD_NAME = "ResetCD";

    [SerializeField] protected float cd = 0.35f;
    protected bool onCD = false;

    //==================== PUBLIC ====================//
    public virtual bool StartAbility()
    {
        if (cd <= 0) return true;
        if (!onCD)
        {
            onCD = true;
            Invoke(RESET_CD_METHOD_NAME, cd);
            return true;
        }
        return false;
    }

    public bool OnCD()
    {
        return onCD;
    }

    public virtual void ResetCD()
    {
        onCD = false;
    }
}
