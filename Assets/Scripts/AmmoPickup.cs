using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    private PlayerInventory inv;
    Outline outine;
    public enum AmmoType
    {
       pistol,
       assaultRifle,
    }

    public AmmoType ammoType;
    private void Start()
    {
        outine = GetComponent<Outline>();
        inv = FindObjectOfType<PlayerInventory>();
    }
    private void OnDestroy()
    {
        switch (ammoType)
        {
            case AmmoType.pistol:
                inv.pistolAmmo += 17;
                break;
            case AmmoType.assaultRifle:
                inv.assaultRifleAmmo += 30;
                break;
        }
    }

}
