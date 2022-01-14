using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Gun : MonoBehaviour
{
    [Header("Weapon Stats")]
    public int magazineCapacity;
    public int totalWeaponAmmo;
    public float range;
    public float rateOfFire;
    public float damage;
    private float ROF;
    [HideInInspector] public int currentAmmo;

    [Header("Game Objects")]
    public GameObject bullet;
    public GameObject muzzlePosition;
    public ParticleSystem muzzleFlash;
    public Light muzzleLight;
    public ParticleSystem casingEjectParticle;
    private Camera playerCam;

    [Header("Attachment System")]
    public GameObject supressor;
    public GameObject laserSight;
    public GameObject flashLight;
    [HideInInspector] public GameObject muzzleObject;
    [HideInInspector] public GameObject sideObject;

    [Header("Tool Tip")]
    public string _name;
    public string _description;
    public string _MagazineSize;
    public string _Damage;
    public string _FireRate;
    public bool isReloading;
    private ToolTipUI tooltipUI;

    [Header("IK")]
    public Transform leftHandIKTarget;
    public Transform rightHandIKTarget;
    public Transform leftIK;
    public Transform rightIK;
    public GameObject hands;


    [Header("Misc")]
    public WeaponSway weaponSway;
    public HeadBob headBob;
    public GameController gc;
    public bool switchingAttachments = false;
    public GameObject weaponModel;
    public Quaternion startRot;
    public Quaternion newRot;
    public Sprite weaponIcon;
    private PlayerInventory playerInv;

    [HideInInspector] public bool equipped;
    [HideInInspector] public bool inInventory;
    [HideInInspector] public bool isSupressed;
    [HideInInspector] public bool canShoot;

    public Transform viewPosition;
    private Animator animator;
    AudioManager audioManager;


    public WeaponType weaponType;
    public enum WeaponType
    {
        Pistol,
        AssaultRifle,
        smg,
    }



    private void Awake()
    {
        canShoot = true;
        playerInv = FindObjectOfType<PlayerInventory>();
        audioManager = FindObjectOfType<AudioManager>();
        leftHandIKTarget = GameObject.Find("LeftHandIKTarget").transform;
        rightHandIKTarget = GameObject.Find("RightHandIKTarget").transform;
        //startRot = weaponModel.transform.localRotation;
        equipped = false;
        hands = GameObject.Find("Hands").gameObject;
        FindViewPos();
        animator = GetComponent<Animator>();
        tooltipUI = GameObject.FindGameObjectWithTag("TTUI").GetComponent<ToolTipUI>();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        weaponSway = gameObject.GetComponent<WeaponSway>();
        headBob = gameObject.GetComponent<HeadBob>();

        playerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        currentAmmo = magazineCapacity;
        CheckHolsterState();
        StartCoroutine(DisableOutlineOnStart());
    }

    private void Start()
    {
        
        
        
    }
    
    private void FindViewPos()
    {
        switch (weaponType)
        {
            case WeaponType.Pistol:
                viewPosition = GameObject.Find("Pistol_Position").transform;
                break;
            case WeaponType.AssaultRifle:
                viewPosition = GameObject.Find("M4A1_Position").transform;
                break;
            case WeaponType.smg:
                viewPosition = GameObject.Find("SMG_Position").transform;
                break;



        }
    }
    public void PauseScripts()
    {
        weaponSway.enabled = false;
        headBob.enabled = false;
    }
    public void UnPauseScripts()
    {
        weaponSway.enabled = true;
        headBob.enabled = true;
    }

    public void EnterAttachmentSystem()
    {
        switchingAttachments = true;
        PauseScripts();
        transform.position = viewPosition.position;
        animator.enabled = true;
        switch (weaponType)
        {
            case WeaponType.Pistol:
               animator.SetBool("inPistolAttachments", true);
                break;
            case WeaponType.AssaultRifle:
                animator.SetBool("inAssaultRifleAttachments", true);
                break;
            case WeaponType.smg:
                animator.SetBool("inSMGAttachments", true);
                break;



        }
    }

    public void ExitAttachmentSystem()
    {
        switchingAttachments = false;
        UnPauseScripts();
        switch (weaponType)
        {
            case WeaponType.Pistol:
                animator.SetBool("inPistolAttachments", false);
                break;
            case WeaponType.AssaultRifle:
                animator.SetBool("inAssaultRifleAttachments", false);
                break;
            case WeaponType.smg:
                animator.SetBool("inSMGAttachments", false);
                break;



        }
    }

    public void CheckHolsterState()
    {
        
    }

    IEnumerator DisableOutlineOnStart()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponent<Outline>().enabled = false;
    }

    public void Reveal()
    {
        tooltipUI.itemName.text = _name;
        tooltipUI.itemDescription.text = _description;
        tooltipUI.itemMagazineSize.text = "Magazine Size: " + _MagazineSize;
        tooltipUI.itemDamage.text = "Damage: " + _Damage;
        tooltipUI.itemFireRate.text = "Fire Rate: " + _FireRate;
        tooltipUI.toolTip.SetActive(true);
        GetComponent<Outline>().enabled = true;
    }

    public void UnReveal()
    {
        tooltipUI.toolTip.SetActive(false);
        GetComponent<Outline>().enabled = false;
    }
    private void OnMouseOver()
    {
        if (tooltipUI != null)
        {
            
        }
        
    }

    private void OnMouseExit()
    {
       if (tooltipUI != null)
        {
            
        }
        
    }

    public void DisableToolTip()
    {
        tooltipUI.toolTip.SetActive(false);
        GetComponent<Outline>().enabled = false;
    }

    public void Reload()
    {

        StartCoroutine(ReloadTimer());
    }

    IEnumerator ReloadTimer()
    {
        canShoot = false;
        animator.SetTrigger("reload");
        yield return new WaitForSeconds(1f);
        int ammoNeededToRefill = magazineCapacity - currentAmmo;
        Debug.Log(ammoNeededToRefill);
        if (totalWeaponAmmo >= ammoNeededToRefill)
        {
            currentAmmo = magazineCapacity;

            switch (weaponType)
            {
                case WeaponType.Pistol:
                    playerInv.pistolAmmo -= ammoNeededToRefill;
                    break;
                case WeaponType.AssaultRifle:
                    playerInv.assaultRifleAmmo -= ammoNeededToRefill;
                    break;
                case WeaponType.smg:
                    playerInv.pistolAmmo -= ammoNeededToRefill;
                    break;
            }
            
            
        }
        else if (totalWeaponAmmo <= ammoNeededToRefill)
        {
            currentAmmo += totalWeaponAmmo;
            switch (weaponType)
            {
                case WeaponType.Pistol:
                    playerInv.pistolAmmo = 0;
                    break;
                case WeaponType.AssaultRifle:
                    playerInv.assaultRifleAmmo = 0;
                    break;
                case WeaponType.smg:
                    playerInv.pistolAmmo = 0;
                    break;
            }
        }
        canShoot = true;
    }



    private void Update()
    {
        if (gc.gamePaused)
        {
            PauseScripts();
        }
        else if (!gc.gamePaused)
        {
            if (inInventory)
            {
                Transform[] transforms = gameObject.GetComponentsInChildren<Transform>();
                foreach (Transform transform in transforms)
                {
                    transform.transform.gameObject.layer = LayerMask.NameToLayer("Weapon");

                }

                if (equipped)
                {
                    if (Input.GetKeyDown(KeyCode.R) && currentAmmo != magazineCapacity && totalWeaponAmmo > 0)
                    {
                        PauseScripts();
                        Reload();
                    }
                    if (!switchingAttachments)
                    {
                        weaponSway.enabled = true;
                        headBob.enabled = true; 
                    }
                    

                    leftHandIKTarget.position = leftIK.position;
                    leftHandIKTarget.rotation = leftIK.rotation;
                    rightHandIKTarget.position = rightIK.position;
                    rightHandIKTarget.rotation = rightIK.rotation;

                    SetHandAnimation();
                }
                else if (!equipped)
                {
                    weaponSway.enabled = false;
                    headBob.enabled = false;
                }
            }
            else if (!inInventory)
            {
                weaponSway.enabled = false;
                headBob.enabled = false;
                Transform[] transforms = gameObject.GetComponentsInChildren<Transform>();
                foreach (Transform transform in transforms)
                {
                    transform.transform.gameObject.layer = LayerMask.NameToLayer("FloorWeapon");
                }
            }
            if (Input.GetMouseButton(0) && !switchingAttachments && canShoot)
            {
                Shooting();
            }


            switch (weaponType)
            {
                case WeaponType.Pistol:
                    totalWeaponAmmo = playerInv.pistolAmmo;
                    break;
                case WeaponType.AssaultRifle:
                    totalWeaponAmmo = playerInv.assaultRifleAmmo;
                    break;
                case WeaponType.smg:
                    totalWeaponAmmo = playerInv.pistolAmmo;
                    break;
                default:
                    break;
            }
            
        }
        

        
    }

    public void SetHandAnimation()
    {
        hands.GetComponent<Animator>().SetBool("m4", false);
        hands.GetComponent<Animator>().SetBool("pistol", false);
        hands.GetComponent<Animator>().SetBool("smg", false);

        switch (weaponType)
        {
            case WeaponType.Pistol:
                hands.GetComponent<Animator>().SetBool("pistol", true);
                break;
            case WeaponType.AssaultRifle:
                hands.GetComponent<Animator>().SetBool("m4", true);
                break;
            case WeaponType.smg:
                hands.GetComponent<Animator>().SetBool("smg", true);
                break;




        }
    }

    IEnumerator LightFlicker()
    {
        muzzleLight.enabled = true;
        yield return new WaitForSeconds(0.025f);
        muzzleLight.enabled = false;
    }
    public void Shooting()
    {
        if (gameObject.activeSelf && equipped && Time.time > ROF && currentAmmo > 0 && !isReloading)
        {
            ROF = Time.time + rateOfFire;
            currentAmmo--;
            
            if (!isSupressed)
            {
                muzzleFlash.Play();
                StartCoroutine(LightFlicker());
            }
            
            Vector3 forward = playerCam.transform.forward * 50;
            Debug.DrawRay(playerCam.transform.position, forward, Color.green);
            RaycastHit hit;
            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, range))
            {
                if (hit.point != null)
                {
                    GameObject b = Instantiate(bullet, muzzlePosition.transform.position, muzzlePosition.transform.rotation);
                    casingEjectParticle.Play();

                     b.transform.LookAt(hit.point);
                     b.gameObject.GetComponent<Rigidbody>().AddForce(b.transform.forward * 8000 * Time.fixedDeltaTime);
                }
            }
            if (isSupressed)
            {
                audioManager.Play("Supressed");
            }
            else
            {
                switch (weaponType)
                {
                    case WeaponType.Pistol:
                        audioManager.Play("Glock");
                        break;
                    case WeaponType.AssaultRifle:
                        audioManager.Play("AR");
                        break;
                    case WeaponType.smg:
                        audioManager.Play("SMG");
                        break;
                }
            }
            
        }
    }

    public void DisableGravity(bool value)
    {
        if (value == true)
        {
            this.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    public void Kinematic(bool value)
    {
        if (value == true)
        {
            this.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void DisableCollider(bool value)
    {
        if (value == true)
        {
            BoxCollider[] colliders = this.GetComponents<BoxCollider>();
            foreach (BoxCollider bc in colliders)
            {
                bc.enabled = false;

                //this.GetComponent<Collider>().enabled = false;
            }
        }
    }
}
