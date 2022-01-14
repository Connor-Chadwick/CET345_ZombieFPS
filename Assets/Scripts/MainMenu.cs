using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Rendering;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject options;
    public GameObject controls;


    public TMP_Text fovText;
    public TMP_Text sensText;
    public Slider fovSlider;
    public Slider sensSlider;

    public RenderPipelineAsset lowQuality;
    public RenderPipelineAsset mediumQuality;
    public RenderPipelineAsset highQuality;


    private Animator animator;

    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        fovSlider.value = PlayerPrefs.GetFloat("FOV", 90);
        sensSlider.value = PlayerPrefs.GetFloat("SENS", 50);
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        SetSens();
        SetFov();
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitToDesktop()
    {
        Application.Quit();
    }

    public void OpenMainMenu()
    {
        options.gameObject.SetActive(false);
        controls.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }

    public void OpenOptions()
    {
        mainMenu.gameObject.SetActive(false);
        controls.gameObject.SetActive(false);
        options.gameObject.SetActive(true);
    }

    public void OpenControls()
    {
        mainMenu.gameObject.SetActive(false);
        options.gameObject.SetActive(false);
        controls.gameObject.SetActive(true);
    }

    public void Settings()
    {
        animator.SetTrigger("settings");
    }

    public void LowGraphics()
    {
        QualitySettings.SetQualityLevel(0);
        QualitySettings.renderPipeline = lowQuality;
        PlayerPrefs.SetInt("QualityLevel", 0);
    }
    public void MediumGraphics()
    {
        QualitySettings.SetQualityLevel(1);
        QualitySettings.renderPipeline = mediumQuality;
        PlayerPrefs.SetInt("QualityLevel", 1);
    }

    public void HighGraphics()
    {
        QualitySettings.SetQualityLevel(2);
        QualitySettings.renderPipeline = highQuality;
        PlayerPrefs.SetInt("QualityLevel", 2);
    }

    public void SetSens()
    {
        sensText.text = "Sensitivity: " + PlayerPrefs.GetFloat("SENS", 50);
        PlayerPrefs.SetFloat("SENS", sensSlider.value);
    }

    public void SetFov()
    {
        fovText.text = "FOV: " + PlayerPrefs.GetFloat("FOV", 90);
        PlayerPrefs.SetFloat("FOV", fovSlider.value);
    }
}


