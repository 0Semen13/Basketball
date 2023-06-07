using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class Player : MonoBehaviour {
    [SerializeField] private bool developerMode;
    [SerializeField] private bool isPC;
    [SerializeField] private bool isPhone;

    [SerializeField] private float speed; //��������

    [SerializeField] private Transform player; //�����
    [SerializeField] private Transform ball; //���
    [SerializeField] private Transform hands; //����
    [SerializeField] private Transform rightHand; //������� ����
    [SerializeField] private Transform posOverHead; //������� ��� ��� �������
    [SerializeField] private Transform posDribble; //������� ��� �����
    [SerializeField] private Transform target; //����
    [SerializeField] private Transform miss1; //�������
    [SerializeField] private Transform miss2;
    [SerializeField] private Transform miss3;
    [SerializeField] private Transform miss4;
    [SerializeField] private Transform miss5;
    [SerializeField] private Transform miss6;
    [SerializeField] private Transform teleportPosition; //������� ���������

    [SerializeField] private Transform[] Misses; //������ ����� �������

    private bool ballInHands = false; //��� � �����
    private bool ballFlying = false; //��� ����� (�� � �����)
    private bool ballStart = true; //��� ��� ������
    private float t0 = 0;

    [SerializeField] private double Percentage2Point = 50;
    [SerializeField] private double Percentage3Point = 40;
    [SerializeField] private double PercentageExtraLong = 30;

    private bool TwoPoint = false;
    private bool ThreePoint = false;
    private bool SuperPoint = true;
    private bool isHit = false;

    private int num = 1; //�����, ������������ � ������������
    private int pnt = 0; //�����, ������� ����� ������ ���������� ����� ���������
    private int mss = 0; //�����, ���������� ������ ������� ��� �������

    public int point;
    public int numberBalls;

    [SerializeField] private Text pointDisplay;
    [SerializeField] private Text ballDisplay;

    [SerializeField] private SaveAndLoad SaveScript; //������ �� �������� ���������� � ��������

    [SerializeField] private Joystick joystick;

    private bool ButtonUpB = false;
    private bool ButtonDownB = false;

    [SerializeField] private GameObject canvasControlPhone; //�����

    private void Start() {
        player.position = teleportPosition.position;

        SaveScript = GameObject.Find("Save And Load").GetComponent<SaveAndLoad>(); //��������� ������� ��� ����������

        SaveScript.LoadGame(); //�������� ������
        GameObject GO = GameObject.Find("Save And Load");
        SaveAndLoad saveAndLoad_point = GO.GetComponent<SaveAndLoad>();
        point = saveAndLoad_point.point_S;

        SaveAndLoad saveAndLoad_balls = GO.GetComponent<SaveAndLoad>();
        numberBalls = saveAndLoad_balls.numberBalls_S;

        if (isPC && !isPhone) {
            canvasControlPhone.gameObject.SetActive(false);
        }
        else if(!isPC && isPhone) {
            canvasControlPhone.gameObject.SetActive(true);
        }
    }

    void Update() {
        if (isPC) {
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            transform.position += direction * speed * Time.deltaTime; //�������� ������
            transform.LookAt(transform.position + direction); //������� ������

            if (ballInHands) {
                isHit = false;
                ballStart = false;
                pnt = 0;

                if (Input.GetKey(KeyCode.Space)) {
                    ball.position = posOverHead.position; //�������� ��� � ���� ��� ������� ������� � ���� � �����
                    rightHand.localEulerAngles = Vector3.left * 0;
                    hands.localEulerAngles = Vector3.right * 180;

                    transform.LookAt(target.position);
                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                }
                else {
                    ball.position = posDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5) * 2);
                    hands.localEulerAngles = Vector3.right * 0;
                    rightHand.localEulerAngles = Vector3.left * 50;
                }

                if (Input.GetKeyUp(KeyCode.Space)) {
                    rightHand.localEulerAngles = Vector3.left * 0;
                    ballInHands = false;
                    ballFlying = true;
                    t0 = 0;
                    num = Random.Range(1, 100); //���������� �����, ��� ��������� � ������������
                    mss = Random.Range(1, 7); //����������, ���� ������� ������
                }
            }

            if (ballFlying && !ballStart) {
                if (SuperPoint && ThreePoint && TwoPoint) { //��� ������ ��������� � ������� ����
                    if (num <= Percentage2Point) {
                        isHit = true;
                        pnt = 2;
                    }
                }

                if (SuperPoint && ThreePoint && !TwoPoint) { //��� ������ ��������� � ������� ����
                    if (num <= Percentage3Point) {
                        isHit = true;
                        pnt = 3;
                    }
                }

                if (SuperPoint && !ThreePoint && !TwoPoint) { //��� ������ ��������� � ����� ������� ����
                    if (num <= PercentageExtraLong) {
                        isHit = true;
                        pnt = 4;
                    }
                }

                if (isHit) { //����� � ������
                    t0 += Time.deltaTime;
                    float duration = 0.5f; //������������
                    float time = t0 / duration; //����� ������

                    Vector3 A = posOverHead.position;
                    Vector3 B = target.position;
                    Vector3 posFly = Vector3.Lerp(A, B, time); //��������� ������� (�����)
                    Vector3 arc = Vector3.up * 5 * Mathf.Sin(time * 3.14f);
                    ball.position = posFly + arc;

                    ball.Rotate(Random.Range(-0.85f, -0.4f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f)); //�������� ����

                    if (time >= 1) {
                        ballFlying = false;
                        ball.GetComponent<Rigidbody>().isKinematic = false;

                        if (pnt == 2) { //���������� �����
                            Debug.Log("+2 ����! ������ ����� " + num + ", ������� ������ � ���������� " + Percentage2Point);
                            point += 2;
                        }
                        else if (pnt == 3) {
                            Debug.Log("+3 ����! ������ ����� " + num + ", ������� ������ � ���������� " + Percentage3Point);
                            point += 3;
                        }
                        else if (pnt == 4) {
                            Debug.Log("+4 ����! ������ ����� " + num + ", ������� ������ � ���������� " + PercentageExtraLong);
                            point += 4;
                        }

                        numberBalls += 1;

                        SaveScript.SaveGame(); //���������� ����� � ����� ����� ���������
                    }
                }
                else {
                    t0 += Time.deltaTime;
                    float duration = 0.5f; //������������
                    float time = t0 / duration; //����� ������

                    Vector3 A = posOverHead.position;
                    Vector3 B = Misses[mss - 1].position;
                    Vector3 posFly = Vector3.Lerp(A, B, time); //��������� ������� (�����)
                    Vector3 arc = Vector3.up * 5 * Mathf.Sin(time * 3.14f);
                    ball.position = posFly + arc;

                    ball.Rotate(Random.Range(-0.85f, -0.4f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f)); //�������� ����

                    if (time >= 1) {
                        ballFlying = false;
                        ball.GetComponent<Rigidbody>().isKinematic = false;
                        Debug.Log("������! ������ �����: " + num);
                    }
                }
            }
        }

        if (isPhone) {
            Vector3 direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
            transform.position += direction * speed * Time.deltaTime; //�������� ������
            transform.LookAt(transform.position + direction); //������� ������

            if (ballInHands) {
                isHit = false;
                ballStart = false;
                pnt = 0;

                if (ButtonDownB) {
                    ball.position = posOverHead.position; //�������� ��� � ���� ��� ������� ������� � ���� � �����
                    rightHand.localEulerAngles = Vector3.left * 0;
                    hands.localEulerAngles = Vector3.right * 180;

                    transform.LookAt(target.position);
                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                }
                else {
                    ball.position = posDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5) * 2);
                    hands.localEulerAngles = Vector3.right * 0;
                    rightHand.localEulerAngles = Vector3.left * 50;
                }

                if (ButtonUpB) {
                    ButtonDownB = false;

                    hands.localEulerAngles = Vector3.right * 0;
                    rightHand.localEulerAngles = Vector3.left * 0;
                    ballInHands = false;
                    ballFlying = true;
                    t0 = 0;
                    num = Random.Range(1, 100); //���������� �����, ��� ��������� � ������������
                    mss = Random.Range(1, 7); //����������, ���� ������� ������

                    ButtonUpB = false;
                }
            }
            else {
                ButtonDownB = false;
                ButtonUpB = false;

            }

            if (ballFlying && !ballStart) {
                if (SuperPoint && ThreePoint && TwoPoint) { //��� ������ ��������� � ������� ����
                    if (num <= Percentage2Point) {
                        isHit = true;
                        pnt = 2;
                    }
                }

                if (SuperPoint && ThreePoint && !TwoPoint) { //��� ������ ��������� � ������� ����
                    if (num <= Percentage3Point) {
                        isHit = true;
                        pnt = 3;
                    }
                }

                if (SuperPoint && !ThreePoint && !TwoPoint) { //��� ������ ��������� � ����� ������� ����
                    if (num <= PercentageExtraLong) {
                        isHit = true;
                        pnt = 4;
                    }
                }

                if (isHit) { //����� � ������
                    t0 += Time.deltaTime;
                    float duration = 0.5f; //������������
                    float time = t0 / duration; //����� ������

                    Vector3 A = posOverHead.position;
                    Vector3 B = target.position;
                    Vector3 posFly = Vector3.Lerp(A, B, time); //��������� ������� (�����)
                    Vector3 arc = Vector3.up * 5 * Mathf.Sin(time * 3.14f);
                    ball.position = posFly + arc;

                    ball.Rotate(Random.Range(-0.85f, -0.4f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f)); //�������� ����

                    if (time >= 1) {
                        ballFlying = false;
                        ball.GetComponent<Rigidbody>().isKinematic = false;

                        if (pnt == 2) { //���������� �����
                            Debug.Log("+2 ����! ������ ����� " + num + ", ������� ������ � ���������� " + Percentage2Point);
                            point += 2;
                        }
                        else if (pnt == 3) {
                            Debug.Log("+3 ����! ������ ����� " + num + ", ������� ������ � ���������� " + Percentage3Point);
                            point += 3;
                        }
                        else if (pnt == 4) {
                            Debug.Log("+4 ����! ������ ����� " + num + ", ������� ������ � ���������� " + PercentageExtraLong);
                            point += 4;
                        }

                        numberBalls += 1;

                        SaveScript.SaveGame(); //���������� ����� � ����� ����� ���������
                    }
                }
                else {
                    t0 += Time.deltaTime;
                    float duration = 0.5f; //������������
                    float time = t0 / duration; //����� ������

                    Vector3 A = posOverHead.position;
                    Vector3 B = Misses[mss - 1].position;
                    Vector3 posFly = Vector3.Lerp(A, B, time); //��������� ������� (�����)
                    Vector3 arc = Vector3.up * 5 * Mathf.Sin(time * 3.14f);
                    ball.position = posFly + arc;

                    ball.Rotate(Random.Range(-0.85f, -0.4f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f)); //�������� ����

                    if (time >= 1) {
                        ballFlying = false;
                        ball.GetComponent<Rigidbody>().isKinematic = false;
                        Debug.Log("������! ������ �����: " + num);
                    }
                }
            }
        }

        if (developerMode) {                    //��� ������������
            if (Input.GetKey(KeyCode.F1)) { //������ ���� �� ������ �����
                ballInHands = true;
                ballFlying = false;
                ball.GetComponent<Rigidbody>().isKinematic = true;
            }

            if (Input.GetKey(KeyCode.F2)) { //��������� ������
                SaveScript.ResetData();
                point = 0;
                numberBalls = 0;
            }
        }

        pointDisplay.text = point.ToString();
        ballDisplay.text = numberBalls.ToString();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Ball" && !ballInHands) {  //������ ����
            ballInHands = true;
            ball.GetComponent<Rigidbody>().isKinematic = true;
        }

        if (other.gameObject.tag == "2 Point") {
            TwoPoint = true;
        }

        if (other.gameObject.tag == "3 Point") {
            ThreePoint = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "2 Point") {
            TwoPoint = false;
        }

        if (other.gameObject.tag == "3 Point") {
            ThreePoint = false;
        }
    }

    public void ButtonUp() {
        ButtonUpB = true;
    }

    public void ButtonDown() {
        ButtonDownB = true;
    }
}