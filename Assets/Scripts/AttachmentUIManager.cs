using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttachmentUIManager : MonoBehaviour
{
    [Header("Weapon Attachment UIs")]
    public GameObject pistolAttachmentUI;
    public GameObject ARAttachmentUI;
    public GameObject SMGAttachmentUI;

    [Header("Pistol Attachment Buttons")]
    public Button attach_pistol_Barrel_Supressor;
    public Button attach_pistol_Side_Laser;
    public Button attach_pistol_Side_FlashLight;
    [Header("Pistol Attachment Craft Buttons")]
    public Button craft_pistol_Barrel_Supressor;
    public Button craft_pistol_Side_Laser;
    public Button craft_pistol_Side_FlashLight;

    [Header("Assault Rifle Attachment Buttons")]
    public Button attach_ar_Barrel_Supressor;
    public Button attach_ar_Side_Laser;
    public Button attach_ar_Side_FlashLight;
    [Header("Assault Rifle Attachment Craft Buttons")]
    public Button craft_ar_Barrel_Supressor;
    public Button craft_ar_Side_Laser;
    public Button craft_ar_Side_FlashLight;

    [Header("SMG Attachment Buttons")]
    public Button attach_smg_Barrel_Supressor;
    public Button attach_smg_Side_Laser;
    public Button attach_smg_Side_FlashLight;
    [Header("SMG Attachment Craft Buttons")]
    public Button craft_smg_Barrel_Supressor;
    public Button craft_smg_Side_Laser;
    public Button craft_smg_Side_FlashLight;


}
