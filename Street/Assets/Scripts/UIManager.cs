using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject SettingsPanel;

    [SerializeField] private GameObject canvasControlPhone;
    [SerializeField] private Button[] ArrButtons;

    private void Start() {
        SettingsPanel.gameObject.SetActive(false);
        PausePanel.gameObject.SetActive(false);
    }

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
}