using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    public bool gamePaused;
    public TMP_Text fovText;
    public TMP_Text sensText;
    public Slider fovSlider;
    public Camera cam;
    public GameObject eleDoor;
    private AudioManager audioManager;
    public GameObject levelBuilder;

    public GameObject UI;
    //public GameObject pauseMenu;
    public GameObject pauseScreen;
    public GameObject optionsScreen;
    public GameObject deathScreen;
    private SceneController sceneControl;


    private void Start()
    {
        sceneControl = FindObjectOfType<SceneController>();
        audioManager = FindObjectOfType<AudioManager>();
        
    }

    public void SpawnNewGenerator()
    {
        Debug.Log("New Generator");
        Destroy(FindObjectOfType<ProceduralLevelGenerator>().gameObject);
        Instantiate(levelBuilder, transform.position, transform.rotation);
        //return;
    }

    

    public void OpenDoor()
    {
        StartCoroutine(OpenElevatorDoor());
    }
    IEnumerator OpenElevatorDoor()
    {
        audioManager.Play("ElevatorBeep");
        yield return new WaitForSeconds(1f);
        audioManager.Play("ElevatorDoor");
        eleDoor = GameObject.FindGameObjectWithTag("ElevatorDoor");
        eleDoor.GetComponent<Animator>().SetTrigger("open");
    }

   

    

}
