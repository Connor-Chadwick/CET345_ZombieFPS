using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Animator animator;
    SceneController sceneManage;
    private bool doorClosed;
    private AudioManager audioManager;
    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        sceneManage = FindObjectOfType<SceneController>();
    }
    public void InteractWithButton()
    {
        audioManager.Play("ElevatorDoor");
        animator.SetTrigger("close");
        sceneManage.Scene2();
        
    }

    public void OpenDoor()
    {
        animator.SetTrigger("open");
    }
    
}
