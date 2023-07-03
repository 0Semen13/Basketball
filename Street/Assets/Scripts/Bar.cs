using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour {
    [SerializeField] private bool isPC;
    [SerializeField] private bool isPhone;

    [SerializeField] private float chanceSpeed = 0.7f;
    private Image chanceBar;
    private float maxChance = 100f;
    public float chance;

    private int flagBarNeedsReset = 0; //���� ��� ��������� ����� ����� ������
    private int flagBarOutside = 0; //����, ������������� ��� ������ ����� �� ������� ����
    private int flagVsBarRepet = 0; //����, �� ���������� ��� � ������, ���� ������ �� ����� ��������

    void Start() {
        chanceBar = GetComponent<Image>();
        chance = 0f;
    }

    void FixedUpdate() {
        if (isPC) {
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

                if (chance > 105) {
                    chance = 35;
                    flagBarOutside = 0;
                }
            }

            if (Input.GetKeyUp(KeyCode.Space) && flagVsBarRepet == 1) {
                flagVsBarRepet = 0;
            }

            chanceBar.fillAmount = chance / maxChance;
        }

        if (isPhone) {

        }
    }
}
