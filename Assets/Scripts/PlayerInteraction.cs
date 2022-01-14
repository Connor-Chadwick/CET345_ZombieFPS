using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Gun weaponPickedUp;
    private PlayerInventory inventory;
    private Camera playerCam;
    public LayerMask floorweapon;
    private GameController gc;
    public GameObject pickUpPos;
    private GameObject tempBattery;
    [HideInInspector] public bool hasBattery;
    public GameObject eleDoor;
    private SceneController scenec;
    AudioManager audioManager;
    private UIManager uiManager;
    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        Physics.IgnoreLayerCollision(10, 13);
        scenec = FindObjectOfType<SceneController>();
        hasBattery = false;
        inventory = GetComponent<PlayerInventory>();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        uiManager = FindObjectOfType<UIManager>();
    }

    private void Update()
    {
        Interaction();
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            
            eleDoor.SetActive(true);
            scenec.Scene2();
        }
    }

    
    void Interaction()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (hasBattery)
            {
                tempBattery.GetComponent<Rigidbody>().useGravity = true;
                tempBattery.GetComponent<Rigidbody>().isKinematic = false;
                tempBattery.GetComponent<Collider>().enabled = true;
                tempBattery.transform.parent = null;
                hasBattery = false;
                tempBattery = null;
                
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiManager.gamePaused = !uiManager.gamePaused;
            uiManager.PauseCheck();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {

            
            Debug.DrawRay(playerCam.transform.position, playerCam.transform.forward * 50f, Color.green);
            RaycastHit hit;
            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 50f, floorweapon))
            {
                Gun gun = hit.transform.GetComponent<Gun>();
                if (gun != null)
                {
                    if (inventory.weaponCount < 2)
                    {
                        inventory.AddingWeapon(gun.gameObject);

                        weaponPickedUp = gun.gameObject.GetComponent<Gun>();
                        weaponPickedUp.DisableCollider(true);
                        weaponPickedUp.DisableGravity(true);
                        weaponPickedUp.Kinematic(true);
                        weaponPickedUp.DisableToolTip();
                    }
                    
                }
                
            }
            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 50f))
            {
                PistolAmmoPickup pistolAmmo = hit.transform.GetComponent<PistolAmmoPickup>();
                ARAmmoPickup arAmmo = hit.transform.GetComponent<ARAmmoPickup>();
                AmmoPickup ammo = hit.transform.GetComponent<AmmoPickup>();
                Battery battery = hit.transform.GetComponent<Battery>();
                WeaponCrate weaponCreate = hit.transform.GetComponent<WeaponCrate>();
                Elevator elevator = hit.transform.GetComponent<Elevator>();
              
                if (ammo != null)
                {
                    audioManager.Play("AmmoPickup");
                    inventory.AddAmmo(ammo.gameObject);
                    
                }

                if (battery != null && !hasBattery)
                {
                    hasBattery = true;
                    battery.transform.parent = pickUpPos.transform;
                    tempBattery = battery.gameObject;
                    battery.GetComponent<Rigidbody>().useGravity = false;
                    battery.GetComponent<Rigidbody>().isKinematic = true;
                    tempBattery.GetComponent<Collider>().enabled = false;
                    StartCoroutine(PickUpBattery(battery.gameObject));
                }
                if (weaponCreate != null)
                {
                    weaponCreate.OpenCrate();
                }

                if (hit.transform.gameObject.CompareTag("Scrap"))
                {
                    audioManager.Play("AmmoPickup");
                    inventory.scrapAmount += 50;
                    hit.transform.gameObject.GetComponent<ToolTipItem>().DisableToolTip();
                    Destroy(hit.transform.gameObject);
                }

                if (hit.transform.gameObject.CompareTag("MedKit"))
                {
                    inventory.AddAmmo(hit.transform.gameObject);
                    inventory.medKits += 1;
                }
                if (elevator != null)
                {
                    elevator.InteractWithButton();
                }
            }

            
        }
    }

    IEnumerator PickUpBattery(GameObject pickup)
    {
        float duration = 0.2f;
        float time = 0;
        Vector3 startPosition = pickup.transform.position;

        while (time < duration)
        {
            pickup.transform.position = Vector3.Lerp(startPosition, pickUpPos.transform.position, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator LerpPickup(GameObject pickup)
    {
        float duration = 0.2f;
        float time = 0;
        Vector3 startPosition = pickup.transform.position;

        while (time < duration)
        {
            pickup.transform.position = Vector3.Lerp(startPosition, pickUpPos.transform.position, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        pickup.transform.position = pickup.GetComponent<Gun>().viewPosition.transform.position;


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            if (other.GetComponentInParent<Door>().isPowered)
            {
                Debug.Log("1");
                other.GetComponentInParent<Animator>().SetBool("open", true);
            }

            if (other.CompareTag("Cell"))
            {
                Debug.Log("1");
            }
          
                
       
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            if (other.GetComponentInParent<Door>().isPowered)
            {
                other.GetComponentInParent<Animator>().SetBool("open", false);
            }
        

        }
    }


}
