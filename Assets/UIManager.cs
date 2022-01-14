using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class UIManager : MonoBehaviour
{
    public Sprite blankImage;
    public Image primaryWeaponImage;
    public Image secondaryWeaponImage;
    public Image bloodScreen;


    public TMP_Text healthText;
    public TMP_Text ammoText;
    public TMP_Text scrapText;
    public TMP_Text medkitText;

    public TMP_Text fovText;
    public TMP_Text sensText;
    public Slider fovSlider;
    public Slider sensSlider;
    public Camera cam;


    public GameObject UI;
    public GameObject pauseScreen;
    public GameObject optionsScreen;
    public GameObject deathScreen;
    public bool gamePaused;

    public PlayerInventory playerInv;
    private PlayerMovement playerMove;

    public RenderPipelineAsset lowQuality;
    public RenderPipelineAsset mediumQuality;
    public RenderPipelineAsset highQuality;
    private void Awake()
    {
        fovSlider.value = PlayerPrefs.GetFloat("FOV", 90);
        sensSlider.value = PlayerPrefs.GetFloat("SENS", 50);
    }
    private void Start()
    {
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        playerInv = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        healthText.text = "HEALTH: " + playerInv.currentHealth.ToString();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Time.timeScale = 1f;
        //SetGraphicsSettings();
    }

    private void Update()
    {
        SetFov();
        SetSens();
        SetAmmoText();
        SetHealthText();
        SetScrapText();
        SetMedKitText();
    }

    public void PauseCheck()
    {
        if (gamePaused)
        {
            UI.SetActive(false);
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (!gamePaused)
        {
            UI.SetActive(true);
            pauseScreen.SetActive(false);
            optionsScreen.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void SetSens()
    {
        sensText.text = "Sensitivity: " + PlayerPrefs.GetFloat("SENS", 50);
        PlayerPrefs.SetFloat("SENS", sensSlider.value);
        playerMove.sensitivity = PlayerPrefs.GetFloat("SENS");
    }

    public void SetFov()
    {
        fovText.text = "FOV: " + PlayerPrefs.GetFloat("FOV", 90);
        PlayerPrefs.SetFloat("FOV", fovSlider.value);
        cam.fieldOfView = PlayerPrefs.GetFloat("FOV");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {

        SceneManager.LoadScene(0);
    }

    private void SetAmmoText()
    {
        if (playerInv.weaponHolster[playerInv.weaponToEquip] != playerInv.emptySlot)
        {
            ammoText.text = playerInv.weaponHolster[playerInv.weaponToEquip].GetComponent<Gun>().currentAmmo.ToString() + " / " + playerInv.weaponHolster[playerInv.weaponToEquip].GetComponent<Gun>().totalWeaponAmmo.ToString();
        }
        else
        {
            ammoText.text = "";
        }
       
    }

    public void QuitToDesktop()
    {
        Application.Quit();
    }

    private void SetHealthText()
    {
        healthText.text = "HEALTH: " + playerInv.currentHealth.ToString();
    }

    private void SetScrapText()
    {
        scrapText.text = "SCRAP: " + playerInv.scrapAmount.ToString();
    }

    private void SetMedKitText()
    {
        medkitText.text = "MEDKITS: " + playerInv.medKits.ToString();
    }

    public void ResumeGame()
    {
        gamePaused = false;
        PauseCheck();
    }

    public void OpenOptions()
    {
        pauseScreen.SetActive(false);
        optionsScreen.SetActive(true);
    }

    public void OpenPause()
    {
        optionsScreen.SetActive(false);
        pauseScreen.SetActive(true);
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

    public void SetGraphicsSettings()
    {
        if (PlayerPrefs.GetInt("QualityLevel", 2) == 0)
        {
            LowGraphics();
        }
        if (PlayerPrefs.GetInt("QualityLevel", 2) == 1)
        {
            MediumGraphics();
        }
        if (PlayerPrefs.GetInt("QualityLevel", 2) == 2)
        {
            HighGraphics();
        }
    }
}
