using UnityEngine;
using Harmony;

public class InfiniteAmmo : MonoBehaviour
{
    private void Awake()
    {
        wm = base.GetComponentInChildren<WeaponManager>();
    }
    private void Update()
    {
        if (!wm.isMasterArmed)
            return;
        foreach (HPEquippable equip in (HPEquippable[])Traverse.Create(wm).Field("equips").GetValue())
        {
            if (equip != null)
            {
                if (equip is HPEquipGun)
                {
                    if (((HPEquipGun)equip).gun == null)
                    {
                        Debug.LogError("Gun is null on HPEQUIP " + equip.name);
                        continue;
                    }
                    ((HPEquipGun)equip).gun.currentAmmo = ((HPEquipGun)equip).gun.maxAmmo;
                }
                else if (equip is RocketLauncher)
                {
                    if (((RocketLauncher)equip).GetCount() != ((RocketLauncher)equip).GetMaxCount())
                        ((RocketLauncher)equip).LoadCount(999);
                }
                else
                {
                    ((HPEquipMissileLauncher)equip).ml.LoadAllMissiles();
                }
                //equip.Equip();
            }
        }
    }
    public WeaponManager wm;
    private Traverse wmTraverse;
}
