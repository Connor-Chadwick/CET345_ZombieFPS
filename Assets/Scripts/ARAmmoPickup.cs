using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARAmmoPickup : MonoBehaviour
{
    PlayerInventory inv;
    private void Awake()
    {
        inv = FindObjectOfType<PlayerInventory>();
    }
    public int ammo = 30;
   
}
