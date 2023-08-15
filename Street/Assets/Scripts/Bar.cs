using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour {
    [SerializeField] private Image chanceBar;
    private float maxChance = 100f;
    private float currentChanceSpeed;
    private float chance;
    [SerializeField] private float maxChanceSpeed;
    [SerializeField] private float startChanceSpeed;
    [SerializeField] private float increasingBarSpeed;

    [SerializeField] private Ball ballScript;

    private int flagVsBarRepet = 0; //Флаг, не начинающий бар с начала, пока кнопка не будет отпущена

    private bool buttonUp = false;
    private bool buttonDown = false;

    private SaveAndLoad saveScript;

    private void Awake() {
        saveScript = GameObject.Find("Save And Load").GetComponent<SaveAndLoad>();
    }

    private void Start() {
        chance = 0f;

        if (PlayerPrefs.GetInt("firstStart") == 0) {
            currentChanceSpeed = startChanceSpeed;
            saveScript.SaveBarSpeed();
        }
        else {
            saveScript.LoadBarSpeed();
            currentChanceSpeed = saveScript.GetBarSpeed();
        }
    }

    private void FixedUpdate() {
        if ((Input.GetKey(KeyCode.Space) || buttonDown) && !ballScript.GetBallPosition(2)) {
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

            if (chance > 100 && chance <= 110) {
                chance = 115;
            }
            else if (chance > 110) {
                chance = 35;
            }

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
    public float GetMaxChance() {
        return maxChanceSpeed;
    }
    public float GetCurrentChance() {
        return currentChanceSpeed;
    }
    public float GetIncreasingBarSpeed() {
        return increasingBarSpeed;
    }
    public float GetChance() {
        return chance;
    }
    public void SetCurrentChance(float value) {
        currentChanceSpeed = value;
    }
    public void SetChance(float value) {
        chance = value;
    }
}