using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    [Header("������ ����")]
    [SerializeField] private bool developerMode;
    [SerializeField] private bool isPhone;

    [Header("������������� ������")]
    [SerializeField] private float speedWithTheBalled; //�������� � �����
    [SerializeField] private float speedWithoutBall; //�������� ��� ����
    private float speed = 0;
    [SerializeField] private double startPercentage2Point; //��������� �������� ���������
    [SerializeField] private double startPercentage3Points;
    [SerializeField] private double startPercentageExtraLong;
    [SerializeField] public double currentPercentage2Point; //������� �������� ���������
    [SerializeField] public double currentPercentage3Points;
    [SerializeField] public double currentPercentageExtraLong;
    [SerializeField] private double increasingMainZone; //���������� �������� � ��������� �� ���������
    [SerializeField] private double increasingMiddleZone;
    [SerializeField] private double increasingLastZone;

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
    [SerializeField] private GameObject FPSLimitation;
    [SerializeField] private GameObject model;

    private int firstStart = 0;
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

    private bool buttonUpB = false; //�����, ������������, ������ �� ������ ��� ���
    private bool buttonDownB = false;

    private float ballHeightFactor; //������������ ������ ������ ����
    private float ballTimeRatio; //������������ ������� ������ ����

    private int num = 0; //�����, ������������ � ������������
    private int pnt = 0; //�����, ������� ����� ������ ���������� ����� ���������
    private int mss = 0; //�����, ���������� ������ ������� ��� �������

    public int point; //������� ����
    public int numberBalls; //������� ����
    public int addingPoints; //����, ����� ������� ���������� ����� �����������

    [Header("�������� UI")]
    [SerializeField] private Text pointDisplay;
    [SerializeField] private Text ballDisplay;
    [SerializeField] private Joystick joystick;
    [SerializeField] private GameObject canvasControlPhone;
    [SerializeField] private GameObject GOBar;

    private SaveAndLoad SaveScript; //������� ��� �����������
    private Bar BarScript;
    private Stamina StaminaScript;
    private Rigidbody rigidBody;
    private Rigidbody rigidBodyBall;
    private Animator animator;

    private void Start() {
        rigidBody = player.GetComponent<Rigidbody>(); //��������� rigidbody
        rigidBodyBall = ball.GetComponent<Rigidbody>();
        SaveScript = GameObject.Find("Save And Load").GetComponent<SaveAndLoad>(); //��������� ��������
        BarScript = GameObject.Find("Bar").GetComponent<Bar>();
        StaminaScript = GameObject.Find("Stamina").GetComponent<Stamina>();
        animator = model.GetComponent<Animator>(); //��������� ���������

        firstStart = PlayerPrefs.GetInt("firstStart");
        if (firstStart == 0) { //������ ����� � ����
            currentPercentage2Point = startPercentage2Point;
            currentPercentage3Points = startPercentage3Points;
            currentPercentageExtraLong = startPercentageExtraLong;
            addingPoints = 50;
            BarScript.currentChanceSpeed = BarScript.startChanceSpeed  ;
            firstStart = 1;
            PlayerPrefs.SetInt("firstStart", firstStart);
            SaveScript.SaveGame(); //���������� ������
        }

        SaveScript.LoadGame(); //�������� ������
        currentPercentage2Point = SaveScript.percentage2Point_S;
        currentPercentage3Points = SaveScript.percentage3Point_S;
        currentPercentageExtraLong = SaveScript.percentageExtraLong_S;
        addingPoints = SaveScript.addingPoints_S;
        BarScript.currentChanceSpeed = SaveScript.chanceSpeed_S;

        point = SaveScript.point_S;
        numberBalls = SaveScript.numberBalls_S;
        pointDisplay.text = "Points: " + point.ToString();
        ballDisplay.text = "Balls: " + numberBalls.ToString();

        player.position = teleportPosition.position;
        ball.position = positionBall.position;

        GOBar.gameObject.SetActive(false);

        if (isPhone) {
            canvasControlPhone.gameObject.SetActive(true);
            FPSLimitation.gameObject.SetActive(true);
        }
        else {
            canvasControlPhone.gameObject.SetActive(false);
            FPSLimitation.gameObject.SetActive(false);
        }
    }

    void FixedUpdate() {
        Vector3 direction = new Vector3(0, 0, 0);

        if (isPhone) direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        else direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (direction == new Vector3(0, 0, 0)) {
            animator.SetFloat("Move", 0);
        }
        else {
            animator.SetFloat("Move", 1);
        }

        rigidBody.AddForce(direction * speed); //�������� ������
        player.LookAt(player.position + direction); //������� ������

        if (ballInHands) {
            speed = speedWithTheBalled;
            animator.SetBool("Ball", true);
            isHit = false;
            ballStart = false;
            pnt = 0;

            if(StaminaScript.StaminaCheck() == 1) {
                if ((buttonDownB || Input.GetKey(KeyCode.Space)) && !ban && !ban2) {
                    rigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                    GOBar.gameObject.SetActive(true);
                    animator.SetFloat("Move", 0);
                    animator.SetBool("Throw", true);
                    ball.position = posOverHead.position;

                    player.LookAt(target.position);
                    player.eulerAngles = new Vector3(0, player.eulerAngles.y, 0);
                }
                else {
                    ball.position = posDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5) * 2);

                    buttonDownB = false;
                }

                if ((buttonUpB || Input.GetKeyUp(KeyCode.Space)) && !ban && !ban2 && !ballFlying) {
                    rigidBody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

                    t0 = 0;
                    num = Random.Range(1, 101); //���������� �����, ��� ��������� � ������������
                    mss = Random.Range(1, 7); //����������, � ����� ������ �������

                    ballInHands = false;
                    ballFlying = true;
                    buttonDownB = false;
                    buttonUpB = false;
                }
                else {
                    buttonUpB = false;
                }
            }
            else{
                ball.position = posDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5) * 2);
            }
        }
        else {
            speed = speedWithoutBall;
            buttonDownB = false;
            buttonUpB = false;
        }

        if (ballFlying && !ballStart) {
            animator.SetBool("Throw", false);
            animator.SetBool("Ball", false);
            ThrowFunction();
        }

        if (developerMode) {                    //��� ������������
            if (Input.GetKey(KeyCode.F1)) { //������ ���� �� ������ �����
                ballInHands = true;
                ballFlying = false;
                rigidBodyBall.isKinematic = true;
                animator.SetBool("Ball", true);
            }

            if (Input.GetKey(KeyCode.F2)) { //��������� ���� ������
                SaveScript.ResetData();
                point = 0;
                numberBalls = 0;
                pointDisplay.text = "Points: " + point.ToString();
                ballDisplay.text = "Balls: " + numberBalls.ToString();
            }
        }
    }

    private void ThrowFunction() {
        if (superPoint && threePoint && twoPoint) { //��� ������ ��������� � ������� ����
            if (num <= (currentPercentage2Point * BarScript.chance) / 100) {
                isHit = true;
                pnt = 2;
            }
            ballHeightFactor = 3.6f;
            ballTimeRatio = 0.41f;
        }

        if (superPoint && threePoint && !twoPoint) { //��� ������ ��������� � ������� ����
            if (num <= (currentPercentage3Points * BarScript.chance) / 100) {
                isHit = true;
                pnt = 3;
            }
            ballHeightFactor = 4.8f;
            ballTimeRatio = 0.46f;
        }

        if (superPoint && !threePoint && !twoPoint) { //��� ������ ��������� � ����� ������� ����
            if (num <= (currentPercentageExtraLong * BarScript.chance) / 100) {
                isHit = true;
                pnt = 4;
            }
            else {
                hitSuperPoint = 0;
            }
            ballHeightFactor = 6.2f;
            ballTimeRatio = 0.55f;
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
                rigidBodyBall.isKinematic = false;

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
                BarScript.chance = 0;

                if (point >= addingPoints) {
                    addingPoints += 50;
                    if (BarScript.currentChanceSpeed < BarScript.maxChanceSpeed) {
                        BarScript.currentChanceSpeed += BarScript.increasing_BarSpeed;
                        SaveScript.SaveBarSpeed();
                    }
                    SaveScript.SaveAddingPoints();
                }

                pointDisplay.text = "Points: " + point.ToString();
                ballDisplay.text = "Balls: " + numberBalls.ToString();

                StaminaScript.StaminaCalculation(); //���������� ������� ����� ������
                SaveScript.SaveCharacteristicsAndPoints();
                GOBar.gameObject.SetActive(false);
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
                rigidBodyBall.isKinematic = false;
                BarScript.chance = 0;
                GOBar.gameObject.SetActive(false);

                StaminaScript.StaminaCalculation(); //���������� ������� ����� ������
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Ball" && !ballInHands) {  //������ ����
            ballInHands = true;
            rigidBodyBall.isKinematic = true;
        }

        if (other.gameObject.tag == "DeadZones") { //������� ���� ��� �������
            ban2 = true;
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
        }

        if (other.gameObject.tag == "DeadZones") {
            ban2 = false;
        }

        if (other.gameObject.tag == "Limits") { //����� �� ������� ��������
            player.position = teleportPosition.position;
        }
    }

    public void ButtonUp() {
        buttonUpB = true;
    }

    public void ButtonDown() {
        buttonDownB = true;
    }
}