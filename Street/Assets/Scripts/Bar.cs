using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour {
    private Image chanceBar;
    private float maxChance = 100f;
    public float maxChanceSpeed = 1.1f;
    public float chanceSpeed;
    public float chance;

    private int flagVsBarRepet = 0; //Флаг, не начинающий бар с начала, пока кнопка не будет отпущена

    private bool buttonUp = false;
    private bool buttonDown = false;

    void Start() {
        chanceBar = GetComponent<Image>();
        chance = 0f;
    }

    void FixedUpdate() {          
        if (Input.GetKey(KeyCode.Space) || buttonDown) {
            if (flagVsBarRepet == 0) {
                if (chance < 105) {
                    chance += chanceSpeed;
                }

                if (chance > 105) {
                    flagVsBarRepet = 1;
                    chance = 35;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) || buttonUp) {
            flagVsBarRepet = 0;
            buttonUp = false;
        }

        chanceBar.fillAmount = chance / maxChance;
    }

    public void ButtonUp() {
        buttonUp = true;
        buttonDown = false;
    }

    public void ButtonDown() {
        buttonDown = true;
        buttonUp = false;
    }
}