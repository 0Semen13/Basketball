using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class Player : MonoBehaviour {
    [Header("������ ����")]
    [SerializeField] private bool developerMode;
    [SerializeField] private bool isPC;
    [SerializeField] private bool isPhone;

    [Header("������������� ������")]
    [SerializeField] private float speed; //��������
    [SerializeField] public double currentPercentage2Point; //������� �������� ���������
    [SerializeField] public double currentPercentage3Points;
    [SerializeField] public double currentPercentageExtraLong;
    [SerializeField] private double startPercentage2Point = 70; //��������� �������� ���������
    [SerializeField] private double startPercentage3Points = 60;
    [SerializeField] private double startPercentageExtraLong = 50;
    [SerializeField] private double increasingMainZone = 0.5; //���������� ��������
    [SerializeField] private double increasingMiddleZone = 0.25;
    [SerializeField] private double increasingLastZone = 0.1;
    private float chanceForBar;

    [Header("�������")]
    [SerializeField] private Transform player; //�����
    [SerializeField] private Transform ball; //���
    [SerializeField] private Transform hands; //����
    [SerializeField] private Transform rightHand; //������ ����
    [SerializeField] private Transform posOverHead; //������� ��� ��� �������
    [SerializeField] private Transform posDribble; //������� ��� �����
    [SerializeField] private Transform target; //����
    [SerializeField] private Transform[] Misses; //������ ����� �������
    [SerializeField] private Transform teleportPosition; //������� ���������
    [SerializeField] private Transform positionBall;
    [SerializeField] private GameObject canvasControlPhone;
    [SerializeField] private GameObject GOBar;

    public int firstStart = 0;
    private bool ballInHands = false; //��� � �����
    private bool ballFlying = false; //��� ����� (�� � �����)
    private bool ballStart = true; //��� ��� ������
    private bool ban = false; //������ �� ������ �� ��������� ��������
    private bool ban2 = false; //������ �� ������ �� ������� ����
    private float t0 = 0;

    private bool twoPoint = false; //����� ������������, � ����� ���� ��������� �����
    private bool threePoint = false;
    private bool superPoint = true;
    private bool isHit = false;
    private int hitSuperPoint = 0;

    public bool buttonUpB = false; //�����, ������������, ������ �� ������ ��� ���
    public bool buttonDownB = false;

    private float ballHeightFactor; //������������ ������� � ������ ������ ����
    private float ballTimeRatio;

    private int num = 0; //�����, ������������ � ������������
    private int pnt = 0; //�����, ������� ����� ������ ���������� ����� ���������
    private int mss = 0; //�����, ���������� ������ ������� ��� �������

    public int point; //������� ����
    public int numberBalls; //������� ����

    [Header("�������� UI")]
    [SerializeField] private Text pointDisplay;
    [SerializeField] private Text ballDisplay;

    [SerializeField] private Joystick joystick;

    private SaveAndLoad SaveScript; //������ �� �������� ���������� � ��������

    private Rigidbody rigidBody;

    private void Start() {
        firstStart = PlayerPrefs.GetInt("firstStart");
        if(firstStart == 0) {
            currentPercentage2Point = startPercentage2Point;
            currentPercentage3Points = startPercentage3Points;
            currentPercentageExtraLong = startPercentageExtraLong;
            firstStart = 1;
            PlayerPrefs.SetInt("firstStart", firstStart);
            SaveScript.SaveGame(); //���������� ������
        }

        rigidBody = GetComponent<Rigidbody>();

        GOBar.gameObject.SetActive(false);

        player.position = teleportPosition.position;
        ball.position = positionBall.position;

        SaveScript = GameObject.Find("Save And Load").GetComponent<SaveAndLoad>(); //��������� ������� ��� ����������
        SaveScript.LoadGame(); //�������� ������
        point = GameObject.Find("Save And Load").GetComponent<SaveAndLoad>().point_S;
        numberBalls = GameObject.Find("Save And Load").GetComponent<SaveAndLoad>().numberBalls_S;
        currentPercentage2Point = GameObject.Find("Save And Load").GetComponent<SaveAndLoad>().percentage2Point_S;
        currentPercentage3Points = GameObject.Find("Save And Load").GetComponent<SaveAndLoad>().percentage3Point_S;
        currentPercentageExtraLong = GameObject.Find("Save And Load").GetComponent<SaveAndLoad>().percentageExtraLong_S;

        if (isPC && !isPhone) {
            canvasControlPhone.gameObject.SetActive(false);
        }
        else if (!isPC && isPhone) {
            canvasControlPhone.gameObject.SetActive(true);
        }
    }

    void FixedUpdate() {
        Vector3 direction = new Vector3(0, 0, 0);
        rigidBody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        if (isPC) {
            direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        }

        if (isPhone) {
            direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        }

        rigidBody.AddForce(direction * speed); //�������� ������     transform.position += direction * speed * Time.deltaTime;
        transform.LookAt(transform.position + direction); //������� ������

        if (ballInHands) {
            isHit = false;
            ballStart = false;
            pnt = 0;

            if ((buttonDownB || Input.GetKey(KeyCode.Space)) && !ban && !ban2) {
                rigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                GOBar.gameObject.SetActive(true);
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

                buttonDownB = false;
            }

            if ((buttonUpB || Input.GetKeyUp(KeyCode.Space)) && !ban && !ban2 && !ballFlying) {
                hands.localEulerAngles = Vector3.right * 0;
                rightHand.localEulerAngles = Vector3.left * 0;

                t0 = 0;
                num = Random.Range(1, 101); //���������� �����, ��� ��������� � ������������
                mss = Random.Range(1, 7); //����������, � ����� ������ �������

                chanceForBar = GameObject.Find("Chance_Bar").GetComponent<Bar>().chance;
                if (chanceForBar > 100 && chanceForBar <= 110) {
                    chanceForBar = 115;
                }
                else if (chanceForBar > 110) {
                    chanceForBar = 35;
                }

                buttonDownB = false;
                ballInHands = false;
                ballFlying = true;
                buttonUpB = false;
            }
            else {
                buttonUpB = false;
            }
        }
        else {
            buttonDownB = false;
            buttonUpB = false;
        }

        if (ballFlying && !ballStart) {
            GameObject.Find("Chance_Bar").GetComponent<Bar>().chance = chanceForBar;
            ThrowFunction();
        }

        if (developerMode) {                    //��� ������������
            if (Input.GetKey(KeyCode.F1)) { //������ ���� �� ������ �����
                ballInHands = true;
                ballFlying = false;
                ball.GetComponent<Rigidbody>().isKinematic = true;
            }

            if (Input.GetKey(KeyCode.F2)) { //��������� ���� ������
                SaveScript.ResetData();
                point = 0;
                numberBalls = 0;
            }

            if (Input.GetKey(KeyCode.F3)) { //��������� �������������
                currentPercentage2Point = startPercentage2Point;
                currentPercentage3Points = startPercentage3Points;
                currentPercentageExtraLong = startPercentageExtraLong;
                SaveScript.SaveGame();
            }
        }

        pointDisplay.text = point.ToString();
        ballDisplay.text = numberBalls.ToString();
    }

    private void ThrowFunction() {
        if (superPoint && threePoint && twoPoint) { //��� ������ ��������� � ������� ����
            if (num <= (currentPercentage2Point * chanceForBar) / 100) {
                isHit = true;
                pnt = 2;
            }
            ballHeightFactor = 3.5f;
            ballTimeRatio = 0.38f;
        }

        if (superPoint && threePoint && !twoPoint) { //��� ������ ��������� � ������� ����
            if (num <= (currentPercentage3Points * chanceForBar) / 100) {
                isHit = true;
                pnt = 3;
            }
            ballHeightFactor = 4.8f;
            ballTimeRatio = 0.44f;
        }

        if (superPoint && !threePoint && !twoPoint) { //��� ������ ��������� � ����� ������� ����
            if (num <= (currentPercentageExtraLong * chanceForBar) / 100) {
                isHit = true;
                pnt = 4;
            }
            else {
                hitSuperPoint = 0;
            }
            ballHeightFactor = 6;
            ballTimeRatio = 0.51f;
        }

        if (isHit) { //����� � ������
            t0 += Time.deltaTime;
            float duration = ballTimeRatio; //������������
            float time = t0 / duration; //����� ������

            Vector3 A = posOverHead.position;
            Vector3 B = target.position;
            Vector3 posFly = Vector3.Lerp(A, B, time); //��������� ������� (�����)
            Vector3 arc = Vector3.up * ballHeightFactor * Mathf.Sin(time * 3.14f);
            ball.position = posFly + arc;

            ball.Rotate(Random.Range(-1f, -0.55f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f)); //�������� ����

            if (time >= 1) {
                ballFlying = false;
                ball.GetComponent<Rigidbody>().isKinematic = false;

                if (pnt == 2) { //���������� �����
                    point += 2;

                    currentPercentage2Point += increasingMainZone;
                    currentPercentage3Points += increasingMiddleZone;
                    currentPercentageExtraLong += increasingLastZone;
                }
                else if (pnt == 3) {
                    point += 3;

                    currentPercentage3Points += increasingMainZone;
                    currentPercentageExtraLong += increasingMiddleZone;
                }
                else if (pnt == 4) {
                    point += 3 + hitSuperPoint;
                    hitSuperPoint += 1;

                    currentPercentage3Points += increasingMiddleZone;
                    currentPercentageExtraLong += increasingMainZone;
                }

                numberBalls += 1;
                GameObject.Find("Chance_Bar").GetComponent<Bar>().chance = 0;
                GOBar.gameObject.SetActive(false);

                SaveScript.SaveGame(); //���������� ������
            }
        }
        else {
            t0 += Time.deltaTime;
            float duration = ballTimeRatio; //������������
            float time = t0 / duration; //����� ������

            Vector3 A = posOverHead.position;
            Vector3 B = Misses[mss - 1].position;
            Vector3 posFly = Vector3.Lerp(A, B, time); //��������� ������� (�����)
            Vector3 arc = Vector3.up * ballHeightFactor * Mathf.Sin(time * 3.14f);
            ball.position = posFly + arc;

            ball.Rotate(Random.Range(-0.85f, -0.4f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f)); //�������� ����

            if (time >= 1) {
                ballFlying = false;
                ball.GetComponent<Rigidbody>().isKinematic = false;
                GameObject.Find("Chance_Bar").GetComponent<Bar>().chance = 0;
                GOBar.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Ball" && !ballInHands) {  //������ ����
            ballInHands = true;
            ball.GetComponent<Rigidbody>().isKinematic = true;
        }

        if (other.gameObject.tag == "Limits") { //����� �� ������� ��������
            player.position = teleportPosition.position;
        }

        if (other.gameObject.tag == "DeadZones") { //������� ���� ��� �������
            ban2 = true;

            if (GOBar.gameObject.activeInHierarchy) { //���� ����� ������� �� �����
                GameObject.Find("Chance_Bar").GetComponent<Bar>().chance = 0;
            }
            GOBar.gameObject.SetActive(false);
        }

        if (other.gameObject.tag == "2 Point") {
            twoPoint = true;
        }

        if (other.gameObject.tag == "3 Point") {
            threePoint = true;
        }

        if (other.gameObject.tag == "Extra long") {
            ban = false;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "2 Point") {
            twoPoint = false;
        }

        if (other.gameObject.tag == "3 Point") {
            threePoint = false;
        }

        if (other.gameObject.tag == "Extra long") {
            ban = true;

            if (GOBar.gameObject.activeInHierarchy) { //���� ����� ������� �� �����
                GameObject.Find("Chance_Bar").GetComponent<Bar>().chance = 0;
            }
            GOBar.gameObject.SetActive(false);
        }

        if (other.gameObject.tag == "DeadZones") {
            ban2 = false;
        }
    }

    public void ButtonUp() {
        buttonUpB = true;
    }

    public void ButtonDown() {
        buttonDownB = true;
    }
}