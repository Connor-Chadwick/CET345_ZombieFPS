using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.VFX;
public class Knife : MonoBehaviour
{
    public GameObject ani;
    public Camera cam;
    public PlayerMovement playerMove;
    public GameObject attachmentUI;
    public GameObject glock;
    public VisualEffect muzzleflash;
    public ParticleSystem blood;
    private ParticleSystem blo;
    // Start is called before the first frame update

    void Start()
    {
        playerMove = GetComponent<PlayerMovement>();
    }

    IEnumerator despawnBlood()
    {
        
      
        yield return new WaitForSeconds(2f);
        Destroy(blo.gameObject);
    }
    void Update()
    {
        /*
        if (playerMove.isRunning)
        {
            glock.GetComponent<Animator>().SetBool("isRunning", true);
        }
        else
        {
            glock.GetComponent<Animator>().SetBool("isRunning", false);
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            playerMove.canCamera = !playerMove.canCamera;
        }
        if (!playerMove.canCamera)
        {
            attachmentUI.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            //glock.GetComponent<Animator>().enabled = true;
            glock.GetComponent<Animator>().SetBool("inAttachments", true);
            
        }
        else
        {
            attachmentUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            glock.GetComponent<Animator>().SetBool("inAttachments", false);
            //glock.GetComponent<Animator>().enabled = false;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            //ani.GetComponent<Animator>().SetBool("attack", true);
            //muzzleflash.Play();
            Vector3 forward = cam.transform.forward * 6;
            Debug.DrawRay(cam.transform.position, forward, Color.green);
            RaycastHit hit;
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 6))
                {
                if (hit.collider != null)
                {
                    //blo = Instantiate(blood, hit.point, hit.transform.rotation);
                }
                    if (hit.transform.CompareTag("zsombie"))
                    {
                        //hit.transform.gameObject.GetComponent<ZombieAI>().isDead = true;
                       // blo = Instantiate(blood, hit.point, hit.transform.rotation);
                       // blo.transform.parent = hit.transform;
                       // blo.transform.localScale = new Vector3(1, 1, 1);
                    }
                    else if (hit.transform.CompareTag("SWAT"))
                    {
                        //hit.transform.gameObject.GetComponent<SwatGuy>().isDead = true;
                        blo = Instantiate(blood, hit.point, hit.transform.rotation);
                        blo.transform.parent = hit.transform;
                        blo.transform.LookAt(this.transform);
                        blo.transform.localScale = new Vector3(1f, 1f, 1f);
                        //StartCoroutine(despawnBlood());
                        

                    }

                }
        }
        if (Input.GetMouseButtonUp(0))
        {
            //ani.GetComponent<Animator>().SetBool("attack", false);
        }
        */



    }
}
