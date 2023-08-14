using UnityEngine;

public class Player : MonoBehaviour {
    [Header("Режимы игры")]
    [SerializeField] private bool DEVELOP;

    [Header("Характеристики игрока")]
    [SerializeField] private float speedWithTheBalled;
    [SerializeField] private float speedWithoutBall;
    private float speed = 0;
    [SerializeField] private double startPercentage2Point;
    [SerializeField] private double startPercentage3Points;
    [SerializeField] private double startPercentageExtraLong;
    [SerializeField] private double currentPercentage2Point;
    [SerializeField] private double currentPercentage3Points;
    [SerializeField] private double currentPercentageExtraLong;
    [SerializeField] private double increasingMainZone;
    [SerializeField] private double increasingMiddleZone;
    [SerializeField] private double increasingLastZone;

    [Header("Объекты")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject model;
    [SerializeField] private Transform target;
    [SerializeField] private Transform teleportPosition;
    [SerializeField] private GameObject FPSLimitation;
    [SerializeField] private GameObject UIManager;

    [Header("Элементы UI")]
    [SerializeField] private Joystick joystick;
    [SerializeField] private GameObject bar;
    [SerializeField] private GameObject stamina;

    private GameObject ball;  //Объекты для кеширования
    private SaveAndLoad saveScript;
    private Ball ballScript;
    private Bar barScript;
    private Stamina staminaScript;
    private UIManager UIManagerScript;
    private Rigidbody rigidBodyPlayer;
    private Rigidbody rigidBodyBall;
    private Animator animator;

    private void Awake() {
        ball = GameObject.Find("Ball");

        rigidBodyPlayer = player.GetComponent<Rigidbody>();
        rigidBodyBall = ball.GetComponent<Rigidbody>();
        saveScript = GameObject.Find("Save And Load").GetComponent<SaveAndLoad>();
        ballScript = ball.GetComponent<Ball>();
        barScript = bar.GetComponent<Bar>();
        staminaScript = stamina.GetComponent<Stamina>();
        UIManagerScript = UIManager.GetComponent<UIManager>();
        animator = model.GetComponent<Animator>();
    }

    private void Start() {
        if (PlayerPrefs.GetInt("firstStart") == 0) { //Первый заход в игру
            currentPercentage2Point = startPercentage2Point;
            currentPercentage3Points = startPercentage3Points;
            currentPercentageExtraLong = startPercentageExtraLong;
            saveScript.SaveCharacteristicsAndPoints();
            saveScript.SaveAddingPoints();
            barScript.SetCurrentChance(barScript.GetStartChance());
            saveScript.SaveBarSpeed();
        }

        saveScript.LoadCharacteristics(); //Загрузка данных
        currentPercentage2Point = saveScript.GetPercentage2Point();
        currentPercentage3Points = saveScript.GetPercentage3Point();
        currentPercentageExtraLong = saveScript.GetPercentageExtraLong();

        saveScript.LoadBarSpeed();
        barScript.SetCurrentChance(saveScript.GetChanceSpeed());

        player.position = teleportPosition.position;

        bar.SetActive(false);
        FPSLimitation.SetActive(!DEVELOP);
    }

    private void FixedUpdate() {
        Vector3 direction = new(joystick.Horizontal + Input.GetAxis("Horizontal"), 0, joystick.Vertical + Input.GetAxis("Vertical"));

        if (direction == new Vector3(0, 0, 0)) animator.SetFloat("Move", 0);
        else {
            animator.SetFloat("Move", 1);
            rigidBodyPlayer.AddForce(direction * speed); //Движение игрока
            player.LookAt(player.position + direction); //Поворот игрока
        }

        if (ballScript.GetBallPosition(1)) {
            speed = speedWithTheBalled;
            animator.SetBool("Ball", true);

            if (staminaScript.StaminaCheck() == 1) {
                if ((ballScript.GetButtonDown() || Input.GetKey(KeyCode.Space)) && !ballScript.GetZone(6) && !ballScript.GetZone(5)) {
                    rigidBodyPlayer.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

                    bar.SetActive(true);
                    animator.SetBool("Throw", true);

                    player.LookAt(target.position);
                    player.eulerAngles = new Vector3(0, player.eulerAngles.y, 0);
                }
            }
        }
        else {
            speed = speedWithoutBall;
        }
    }

    private void Update() {
        if (ballScript.GetBallPosition(2) && !ballScript.GetBallPosition(3)) {
            animator.SetBool("Throw", false);
            animator.SetBool("Ball", false);
        }

        if (DEVELOP) {                      //ДЛЯ РАЗРАБОТЧИКА
            if (Input.GetKey(KeyCode.F1)) { //Подбор мяча из любого места
                ballScript.SetBallPosition(1, true);
                ballScript.SetBallPosition(2, false);
                rigidBodyBall.isKinematic = true;
                animator.SetBool("Ball", true);
            }

            if (Input.GetKey(KeyCode.F2)) { //Обнуление всех данных
                saveScript.ResetData();
                ballScript.SetPointsBalls(0, 0);
                UIManagerScript.SetTextPoints(ballScript.GetPointsBallsAddingPoints(1), ballScript.GetPointsBallsAddingPoints(2));
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Ball") && !ballScript.GetBallPosition(1)) {
            ballScript.SetBallPosition(1, true);
            rigidBodyBall.isKinematic = true;
        }

        if (other.gameObject.CompareTag("DeadZones")) { //Мертвая зона под кольцом
            ballScript.SetZone(5, true);
        }

        if (other.gameObject.CompareTag("2 Point")) {
            ballScript.SetZone(2, true);
        }

        if (other.gameObject.CompareTag("3 Point")) {
            ballScript.SetZone(3, true);
        }

        if (other.gameObject.CompareTag("Extra long")) {
            ballScript.SetZone(6, false);
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("2 Point")) {
            ballScript.SetZone(2, false);
        }

        if (other.gameObject.CompareTag("3 Point")) {
            ballScript.SetZone(3, false);
        }

        if (other.gameObject.CompareTag("Extra long")) {
            ballScript.SetZone(6, true);
        }

        if (other.gameObject.CompareTag("DeadZones")) {
            ballScript.SetZone(5, false);
        }

        if (other.gameObject.CompareTag("Limits")) { //Выход за пределы площадки
            player.position = teleportPosition.position;
        }
    }
    public double GetCurrentPercentage(int zone) {
        if(zone == 2) {
            return currentPercentage2Point;
        }
        else if(zone == 3) {
            return currentPercentage3Points;
        }
        else if(zone == 4) {
            return currentPercentageExtraLong;
        }
        else {
            return 0;
        }
    }
    public void SetCurrentPercentage(int zone) {
        if (zone == 2) {
            currentPercentage2Point += increasingMainZone;
            currentPercentage3Points += increasingMiddleZone;
            currentPercentageExtraLong += increasingLastZone;
        }
        else if (zone == 3) {
            currentPercentage3Points += increasingMainZone;
            currentPercentageExtraLong += increasingMiddleZone;
        }
        else if (zone == 4) {
            currentPercentage3Points += increasingMiddleZone;
            currentPercentageExtraLong += increasingMainZone;
        }
    }
}