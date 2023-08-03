using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    [Header("������ ����")]
    [SerializeField] private bool DEVELOP;

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
    [SerializeField] private Transform[] misses; //������ ����� �������
    [SerializeField] private Transform teleportPosition; //������� ���������
    [SerializeField] private Transform positionBall;
    [SerializeField] private GameObject FPSLimitation;
    [SerializeField] private GameObject model;

    [Header("�������� UI")]
    [SerializeField] private Text pointDisplay;
    [SerializeField] private Text ballDisplay;
    [SerializeField] private Joystick joystick;
    [SerializeField] private GameObject canvasControlPhone;
    [SerializeField] private GameObject bar;
    [SerializeField] private GameObject stamina;
    [SerializeField] private GameObject trainingPanel1;

    [Header("������ ����")]
    [SerializeField] private int startAddingPoints;
    [SerializeField] private int increasingAddingPoints;

    private int firstStart = 0;
    private bool ballInHands = false; //��� � �����
    private bool ballFlying = false; //��� ����� (�� � �����)
    private bool ballStart = true; //��� ��� ������
    private bool banThrowOutsideSquare = false; //������ �� ������ �� ��������� ��������
    private bool banThrowInDeadZone = false; //������ �� ������ �� ������� ����
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

    private SaveAndLoad saveScript; //������� ��� �����������
    private Bar barScript;
    private Stamina staminaScript;
    private Rigidbody rigidBody;
    private Rigidbody rigidBodyBall;
    private Animator animator;

    private void Start() {
        rigidBody = player.GetComponent<Rigidbody>(); //��������� rigidbody
        rigidBodyBall = ball.GetComponent<Rigidbody>();
        saveScript = GameObject.Find("Save And Load").GetComponent<SaveAndLoad>(); //��������� ��������
        barScript = bar.GetComponent<Bar>();
        staminaScript = stamina.GetComponent<Stamina>();
        animator = model.GetComponent<Animator>(); //��������� ���������

        firstStart = PlayerPrefs.GetInt("firstStart");
        if (firstStart == 0) { //������ ����� � ����
            currentPercentage2Point = startPercentage2Point;
            currentPercentage3Points = startPercentage3Points;
            currentPercentageExtraLong = startPercentageExtraLong;
            addingPoints = startAddingPoints;
            barScript.currentChanceSpeed = barScript.startChanceSpeed;
            firstStart = 1;
            saveScript.SaveCharacteristicsAndPoints(); //���������� ������
            saveScript.SaveAddingPoints();
            saveScript.SaveBarSpeed();
            PlayerPrefs.SetInt("firstStart", firstStart);
            trainingPanel1.SetActive(true);
        }

        saveScript.LoadGame(); //�������� ������
        currentPercentage2Point = saveScript.percentage2Point_S;
        currentPercentage3Points = saveScript.percentage3Point_S;
        currentPercentageExtraLong = saveScript.percentageExtraLong_S;
        addingPoints = saveScript.addingPoints_S;
        barScript.currentChanceSpeed = saveScript.chanceSpeed_S;

        point = saveScript.point_S;
        numberBalls = saveScript.numberBalls_S;
        pointDisplay.text = "����: " + point.ToString();
        ballDisplay.text = "����: " + numberBalls.ToString();

        player.position = teleportPosition.position;
        ball.position = positionBall.position;

        bar.gameObject.SetActive(false);
        FPSLimitation.gameObject.SetActive(!DEVELOP);
    }

    private void FixedUpdate() {
        Vector3 direction = new Vector3(joystick.Horizontal + Input.GetAxis("Horizontal"), 0, joystick.Vertical + Input.GetAxis("Vertical"));

        if (direction == new Vector3(0, 0, 0)) animator.SetFloat("Move", 0);
        else {
            animator.SetFloat("Move", 1);
            rigidBody.AddForce(direction * speed); //�������� ������
            player.LookAt(player.position + direction); //������� ������
        }

        if (ballInHands) {
            if (staminaScript.StaminaCheck() == 1) {
                if ((buttonDownB || Input.GetKey(KeyCode.Space)) && !banThrowOutsideSquare && !banThrowInDeadZone) {
                    rigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                    bar.gameObject.SetActive(true);
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
            }
            else {
                ball.position = posDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5) * 2);
            }
        }
    }

    private void Update() {
        if (ballInHands) {
            speed = speedWithTheBalled;
            animator.SetBool("Ball", true);
            isHit = false;
            ballStart = false;
            pnt = 0;

            if (staminaScript.StaminaCheck() == 1) {
                if ((buttonUpB || Input.GetKeyUp(KeyCode.Space)) && !banThrowOutsideSquare && !banThrowInDeadZone && !ballFlying) {
                    rigidBody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

                    t0 = 0;
                    num = Random.Range(1, 101); //���������� �����, ��� ��������� � ������������
                    mss = Random.Range(1, 7); //����������, � ����� ������ �������

                    ballInHands = false;
                    ballFlying = true;
                    buttonDownB = false;
                    buttonUpB = false;

                    staminaScript.StaminaReduction(); //���������� �������
                }
                else {
                    buttonUpB = false;
                }
            }
            else {
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

        if (DEVELOP) {                      //��� ������������
            if (Input.GetKey(KeyCode.F1)) { //������ ���� �� ������ �����
                ballInHands = true;
                ballFlying = false;
                rigidBodyBall.isKinematic = true;
                animator.SetBool("Ball", true);
            }

            if (Input.GetKey(KeyCode.F2)) { //��������� ���� ������
                saveScript.ResetData();
                point = 0;
                numberBalls = 0;
                pointDisplay.text = "����: " + point.ToString();
                ballDisplay.text = "����: " + numberBalls.ToString();
            }
        }
    }

    private void ThrowFunction() {
        if (superPoint && threePoint && twoPoint) { //��� ������ ��������� � ������� ����
            if (num <= (currentPercentage2Point * barScript.chance) / 100) {
                isHit = true;
                pnt = 2;
            }
            ballHeightFactor = 3.6f;
            ballTimeRatio = 0.45f;
        }
        else if (superPoint && threePoint && !twoPoint) { //��� ������ ��������� � ������� ����
            if (num <= (currentPercentage3Points * barScript.chance) / 100) {
                isHit = true;
                pnt = 3;
            }
            ballHeightFactor = 4.8f;
            ballTimeRatio = 0.525f;
        }
        else if (superPoint && !threePoint && !twoPoint) { //��� ������ ��������� � ����� ������� ����
            if (num <= (currentPercentageExtraLong * barScript.chance) / 100) {
                isHit = true;
                pnt = 4;
            }
            else {
                hitSuperPoint = 0;
            }
            ballHeightFactor = 6.2f;
            ballTimeRatio = 0.61f;
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
                barScript.chance = 0;

                if (point >= addingPoints) {
                    addingPoints += increasingAddingPoints;
                    if (barScript.currentChanceSpeed < barScript.maxChanceSpeed) {
                        barScript.currentChanceSpeed += barScript.increasingBarSpeed;
                        saveScript.SaveBarSpeed();
                    }
                    saveScript.SaveAddingPoints();
                }

                pointDisplay.text = "����: " + point.ToString();
                ballDisplay.text = "����: " + numberBalls.ToString();

                saveScript.SaveCharacteristicsAndPoints();
                bar.gameObject.SetActive(false);
            }
        }
        else {
            t0 += Time.deltaTime;
            float duration = ballTimeRatio; //������������
            float time = t0 / duration; //����� ������

            Vector3 A = posOverHead.position;
            Vector3 B = misses[mss - 1].position;
            Vector3 posFly = Vector3.Lerp(A, B, time); //��������� ������� (�����)
            Vector3 arc = Vector3.up * ballHeightFactor * Mathf.Sin(time * 3.14f);
            ball.position = posFly + arc;

            ball.Rotate(Random.Range(-0.85f, -0.4f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f)); //�������� ����

            if (time >= 1) {
                ballFlying = false;
                rigidBodyBall.isKinematic = false;
                barScript.chance = 0;
                bar.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Ball" && !ballInHands) {  //������ ����
            ballInHands = true;
            rigidBodyBall.isKinematic = true;
        }

        if (other.gameObject.tag == "DeadZones") { //������� ���� ��� �������
            banThrowInDeadZone = true;
        }

        if (other.gameObject.tag == "2 Point") {
            twoPoint = true;
        }

        if (other.gameObject.tag == "3 Point") {
            threePoint = true;
        }

        if (other.gameObject.tag == "Extra long") {
            banThrowOutsideSquare = false;
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
            banThrowOutsideSquare = true;
        }

        if (other.gameObject.tag == "DeadZones") {
            banThrowInDeadZone = false;
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