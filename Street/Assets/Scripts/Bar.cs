using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour {
    [SerializeField] private Image chanceBar;
    private float maxChance = 100f;
    public float maxChanceSpeed;
    public float startChanceSpeed;
    public float currentChanceSpeed;
    public float chance;
    public float increasingBarSpeed;

    private int flagVsBarRepet = 0; //Флаг, не начинающий бар с начала, пока кнопка не будет отпущена

    private bool buttonUp = false;
    private bool buttonDown = false;

    private void Start() {
        chance = 0f;
    }

    private void FixedUpdate() {
        if (Input.GetKey(KeyCode.Space) || buttonDown) {
            if (flagVsBarRepet == 0) {
                if (chance < 105) {
                    chance += currentChanceSpeed;
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

            if (chance > 100 && chance <= 110) {
                chance = 115;
            }
            else if (chance > 110) {
                chance = 35;
            }
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