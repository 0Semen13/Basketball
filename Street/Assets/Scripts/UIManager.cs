using System;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("œ‡ÌÂÎË UI")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject promoCodePanel;
    [SerializeField] private GameObject characteristicsPanel;
    [SerializeField] private GameObject aboutTheGamePanel;
    [SerializeField] private GameObject trainingPanel_1;
    [SerializeField] private GameObject trainingPanel_2;

    [Header("›ÎÂÏÂÌÚ˚ UI")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Dropdown qualityDropdown;
    [SerializeField] private Toggle languageToggle;
    [SerializeField] private Toggle vibrationToggle;
    [SerializeField] private Toggle vfxToggle;
    [SerializeField] private Toggle noticesToggle;
    [SerializeField] private Toggle fpsToggle;
    [SerializeField] private Toggle nightModeToggle;
    [SerializeField] private Text versionText;

    [SerializeField] private GameObject canvasControlPhone;
    [SerializeField] private Button[] buttons;
    [SerializeField] private GameObject fps;

    [TextArea(5, 5)] public string cheatCodes = "◊ËÚ ÍÓ‰˚:";
    [SerializeField] private InputField inputField;

    [SerializeField] private Text twoPointsText;
    [SerializeField] private Text threePointsText;
    [SerializeField] private Text extraLongPointsText;

    [SerializeField] private Text pointDisplay;
    [SerializeField] private Text ballDisplay;

    private Player playerScript;

    public event EventHandler onLanguageChange;

    private void Awake() {
        playerScript = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Start() {
        if (PlayerPrefs.GetInt("firstStart") == 0) {
            settingsPanel.gameObject.SetActive(true);
            trainingPanel_1.gameObject.SetActive(true);
        }
        else {
            settingsPanel.gameObject.SetActive(false);
            trainingPanel_1.gameObject.SetActive(false);
        }
        trainingPanel_2.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
        promoCodePanel.gameObject.SetActive(false);
        characteristicsPanel.gameObject.SetActive(false);
        aboutTheGamePanel.gameObject.SetActive(false);

        versionText.text = "Version: " + Application.version;

        LoadSettings();
    }

    // œ¿”« ¿

    public void PauseOpen() {
        Time.timeScale = 0;
        pausePanel.gameObject.SetActive(true);
        canvasControlPhone.gameObject.SetActive(false);

        for(int i = 0; i < buttons.Length; i++) {
            buttons[i].interactable = false;
        }
    }

    public void PauseClose() {
        Time.timeScale = 1;
        pausePanel.gameObject.SetActive(false);
        canvasControlPhone.gameObject.SetActive(true);

        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].interactable = true;
        }
    }

    public void CharacteristicsOpen() {
        twoPointsText.text = playerScript.GetCurrentPercentage(2).ToString();
        threePointsText.text = playerScript.GetCurrentPercentage(3).ToString();
        extraLongPointsText.text = playerScript.GetCurrentPercentage(4).ToString();
    }

    public void PassedTheRules() {
        PlayerPrefs.SetInt("firstStart", 1);
    }

    //Õ¿—“–Œ… »

    public void SetQality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetLanguage() {
        LocalisationSystem.SetLanguage(languageToggle.isOn);
        onLanguageChange?.Invoke(this, EventArgs.Empty);
    }

    public void SaveSettings() {
        PlayerPrefs.SetInt("Quality", qualityDropdown.value);

        PlayerPrefs.SetFloat("Music", musicSlider.value);
        PlayerPrefs.SetFloat("SFX", sfxSlider.value);

        PlayerPrefs.SetInt("Language", Convert.ToInt32(languageToggle.isOn));
        PlayerPrefs.SetInt("Vibration", Convert.ToInt32(vibrationToggle.isOn));
        PlayerPrefs.SetInt("VFX", Convert.ToInt32(vfxToggle.isOn));
        PlayerPrefs.SetInt("Notices", Convert.ToInt32(noticesToggle.isOn));
        PlayerPrefs.SetInt("FPS", Convert.ToInt32(fpsToggle.isOn));
        fps.SetActive(fpsToggle.isOn);
        PlayerPrefs.SetInt("NightMode", Convert.ToInt32(nightModeToggle.isOn));

        settingsPanel.gameObject.SetActive(false);
        PlayerPrefs.Save();
    }

    public void LoadSettings() {
        if (PlayerPrefs.HasKey("Quality")) qualityDropdown.value = PlayerPrefs.GetInt("Quality");
        else qualityDropdown.value = 1;

        if (PlayerPrefs.HasKey("Music")) musicSlider.value = PlayerPrefs.GetFloat("Music");
        else musicSlider.value = 1;
        if (PlayerPrefs.HasKey("SFX")) sfxSlider.value = PlayerPrefs.GetFloat("SFX");
        else sfxSlider.value = 1;

        if (PlayerPrefs.HasKey("Language")) languageToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Language"));
        else languageToggle.isOn = true;
        if (PlayerPrefs.HasKey("Vibration")) vibrationToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Vibration"));
        else vibrationToggle.isOn = true;
        if (PlayerPrefs.HasKey("VFX")) vfxToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("VFX"));
        else vfxToggle.isOn = true;
        if (PlayerPrefs.HasKey("Notices")) noticesToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Notices"));
        else noticesToggle.isOn = true;
        if (PlayerPrefs.HasKey("FPS")) fpsToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("FPS"));
        else fpsToggle.isOn = false;
        fps.SetActive(fpsToggle.isOn);
        if (PlayerPrefs.HasKey("NightMode")) nightModeToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("NightMode"));
        else nightModeToggle.isOn = false;
    }

    public void ResetSettings() {
        PlayerPrefs.DeleteKey("Quality");
        PlayerPrefs.DeleteKey("Music");
        PlayerPrefs.DeleteKey("SFX");
        PlayerPrefs.DeleteKey("Language");
        PlayerPrefs.DeleteKey("Vibration");
        PlayerPrefs.DeleteKey("VFX");
        PlayerPrefs.DeleteKey("Notices");
        PlayerPrefs.DeleteKey("FPS");
        PlayerPrefs.DeleteKey("NightMode");
        LoadSettings();
    }

    //◊»“- Œƒ€

    public void Console() {
        switch (inputField.text) {
            case ("1"):
            Debug.Log("1");
            break;

            default:
            Debug.Log(inputField.text);
            break;
        }

        inputField.text = "";
    }

    //ƒ–”√Œ≈

    public void SetTextPoints(int point, int numberBalls) {
        pointDisplay.text = point.ToString();
        ballDisplay.text = numberBalls.ToString();
    }
}