using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AttachmentSystem : MonoBehaviour
{
    //public GameObject muzzlePosition;
    public GameObject muzzle_Object;
    public GameObject muzzle_Supressor;

    public GameObject side_Object;
    public GameObject side_Laser;
    public GameObject side_FlashLight;

    public PlayerInventory playerInv;
    public AttachmentUIManager attachmentUIManager;

    public int supressorCost;
    public int flashLightCost;
    public int laserSightCost;
    Gun gun;
    AudioManager audioManager;
    

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        attachmentUIManager = FindObjectOfType<AttachmentUIManager>();
        playerInv = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
    }

    public void Remove_Muzzle()
    {
        playerInv.weaponHolster[playerInv.weaponToEquip].GetComponent<Gun>().muzzleObject.SetActive(false);
        gun.isSupressed = false;
    }

    public void Attach_Muzzle_Supressor()
    {
        gun = playerInv.weaponHolster[playerInv.weaponToEquip].GetComponent<Gun>();
        gun.muzzleObject = gun.supressor;
        gun.muzzleObject.SetActive(true);
        gun.isSupressed = true;
    }

    public void Remove_Side()
    {
        gun = playerInv.weaponHolster[playerInv.weaponToEquip].GetComponent<Gun>();
        gun.sideObject.SetActive(false);
        gun.sideObject = null;
    }

    public void Attach_Side_Laser()
    {
        gun = playerInv.weaponHolster[playerInv.weaponToEquip].GetComponent<Gun>();
        if (gun.sideObject != null)
        {
            gun.sideObject.SetActive(false);
            gun.sideObject = null;
        }
        if (gun.sideObject == null)
        {
            gun.sideObject = gun.laserSight;
            gun.sideObject.SetActive(true);
        }
    }

    public void Attach_Side_FlashLight()
    {
        gun = playerInv.weaponHolster[playerInv.weaponToEquip].GetComponent<Gun>();
        if (gun.sideObject != null)
        {
            gun.sideObject.SetActive(false);
            gun.sideObject = null;
        }
        if (gun.sideObject == null)
        {
            gun.sideObject = gun.flashLight;
            gun.sideObject.SetActive(true);
        }
    }

    public void Pistol_Craft_Supressor()
    {
        if (playerInv.scrapAmount >= supressorCost)
        {
            attachmentUIManager.craft_pistol_Barrel_Supressor.gameObject.SetActive(false);
            attachmentUIManager.attach_pistol_Barrel_Supressor.interactable = true;
            playerInv.scrapAmount -= supressorCost;
            audioManager.Play("Craft");
        }
    }

    public void Pistol_Craft_FlashLight()
    {
        if (playerInv.scrapAmount >= flashLightCost)
        {
            attachmentUIManager.craft_pistol_Side_FlashLight.gameObject.SetActive(false);
            attachmentUIManager.attach_pistol_Side_FlashLight.interactable = true;
            playerInv.scrapAmount -= flashLightCost;
            audioManager.Play("Craft");
        }
    }

    public void Pistol_Craft_LaserSight()
    {
        if (playerInv.scrapAmount >= laserSightCost)
        {
            attachmentUIManager.craft_pistol_Side_Laser.gameObject.SetActive(false);
            attachmentUIManager.attach_pistol_Side_Laser.interactable = true;
            playerInv.scrapAmount -= laserSightCost;
            audioManager.Play("Craft");
        }
    }

    public void AR_Craft_Supressor()
    {
        if (playerInv.scrapAmount >= supressorCost)
        {
            attachmentUIManager.craft_ar_Barrel_Supressor.gameObject.SetActive(false);
            attachmentUIManager.attach_ar_Barrel_Supressor.interactable = true;
            playerInv.scrapAmount -= supressorCost;
            audioManager.Play("Craft");
        }
    }

    public void AR_Craft_FlashLight()
    {
        if (playerInv.scrapAmount >= flashLightCost)
        {
            attachmentUIManager.craft_ar_Side_FlashLight.gameObject.SetActive(false);
            attachmentUIManager.attach_ar_Side_FlashLight.interactable = true;
            playerInv.scrapAmount -= flashLightCost;
            audioManager.Play("Craft");
        }
    }

    public void AR_Craft_LaserSight()
    {
        if (playerInv.scrapAmount >= laserSightCost)
        {
            attachmentUIManager.craft_ar_Side_Laser.gameObject.SetActive(false);
            attachmentUIManager.attach_ar_Side_Laser.interactable = true;
            playerInv.scrapAmount -= laserSightCost;
            audioManager.Play("Craft");
        }
    }

    public void SMG_Craft_Supressor()
    {
        if (playerInv.scrapAmount >= supressorCost)
        {
            attachmentUIManager.craft_smg_Barrel_Supressor.gameObject.SetActive(false);
            attachmentUIManager.attach_smg_Barrel_Supressor.interactable = true;
            playerInv.scrapAmount -= supressorCost;
            audioManager.Play("Craft");
        }
    }

    public void SMG_Craft_FlashLight()
    {
        if (playerInv.scrapAmount >= flashLightCost)
        {
            attachmentUIManager.craft_smg_Side_FlashLight.gameObject.SetActive(false);
            attachmentUIManager.attach_smg_Side_FlashLight.interactable = true;
            playerInv.scrapAmount -= flashLightCost;
            audioManager.Play("Craft");
        }
    }

    public void SMG_Craft_LaserSight()
    {
        if (playerInv.scrapAmount >= laserSightCost)
        {
            attachmentUIManager.craft_smg_Side_Laser.gameObject.SetActive(false);
            attachmentUIManager.attach_smg_Side_Laser.interactable = true;
            playerInv.scrapAmount -= laserSightCost;
            audioManager.Play("Craft");
        }
    }


    //old code only worked with one gun keeping it for reference
    //public void Remove_Muzzle()
    //{
    //    muzzle_Object.SetActive(false);
    //}
    //public void Attach_Supressor()
    //{ 
        
    //    muzzle_Object = muzzle_Supressor;
    //    muzzle_Object.SetActive(true);
    //}

    //public void RemoveSide() 
    //{
    //    side_Object.SetActive(false);
    //    side_Object = null;
    //}

    //public void Attach_Laser()
    //{
    //    if (side_Object != null)
    //    {
    //        side_Object.SetActive(false);
    //        side_Object = null;
    //    }
    //    if (side_Object == null)
    //    {
    //        side_Object = side_Laser;
    //        side_Object.SetActive(true);
    //    }

    //}
    //public void Attach_FlashLight()
    //{
    //    if (side_Object != null)
    //    {
    //        side_Object.SetActive(false);
    //        side_Object = null;
    //    }
    //    if (side_Object == null)
    //    {
    //        side_Object = side_FlashLight;
    //        side_Object.SetActive(true);
    //    }
        
        
    //}

}
