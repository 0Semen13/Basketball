using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour {
    [SerializeField] private float chanceSpeed = 0.7f;
    private Image chanceBar;
    private float maxChance = 100f;
    public float chance;

    private int flagVsBarRepet = 0; //Флаг, не начинающий бар с начала, пока кнопка не будет отпущена

    private bool buttonUp = false;
    private bool buttonDown = false;

    void Start() {
        chanceBar = GetComponent<Image>();
        chance = 0f;
    }

    void FixedUpdate() {
        chanceBar.fillAmount = chance / maxChance;

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
        }
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