using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    [Header("Режимы игры")]
    [SerializeField] private bool developerMode;
    [SerializeField] private bool isPhone;

    [Header("Характристики игрока")]
    [SerializeField] private float speedWithTheBalled; //Скорость с мячом
    [SerializeField] private float speedWithoutBall; //Скорость без мяча
    private float speed = 0;
    [SerializeField] private double startPercentage2Point; //Стартовые значения процентов
    [SerializeField] private double startPercentage3Points;
    [SerializeField] private double startPercentageExtraLong;
    [SerializeField] public double currentPercentage2Point; //Текущие значения процентов
    [SerializeField] public double currentPercentage3Points;
    [SerializeField] public double currentPercentageExtraLong;
    [SerializeField] private double increasingMainZone; //Добавочные значения к процентам за попадание
    [SerializeField] private double increasingMiddleZone;
    [SerializeField] private double increasingLastZone;

    [Header("Объекты")]
    [SerializeField] private Transform player; //Игрок
    [SerializeField] private Transform ball; //Мяч
    [SerializeField] private Transform hands; //Руки
    [SerializeField] private Transform rightHand; //Правая рука
    [SerializeField] private Transform posOverHead; //Позиция рук над головой
    [SerializeField] private Transform posDribble; //Позиция рук внизу
    [SerializeField] private Transform target; //Цель
    [SerializeField] private Transform[] Misses; //Массив точек промаха
    [SerializeField] private Transform teleportPosition; //Позиция телепорта
    [SerializeField] private Transform positionBall;
    [SerializeField] private GameObject FPSLimitation;
    [SerializeField] private GameObject model;

    private int firstStart = 0;
    private bool ballInHands = false; //Мяч в руках
    private bool ballFlying = false; //Мяч летит (Не в руках)
    private bool ballStart = true; //Мяч при старте
    private bool ban = false; //Запрет на бросок за пределами площадки
    private bool ban2 = false; //Запрет на бросок из мертвой зоны
    private float t0 = 0;

    private bool twoPoint = false; //Флаги показывающий, в какой зоне находится игрок
    private bool threePoint = false;
    private bool superPoint = true;
    private bool isHit = false;
    private int hitSuperPoint = 0;

    private bool buttonUpB = false; //Флаги, показывающие, нажата ли кнопка или нет
    private bool buttonDownB = false;

    private float ballHeightFactor; //Коэффициенты высоты полета мяча
    private float ballTimeRatio; //Коэффициенты времени полета мяча

    private int num = 0; //Число, сравниваемое с вероятностью
    private int pnt = 0; //Число, сколько очков должно добавиться после попадания
    private int mss = 0; //Число, означающее индекс промаха при промахе

    public int point; //Забитые очки
    public int numberBalls; //Забитые мячи
    public int addingPoints; //Очки, после которых ускоряется шкала вероятности

    [Header("Элементы UI")]
    [SerializeField] private Text pointDisplay;
    [SerializeField] private Text ballDisplay;
    [SerializeField] private Joystick joystick;
    [SerializeField] private GameObject canvasControlPhone;
    [SerializeField] private GameObject GOBar;

    private SaveAndLoad SaveScript; //Объекты для кеширования
    private Bar BarScript;
    private Stamina StaminaScript;
    private Rigidbody rigidBody;
    private Rigidbody rigidBodyBall;
    private Animator animator;

    private void Start() {
        rigidBody = player.GetComponent<Rigidbody>(); //Получение rigidbody
        rigidBodyBall = ball.GetComponent<Rigidbody>();
        SaveScript = GameObject.Find("Save And Load").GetComponent<SaveAndLoad>(); //Получение скриптов
        BarScript = GameObject.Find("Bar").GetComponent<Bar>();
        StaminaScript = GameObject.Find("Stamina").GetComponent<Stamina>();
        animator = model.GetComponent<Animator>(); //Получение аниматора

        firstStart = PlayerPrefs.GetInt("firstStart");
        if (firstStart == 0) { //Первый заход в игру
            currentPercentage2Point = startPercentage2Point;
            currentPercentage3Points = startPercentage3Points;
            currentPercentageExtraLong = startPercentageExtraLong;
            addingPoints = 50;
            BarScript.currentChanceSpeed = BarScript.startChanceSpeed  ;
            firstStart = 1;
            PlayerPrefs.SetInt("firstStart", firstStart);
            SaveScript.SaveGame(); //Сохранение данных
        }

        SaveScript.LoadGame(); //Загрузка данных
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

        rigidBody.AddForce(direction * speed); //Движение игрока
        player.LookAt(player.position + direction); //Поворот игрока

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
                    num = Random.Range(1, 101); //Определяет число, для сравнения с вероятностью
                    mss = Random.Range(1, 7); //Определяет, в какой промах попадет

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

        if (developerMode) {                    //ДЛЯ РАЗРАБОТЧИКА
            if (Input.GetKey(KeyCode.F1)) { //Подбор мяча из любого места
                ballInHands = true;
                ballFlying = false;
                rigidBodyBall.isKinematic = true;
                animator.SetBool("Ball", true);
            }

            if (Input.GetKey(KeyCode.F2)) { //Обнуление всех данных
                SaveScript.ResetData();
                point = 0;
                numberBalls = 0;
                pointDisplay.text = "Points: " + point.ToString();
                ballDisplay.text = "Balls: " + numberBalls.ToString();
            }
        }
    }

    private void ThrowFunction() {
        if (superPoint && threePoint && twoPoint) { //При броске находится в средней зоне
            if (num <= (currentPercentage2Point * BarScript.chance) / 100) {
                isHit = true;
                pnt = 2;
            }
            ballHeightFactor = 3.6f;
            ballTimeRatio = 0.41f;
        }

        if (superPoint && threePoint && !twoPoint) { //При броске находится в дальней зоне
            if (num <= (currentPercentage3Points * BarScript.chance) / 100) {
                isHit = true;
                pnt = 3;
            }
            ballHeightFactor = 4.8f;
            ballTimeRatio = 0.46f;
        }

        if (superPoint && !threePoint && !twoPoint) { //При броске находится в сверх дальней зоне
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

        if (isHit) { //Попал в кольцо
            t0 += Time.deltaTime;
            float duration = ballTimeRatio; //Длительность
            float time = t0 / duration; //Время полета

            Vector3 A = posOverHead.position;
            Vector3 B = target.position;
            Vector3 posFly = Vector3.Lerp(A, B, time); //Изменение позиции (полет)
            Vector3 arc = Vector3.up * ballHeightFactor * Mathf.Sin(time * 3.14f);
            ball.position = posFly + arc;

            ball.Rotate(Random.Range(-1f, -0.55f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f)); //Вращение мяча

            if (time >= 1) {
                ballFlying = false;
                rigidBodyBall.isKinematic = false;

                if (pnt == 2) { //Добавление очков
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

                StaminaScript.StaminaCalculation(); //Уменьшение стамины после броска
                SaveScript.SaveCharacteristicsAndPoints();
                GOBar.gameObject.SetActive(false);
            }
        }
        else {
            t0 += Time.deltaTime;
            float duration = ballTimeRatio; //Длительность
            float time = t0 / duration; //Время полета

            Vector3 A = posOverHead.position;
            Vector3 B = Misses[mss - 1].position;
            Vector3 posFly = Vector3.Lerp(A, B, time); //Изменение позиции (полет)
            Vector3 arc = Vector3.up * ballHeightFactor * Mathf.Sin(time * 3.14f);
            ball.position = posFly + arc;

            ball.Rotate(Random.Range(-0.85f, -0.4f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f)); //Вращение мяча

            if (time >= 1) {
                ballFlying = false;
                rigidBodyBall.isKinematic = false;
                BarScript.chance = 0;
                GOBar.gameObject.SetActive(false);

                StaminaScript.StaminaCalculation(); //Уменьшение стамины после броска
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Ball" && !ballInHands) {  //Подбор мяча
            ballInHands = true;
            rigidBodyBall.isKinematic = true;
        }

        if (other.gameObject.tag == "DeadZones") { //Мертвая зона под кольцом
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

        if (other.gameObject.tag == "Limits") { //Выход за пределы площадки
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