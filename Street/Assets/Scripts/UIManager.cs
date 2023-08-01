using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Панели UI")]
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject SettingsPanel;

    [Header("Элементы UI")]
    [SerializeField] private Dropdown QualityDropdown;

    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SFXSlider;

    [SerializeField] private Toggle LanguageToggle;
    [SerializeField] private Toggle VibrationToggle;
    [SerializeField] private Toggle VFXToggle;
    [SerializeField] private Toggle NoticesToggle;
    [SerializeField] private Toggle FPSToggle;
    [SerializeField] private Toggle NightModeToggle;

    [SerializeField] private GameObject canvasControlPhone;
    [SerializeField] private Button[] ArrButtons;
    [SerializeField] private GameObject FPS;

    [SerializeField] private Text VersionText;

    private void Start() {
        SettingsPanel.gameObject.SetActive(false);
        PausePanel.gameObject.SetActive(false);
        VersionText.text = "Version: " + Application.version;
        LoadSettings();
    }

    // ПАУЗКА

    public void PauseOpen() {
        PausePanel.gameObject.SetActive(true);
        Time.timeScale = 0;
        canvasControlPhone.gameObject.SetActive(false);

        for(int i = 0; i < ArrButtons.Length; i++) {
            ArrButtons[i].interactable = false;
        }
    }

    public void PauseClose() {
        PausePanel.gameObject.SetActive(false);
        Time.timeScale = 1;
        canvasControlPhone.gameObject.SetActive(true);

        for (int i = 0; i < ArrButtons.Length; i++) {
            ArrButtons[i].interactable = true;
        }
    }

    //НАСТРОЙКИ

    public void SetQality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SaveSettings() {
        PlayerPrefs.SetInt("Quality", QualityDropdown.value);

        PlayerPrefs.SetFloat("Music", MusicSlider.value);
        PlayerPrefs.SetFloat("SFX", SFXSlider.value);

        PlayerPrefs.SetInt("Language", System.Convert.ToInt32(LanguageToggle.isOn));
        PlayerPrefs.SetInt("Vibration", System.Convert.ToInt32(VibrationToggle.isOn));
        PlayerPrefs.SetInt("VFX", System.Convert.ToInt32(VFXToggle.isOn));
        PlayerPrefs.SetInt("Notices", System.Convert.ToInt32(NoticesToggle.isOn));
        PlayerPrefs.SetInt("FPS", System.Convert.ToInt32(FPSToggle.isOn));
        FPS.SetActive(FPSToggle.isOn);
        PlayerPrefs.SetInt("NightMode", System.Convert.ToInt32(NightModeToggle.isOn));

        SettingsPanel.gameObject.SetActive(false);
        PlayerPrefs.Save();
    }

    public void LoadSettings() {
        if (PlayerPrefs.HasKey("Quality")) QualityDropdown.value = PlayerPrefs.GetInt("Quality");
        else QualityDropdown.value = 1;

        if (PlayerPrefs.HasKey("Music")) MusicSlider.value = PlayerPrefs.GetFloat("Music");
        else MusicSlider.value = 1;
        if (PlayerPrefs.HasKey("SFX")) SFXSlider.value = PlayerPrefs.GetFloat("SFX");
        else SFXSlider.value = 1;

        if (PlayerPrefs.HasKey("Language")) LanguageToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("Language"));
        else LanguageToggle.isOn = true;
        if (PlayerPrefs.HasKey("Vibration")) VibrationToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("Vibration"));
        else VibrationToggle.isOn = true;
        if (PlayerPrefs.HasKey("VFX")) VFXToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("VFX"));
        else VFXToggle.isOn = true;
        if (PlayerPrefs.HasKey("Notices")) NoticesToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("Notices"));
        else NoticesToggle.isOn = true;
        if (PlayerPrefs.HasKey("FPS")) FPSToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("FPS"));
        else FPSToggle.isOn = false;
        FPS.SetActive(FPSToggle.isOn);
        if (PlayerPrefs.HasKey("NightMode")) NightModeToggle.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("NightMode"));
        else NightModeToggle.isOn = false;
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
}