using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Панели UI")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject promoCodePanel;

    [Header("Элементы UI")]
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

    [TextArea(5, 5)] public string cheatCodes = "Чит коды:";
    [SerializeField] private InputField inputField;

    [SerializeField] private Text twoPointsText;
    [SerializeField] private Text threePointsText;
    [SerializeField] private Text extraLongPointsText;

    private Player playerScript;

    private void Start() {
        //settingsPanel.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
        promoCodePanel.gameObject.SetActive(false);
        versionText.text = "Version: " + Application.version;
        LoadSettings();
    }

    // ПАУЗКА

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
        playerScript = GameObject.Find("Player").GetComponent<Player>();

        twoPointsText.text += playerScript.currentPercentage2Point;
        threePointsText.text += playerScript.currentPercentage3Points;
        extraLongPointsText.text += playerScript.currentPercentageExtraLong;
    }

    public void CharacteristicsClose() {
        playerScript = GameObject.Find("Player").GetComponent<Player>();

        twoPointsText.text = "Средние: ";
        threePointsText.text = "Дальние: ";
        extraLongPointsText.text = "Сверх-дальние: ";
    }

    //НАСТРОЙКИ

    public void SetQality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SaveSettings() {
        PlayerPrefs.SetInt("Quality", qualityDropdown.value);

        PlayerPrefs.SetFloat("Music", musicSlider.value);
        PlayerPrefs.SetFloat("SFX", sfxSlider.value);

        PlayerPrefs.SetInt("Language", System.Convert.ToInt32(languageToggle.isOn));
        PlayerPrefs.SetInt("Vibration", System.Convert.ToInt32(vibrationToggle.isOn));
        PlayerPrefs.SetInt("VFX", System.Convert.ToInt32(vfxToggle.isOn));
        PlayerPrefs.SetInt("Notices", System.Convert.ToInt32(noticesToggle.isOn));
        PlayerPrefs.SetInt("FPS", System.Convert.ToInt32(fpsToggle.isOn));
        fps.SetActive(fpsToggle.isOn);
        PlayerPrefs.SetInt("NightMode", System.Convert.ToInt32(nightModeToggle.isOn));

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

        if (PlayerPrefs.HasKey("Language")) languageToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("Language"));
        else languageToggle.isOn = true;
        if (PlayerPrefs.HasKey("Vibration")) vibrationToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("Vibration"));
        else vibrationToggle.isOn = true;
        if (PlayerPrefs.HasKey("VFX")) vfxToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("VFX"));
        else vfxToggle.isOn = true;
        if (PlayerPrefs.HasKey("Notices")) noticesToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("Notices"));
        else noticesToggle.isOn = true;
        if (PlayerPrefs.HasKey("FPS")) fpsToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("FPS"));
        else fpsToggle.isOn = false;
        fps.SetActive(fpsToggle.isOn);
        if (PlayerPrefs.HasKey("NightMode")) nightModeToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("NightMode"));
        else nightModeToggle.isOn = false;
    }

    public void ResetSettings() { //Сброс настроек
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

    //Чит-Код

    public void Console() { //Чит консоль
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
}