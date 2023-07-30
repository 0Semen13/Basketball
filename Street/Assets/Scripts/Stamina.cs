using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour {
    [SerializeField] private Image staminaBar;
    [SerializeField] private float maxStamina;
    [SerializeField] private float stamina;

    [SerializeField] private int staminaBoostTime;

    private void Start() {
        stamina = maxStamina;
    }

    public void StaminaCalculation() { //Высчитывание стамины (Уменьшение)
        stamina -= 1;
        staminaBar.fillAmount = stamina / maxStamina;
        StartCoroutine(IncreaseStamina());
    }

    public int StaminaCheck() { //Проверка, есть ли стамина
        if (stamina == 0) return 0;
        else return 1;
    }

    private IEnumerator IncreaseStamina() {
        yield return new WaitForSeconds(staminaBoostTime);
        stamina += 1;
        staminaBar.fillAmount = stamina / maxStamina;
    }
}