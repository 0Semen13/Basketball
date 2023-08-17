using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour {
    [SerializeField] private Image staminaBar;
    [SerializeField] private float maxStamina;
    [SerializeField] private int staminaBoostTime;
    private float stamina;

    private bool StaminaIsFilled = false;

    private SaveAndLoad saveScript;

    private void Awake() {
        saveScript = GameObject.Find("Save And Load").GetComponent<SaveAndLoad>();
    }

    private void Start() {
        if (PlayerPrefs.GetInt("firstStart") == 0) {
            stamina = maxStamina;
            saveScript.SaveStamina();
        }
        else {
            saveScript.LoadStamina();
            stamina = saveScript.GetStamina();

            DateTime lastSaveTime = saveScript.LoadDateTime("lastSaveTime", DateTime.UtcNow);
            TimeSpan timePassed = DateTime.UtcNow - lastSaveTime;
            int secondPassed = (int)timePassed.TotalSeconds;
            secondPassed = Mathf.Clamp(secondPassed, 0, 2 * 60 * 60);
            stamina += secondPassed / staminaBoostTime;
            saveScript.SaveDateTime("lastSaveTime", DateTime.UtcNow);

            if (stamina > maxStamina) {
                stamina = maxStamina;
            }
        }
        staminaBar.fillAmount = stamina / maxStamina;

        if(stamina < maxStamina) {
            StaminaIsFilled = true;
            StartCoroutine(IncreaseStamina());
        }
    }

    public void StaminaReduction() { //Уменьшение стамины
        stamina -= 1;
        staminaBar.fillAmount = stamina / maxStamina;
        saveScript.SaveStamina();
        saveScript.SaveDateTime("lastSaveTime", DateTime.UtcNow);

        if (!StaminaIsFilled) {
            StartCoroutine(IncreaseStamina());
            StaminaIsFilled = true;
        }
    }

    private IEnumerator IncreaseStamina() {
        yield return new WaitForSeconds(staminaBoostTime);
        stamina += 1;
        staminaBar.fillAmount = stamina / maxStamina;
        saveScript.SaveStamina();

        if (StaminaIsFilled) {
            if (stamina < maxStamina) StartCoroutine(IncreaseStamina());
            else if (stamina == maxStamina) StaminaIsFilled = false;
        }
    }

    public float GetStamina() {
        return stamina;
    }
}