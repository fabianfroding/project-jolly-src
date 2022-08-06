using UnityEngine;

public class PlayerBarrier : PlayerAbility
{
    [SerializeField] private GameObject barrierPrefab;
    private GameObject barrierInstance;

    //==================== PUBLIC ====================//
    public override bool StartAbility()
    {
        if (base.StartAbility())
        {
            barrierInstance = Instantiate(barrierPrefab, transform);
            barrierInstance.transform.parent = transform;
            /*bool barrierRefreshEquipped = UIManager.GetUIManagerScript().GetEquipmentMenu().IsEquipped("BarrierRefresh");
            if (barrierRefreshEquipped)
            {
                CancelInvoke(RESET_CD_METHOD_NAME);
                Invoke(RESET_CD_METHOD_NAME, cd * 0.5f);
            }*/
        }
        return true;
    }

    public bool IsActive()
    {
        if (barrierInstance != null && barrierInstance.GetComponent<Barrier>().IsActive())
            return true;
        else return false;
    }

    public void DestroyBarrier()
    {
        if (barrierInstance != null)
            barrierInstance.GetComponent<Barrier>().DestroySelf();
    }
}
