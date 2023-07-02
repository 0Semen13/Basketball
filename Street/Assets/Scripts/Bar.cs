using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    private Image chanceBar;
    [SerializeField] private float chanceSpeed = 0.7f;
    private float maxChance = 100f;
    public float chance;

    private int flagBarNeedsReset = 0; //Флаг для обнуления шкалы после броска
    private int flagBarOutside = 0; //Флаг, срабатываемый при выходе шкалы за пределы Бара
    private int flagVsBarRepet = 0; //Флаг, не начинающий бар с начала, пока кнопка не будет отпущена

    void Start() {
        chanceBar = GetComponent<Image>();
        chance = 0f;
    }

    void FixedUpdate() {
        if (Input.GetKey(KeyCode.Space)) {
            if (flagVsBarRepet == 0) {
                if (chance < 105) {
                    if (flagBarNeedsReset == 1) {
                        chance = 0f;
                    }
                    flagBarNeedsReset = 0;

                    chance += chanceSpeed;
                }

                if (chance > 105) {
                    flagBarOutside = 1;
                    flagVsBarRepet = 1;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) || flagBarOutside == 1) {
            flagBarNeedsReset = 1;

            if (chance > 100 && chance <= 105) {
                chance = 100;
            }
            else if (chance > 105) {
                chance = 35;
                flagBarOutside = 0;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && flagVsBarRepet == 1) {
            flagVsBarRepet = 0;
        }

        chanceBar.fillAmount = chance / maxChance;
    }
}
