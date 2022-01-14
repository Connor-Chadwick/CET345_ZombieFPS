using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerInventory : MonoBehaviour
{
    [Header("Weapon Holster")]
    public GameObject[] weaponHolster = new GameObject[2];


    [HideInInspector] public int previousEquipped;
    [HideInInspector] public int weaponToEquip;
    [HideInInspector] public int weaponCount;
    private int keyPressed;

    [Header("Player Resources")]
    public int assaultRifleAmmo;
    public int pistolAmmo;
    public int scrapAmount;
    public int medKits;

    [Header("Game Objects")]
    public GameObject weaponHolder;
    public GameObject emptySlot;
    public GameObject droppedWeapon;
    public GameObject playerOrien;
    public GameObject hands;
    private bool inAttachmentSystem = false;
    public GameObject pistolAttachmentUI;
    public GameObject assaultRifleAttachmentUI;
    public GameObject smgAttachmentUI;
    public UIManager uiManager;
    private AttachmentUIManager attachmentUI;

    [Header("Health")]
    public float currentHealth;
    public float maxHealth;
    private bool isHealing = false;
    public bool isDead = false;

    [Header("Misc")]
    public int targetFPS;
    public Transform _selection;
    public LayerMask weponLayer;
    AudioManager audioManager;
    private GameController gc;
    private bool canHeal;
    public Coroutine lastCo;
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFPS;
        isDead = false;
        canHeal = true;
    }
    private void Start()
    {
        gc = FindObjectOfType<GameController>();
        audioManager = FindObjectOfType<AudioManager>();
        weaponHolder = GameObject.Find("WeaponHolder");
        hands = GameObject.Find("Hands").gameObject;
        uiManager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
        attachmentUI = GameObject.FindGameObjectWithTag("UI").GetComponent<AttachmentUIManager>();
        pistolAttachmentUI = attachmentUI.pistolAttachmentUI;
        assaultRifleAttachmentUI = attachmentUI.ARAttachmentUI;
        smgAttachmentUI = attachmentUI.SMGAttachmentUI;
        currentHealth = 100;
        maxHealth = 100;
        weaponCount = 0;
        playerOrien = GameObject.FindGameObjectWithTag("Orientation");
        for (int i = 0; i < weaponHolster.Length; i++)
        {
            weaponHolster[i] = emptySlot;
        }
        
    }

    private void Update()
    {
        
        if (!isDead)
        {
            HealthManager();
            Selection();
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
            SwitchAttachments();
            if (weaponCount == 0)
            {
                hands.SetActive(false);
            }
            else if (weaponCount >= 1 && weaponHolster[previousEquipped] != emptySlot)
            {
                hands.SetActive(true);
            }
            if (!inAttachmentSystem)
            {
                DropWeapon();
                SwitchWeapon();
                if (Input.GetKeyDown(KeyCode.Alpha3) && medKits > 0)
                {
                    isHealing = true;
                    lastCo = StartCoroutine(Heal());
                }
            }

            if (Application.targetFrameRate != targetFPS)
                Application.targetFrameRate = targetFPS;
            if (Input.GetKeyDown(KeyCode.I))
            {
                targetFPS = 30;
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                targetFPS = 60;
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                targetFPS = 240;
            }
        }
       

    }

    public void Reload()
    {
        
    }

    public void Selection()
    {
        if (_selection != null)
        {
            
            
            if (_selection.transform.gameObject.GetComponent<Gun>())
            {
                _selection.transform.gameObject.GetComponent<Gun>().UnReveal();
                _selection = null;
            }
            else if (_selection.transform.gameObject.GetComponent<ToolTipItem>())
            {
                _selection.transform.gameObject.GetComponent<ToolTipItem>().UnReveal();
                _selection = null;
            }
        }

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, weponLayer))
        {
            Transform selection = hit.transform;
            if (selection.GetComponent<Gun>())
            {
                selection.gameObject.GetComponent<Gun>().Reveal();
                _selection = selection;
            }
            if (selection.GetComponent<ToolTipItem>())
            {
                selection.gameObject.GetComponent<ToolTipItem>().Reveal();
                _selection = selection;
            }

            
        }

        
    }

    public bool AddingWeapon(GameObject weapon)
    {
        for (int i = 0; i < weaponHolster.Length; i++)
        {
            // If no weapon in weaponHolster
            if (weaponHolster[i] == emptySlot && weaponCount == 0)
            {
                if (!hands.activeSelf)
                {
                    hands.SetActive(true);
                }
                Debug.Log("First IF");
                weaponHolster[i] = weapon;

                weapon.transform.parent = weaponHolder.transform;
                StartCoroutine(LerpGunPos(weapon));
                weapon.transform.rotation = weaponHolder.transform.rotation;
                weapon.GetComponent<Gun>().inInventory = true;
                weapon.GetComponent<Gun>().equipped = true;



                //Fixes weapon drop / throw position for some reason?
                weapon.SetActive(false);
                weapon.SetActive(true);

                uiManager.primaryWeaponImage.sprite = weapon.GetComponent<Gun>().weaponIcon;
                keyPressed = 1;
                weaponCount++;
                audioManager.Play("WeaponPickup");
                return true;
            }

            if (weaponHolster[i] == emptySlot && weaponHolster[weaponToEquip] == emptySlot && weaponCount > 0)
            {
                if (!hands.activeSelf)
                {
                    hands.SetActive(true);
                }
                Debug.Log("Second IF");
                weaponHolster[i] = weapon;

                weapon.transform.parent = weaponHolder.transform;
                StartCoroutine(LerpGunPos(weapon));
                weapon.transform.rotation = weaponHolder.transform.rotation;
                weapon.GetComponent<Gun>().inInventory = true;
                weapon.GetComponent<Gun>().equipped = true;
                weapon.transform.gameObject.SetActive(true);

                weaponCount++;
                audioManager.Play("WeaponPickup");
                return true;
            }

            //if player has weapon equipped and there is a spare slot
            if (weaponHolster[i] == emptySlot && weaponCount > 0)
            {
                if (!hands.activeSelf)
                {
                    hands.SetActive(true);
                }
                Debug.Log("Second IF");
                weaponHolster[i] = weapon;

                weapon.transform.parent = weaponHolder.transform;
                StartCoroutine(LerpGunPos(weapon));
                weapon.transform.rotation = weaponHolder.transform.rotation;
                weapon.GetComponent<Gun>().inInventory = true;
                weapon.transform.gameObject.SetActive(false);
                uiManager.secondaryWeaponImage.sprite = weapon.GetComponent<Gun>().weaponIcon;
                weaponCount++;
                audioManager.Play("WeaponPickup");
                return true;
            }

            
        }

        return false;
    }

    public void AddAmmo(GameObject pickup)
    {
        StartCoroutine(LerpPickup(pickup));
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        audioManager.Play("Hit");
        
        StartCoroutine(ShowBlood());
        StopCoroutine(lastCo);
    }

    IEnumerator LerpGunPos(GameObject weapon)
    {
        float duration = 0.2f;
        float time = 0;
        Vector3 startPosition = weapon.transform.position;

        while (time < duration)
        {
            weapon.transform.position = Vector3.Lerp(startPosition, weapon.GetComponent<Gun>().viewPosition.transform.position, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        weapon.transform.position = weapon.GetComponent<Gun>().viewPosition.transform.position;


    }

    IEnumerator LerpPickup(GameObject pickup)
    {
        float duration = 0.2f;
        float time = 0;
        Vector3 startPosition = pickup.transform.position;

        while (time < duration)
        {
            pickup.transform.position = Vector3.Lerp(startPosition, this.transform.position, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        pickup.transform.position = this.transform.position;
        Destroy(pickup);
    }

  
    public void SwitchWeapon()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha1) && weaponHolster[0] != emptySlot && keyPressed != 1)
        {
            if (!hands.activeSelf)
            {
                hands.SetActive(true);
            }
            if (weaponHolster[previousEquipped] != emptySlot)
            {
                weaponHolster[previousEquipped].GetComponent<Gun>().equipped = false;
                weaponHolster[previousEquipped].SetActive(false);
            }
            
            weaponToEquip = 0;
            weaponHolster[weaponToEquip].SetActive(true);
            weaponHolster[weaponToEquip].GetComponent<Gun>().equipped = true;

            if (weaponCount == 1)
            {
                uiManager.primaryWeaponImage.sprite = weaponHolster[weaponToEquip].GetComponent<Gun>().weaponIcon;
                uiManager.secondaryWeaponImage.sprite = uiManager.blankImage;
            }
            else
            {
                uiManager.primaryWeaponImage.sprite = weaponHolster[weaponToEquip].GetComponent<Gun>().weaponIcon;
                uiManager.secondaryWeaponImage.sprite = weaponHolster[previousEquipped].GetComponent<Gun>().weaponIcon;
            }
            previousEquipped = weaponToEquip;
            Debug.Log("In Slot 1");
            keyPressed = 1;
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && weaponHolster[1] != emptySlot && keyPressed != 2)
        {
           
            if (!hands.activeSelf)
            {
                hands.SetActive(true);
            }
            if (weaponHolster[previousEquipped] != emptySlot)
            {
                weaponHolster[previousEquipped].GetComponent<Gun>().equipped = false;
                weaponHolster[previousEquipped].SetActive(false);
            }
            weaponToEquip = 1;
            weaponHolster[weaponToEquip].SetActive(true);
            weaponHolster[weaponToEquip].GetComponent<Gun>().equipped = true;
            

            if (weaponCount == 1)
            {
                uiManager.primaryWeaponImage.sprite = weaponHolster[weaponToEquip].GetComponent<Gun>().weaponIcon;
                uiManager.secondaryWeaponImage.sprite = uiManager.blankImage;
            }
            else
            {
                uiManager.primaryWeaponImage.sprite = weaponHolster[weaponToEquip].GetComponent<Gun>().weaponIcon;
                uiManager.secondaryWeaponImage.sprite = weaponHolster[previousEquipped].GetComponent<Gun>().weaponIcon;
            }
            previousEquipped = weaponToEquip;
            Debug.Log("In Slot 2");
            keyPressed = 2;


        }

        
    }

    IEnumerator ShowBlood()
    {
        uiManager.bloodScreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        uiManager.bloodScreen.gameObject.SetActive(false);
    }
    public void DropWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            hands.SetActive(false);
            droppedWeapon = weaponHolster[weaponToEquip];

            weaponHolster[weaponToEquip].gameObject.transform.parent = null;
            weaponHolster[weaponToEquip].gameObject.GetComponent<Rigidbody>().isKinematic = false;
            weaponHolster[weaponToEquip].gameObject.GetComponent<Rigidbody>().useGravity = true;
            weaponHolster[weaponToEquip].gameObject.GetComponent<Gun>().inInventory = false;
            weaponHolster[weaponToEquip].gameObject.GetComponent<Gun>().equipped = false;
            weaponHolster[weaponToEquip].gameObject.GetComponent<Rigidbody>().AddRelativeForce(0, 0, 300f * Time.fixedDeltaTime, ForceMode.Impulse);

            Vector3 forceposition = playerOrien.transform.forward;

            droppedWeapon.gameObject.GetComponent<Collider>().enabled = true;

            weaponHolster[weaponToEquip].gameObject.GetComponent<Collider>().isTrigger = false;
            weaponHolster[weaponToEquip] = emptySlot;

            BoxCollider[] colliders = weaponHolster[weaponToEquip].GetComponents<BoxCollider>();
            foreach (BoxCollider bc in colliders)
            {
                bc.enabled = true;
            }
            uiManager.primaryWeaponImage.sprite = uiManager.blankImage;
            weaponCount--;
        }

    }

    public void SwitchAttachments()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inAttachmentSystem = !inAttachmentSystem;

            Gun gun = weaponHolster[weaponToEquip].gameObject.GetComponent<Gun>();

            if (inAttachmentSystem)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                uiManager.scrapText.gameObject.SetActive(true);
                DisplayAttachmentUI(gun);
                gun.EnterAttachmentSystem();
                GetComponent<PlayerMovement>().canCamera = false;
            }
            else if (!inAttachmentSystem)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                uiManager.scrapText.gameObject.SetActive(false);
                DissableAttachmentUI(gun);
                gun.ExitAttachmentSystem();
                GetComponent<PlayerMovement>().canCamera = true;
            }
        }

        
        


    }

    public void DisplayAttachmentUI(Gun gun)
    {
        if (gun.weaponType == Gun.WeaponType.Pistol)
        {
            pistolAttachmentUI.SetActive(true);
        }
        else if (gun.weaponType == Gun.WeaponType.AssaultRifle)
        {
            assaultRifleAttachmentUI.SetActive(true);
        }
        else if (gun.weaponType == Gun.WeaponType.smg)
        {
            smgAttachmentUI.SetActive(true);
        }

    }

    public void DissableAttachmentUI(Gun gun)
    {
        if (gun.weaponType == Gun.WeaponType.Pistol)
        {
            pistolAttachmentUI.SetActive(false);
        }
        else if (gun.weaponType == Gun.WeaponType.AssaultRifle)
        {
            assaultRifleAttachmentUI.SetActive(false);
        }
        else if (gun.weaponType == Gun.WeaponType.smg)
        {
            smgAttachmentUI.SetActive(false);
        }
    }

    public void HealthManager()
    {
        if (currentHealth <= 0 && !isDead)
        {
            uiManager.deathScreen.SetActive(true);
            
            
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GetComponent<PlayerMovement>().canCamera = false;
            ZombieAI[] zomb = FindObjectsOfType<ZombieAI>();
            foreach (ZombieAI a in zomb)
            {
                Destroy(a.gameObject);
            }

            SwatGuy[] swat = FindObjectsOfType<SwatGuy>();
            foreach (SwatGuy b in swat)
            {
                Destroy(b.gameObject);
            }
            isDead = true;

        }
    }

  

    
    public IEnumerator Heal()
    {
        medKits--;

        while (isHealing)
        {
            if (currentHealth == maxHealth)
            {
                isHealing = false;
            }
            if (currentHealth < maxHealth && canHeal)
            {
                currentHealth++;
                uiManager.healthText.text = "HEALTH: " + currentHealth.ToString();
            }
            yield return new WaitForSeconds(0.1f);

        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {
            TakeDamage(10f);
        }

        if (other.CompareTag("Cell"))
        {
            other.gameObject.GetComponent<CellControl>().OpenCell();
        }
        if (other.CompareTag("EndGame"))
        {
            SceneController sc = FindObjectOfType<SceneController>();
            
            foreach (GameObject g in sc.obsToNotDestroy)
            {
                Destroy(g.gameObject);
            }
            Debug.Log("endgame");
            uiManager.MainMenu();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cell"))
        {
            other.gameObject.GetComponent<CellControl>().CloseCell();
        }
    }
}
