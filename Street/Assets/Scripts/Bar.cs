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

    private int flagVsBarRepet = 0; //����, �� ���������� ��� � ������, ���� ������ �� ����� ��������

    private bool buttonUp = false;
    private bool buttonDown = false;

    private void Start() {
        chance = 0f;
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
    public float GetStartChance() {
        return startChanceSpeed;
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
    public void SetMaxChance(float value) {
        maxChanceSpeed = value;
    }
    public void SetStartChance(float value) {
        startChanceSpeed = value;
    }
    public void SetCurrentChance(float value) {
        currentChanceSpeed = value;
    }
    public void SetIncreasingBarSpeed(float value) {
        increasingBarSpeed = value;
    }
    public void SetChance(float value) {
        chance = value;
    }
}