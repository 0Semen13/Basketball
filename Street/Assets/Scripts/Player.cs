using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    [Header("Режимы игры")]
    [SerializeField] private bool developerMode;
    [SerializeField] private bool isPC;
    [SerializeField] private bool isPhone;

    [Header("Характристики игрока")]
    [SerializeField] private float speed; //Скорость
    [SerializeField] public double currentPercentage2Point; //Текущие значения процентов
    [SerializeField] public double currentPercentage3Points;
    [SerializeField] public double currentPercentageExtraLong;
    [SerializeField] private double startPercentage2Point = 70; //Стартовые значения процентов
    [SerializeField] private double startPercentage3Points = 60;
    [SerializeField] private double startPercentageExtraLong = 50;
    [SerializeField] private double increasingMainZone = 0.5; //Добавочные значения
    [SerializeField] private double increasingMiddleZone = 0.25;
    [SerializeField] private double increasingLastZone = 0.1;
    private float chanceForBar;
    public float chanceSpeedForSave;

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

    public int firstStart = 0;
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

    public bool buttonUpB = false; //Флаги, показывающие, нажата ли кнопка или нет
    public bool buttonDownB = false;

    private float ballHeightFactor; //Коэффициенты времени и высоты полета мяча
    private float ballTimeRatio;

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
    [SerializeField] private GameObject backBar;
    [SerializeField] private GameObject foreBar;

    private Image imgBackBar, imgForeBar;
    private SaveAndLoad SaveScript; //Объекты для кеширования
    private Bar BarScript;
    private Rigidbody rigidBody;
    private Rigidbody rigidBodyBall;

    private void Start() {
        rigidBody = player.GetComponent<Rigidbody>();
        rigidBodyBall = ball.GetComponent<Rigidbody>();
        imgBackBar = backBar.GetComponent<Image>();
        imgForeBar = foreBar.GetComponent<Image>();
        SaveScript = GameObject.Find("Save And Load").GetComponent<SaveAndLoad>(); //Получение скрипта для сохранения
        BarScript = GameObject.Find("Chance_Bar").GetComponent<Bar>();

        firstStart = PlayerPrefs.GetInt("firstStart");
        if(firstStart == 0) { //Первый заход в игру
            currentPercentage2Point = startPercentage2Point;
            currentPercentage3Points = startPercentage3Points;
            currentPercentageExtraLong = startPercentageExtraLong;
            addingPoints = 50;
            BarScript.chanceSpeed = 0.8f;
            chanceSpeedForSave = BarScript.chanceSpeed;
            firstStart = 1;

            SaveScript.SaveGame(); //Сохранение данных
        }

        SaveScript.LoadGame(); //Загрузка данных
        currentPercentage2Point = SaveScript.percentage2Point_S;
        currentPercentage3Points = SaveScript.percentage3Point_S;
        currentPercentageExtraLong = SaveScript.percentageExtraLong_S;
        addingPoints = SaveScript.addingPoints_S;
        BarScript.chanceSpeed = SaveScript.chanceSpeed_S;
        chanceSpeedForSave = SaveScript.chanceSpeed_S;
        point = SaveScript.point_S;
        numberBalls = SaveScript.numberBalls_S;
        imgBackBar.enabled = false;
        imgForeBar.enabled = false;
        player.position = teleportPosition.position;
        ball.position = positionBall.position;

        pointDisplay.text = point.ToString();
        ballDisplay.text = numberBalls.ToString();

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

        rigidBody.AddForce(direction * speed); //Движение игрока
        player.LookAt(player.position + direction); //Поворот игрока

        if (ballInHands) {
            isHit = false;
            ballStart = false;
            pnt = 0;

            if ((buttonDownB || Input.GetKey(KeyCode.Space)) && !ban && !ban2) {
                rigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                imgBackBar.enabled = true;
                imgForeBar.enabled = true;
                ball.position = posOverHead.position; //Поднятие рук и мяча при зажатом пробеле и мяче в руках
                rightHand.localEulerAngles = Vector3.left * 0;
                hands.localEulerAngles = Vector3.right * 180;

                player.LookAt(target.position);
                player.eulerAngles = new Vector3(0, player.eulerAngles.y, 0);
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
                num = Random.Range(1, 101); //Определяет число, для сравнения с вероятностью
                mss = Random.Range(1, 7); //Определяет, в какой промах попадет

                chanceForBar = BarScript.chance;
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
            BarScript.chance = chanceForBar;
            ThrowFunction();
        }

        if (developerMode) {                    //ДЛЯ РАЗРАБОТЧИКА
            if (Input.GetKey(KeyCode.F1)) { //Подбор мяча из любого места
                ballInHands = true;
                ballFlying = false;
                rigidBodyBall.isKinematic = true;
            }

            if (Input.GetKey(KeyCode.F2)) { //Обнуление всех данных
                SaveScript.ResetData();
                point = 0;
                numberBalls = 0;
            }

            if (Input.GetKey(KeyCode.F3)) { //Обнуление характеристик
                currentPercentage2Point = startPercentage2Point;
                currentPercentage3Points = startPercentage3Points;
                currentPercentageExtraLong = startPercentageExtraLong;
                SaveScript.SaveGame();
            }
        }
    }

    private void ThrowFunction() {
        if (superPoint && threePoint && twoPoint) { //При броске находится в средней зоне
            if (num <= (currentPercentage2Point * chanceForBar) / 100) {
                isHit = true;
                pnt = 2;
            }
            ballHeightFactor = 3.6f;
            ballTimeRatio = 0.41f;
        }

        if (superPoint && threePoint && !twoPoint) { //При броске находится в дальней зоне
            if (num <= (currentPercentage3Points * chanceForBar) / 100) {
                isHit = true;
                pnt = 3;
            }
            ballHeightFactor = 4.8f;
            ballTimeRatio = 0.46f;
        }

        if (superPoint && !threePoint && !twoPoint) { //При броске находится в сверх дальней зоне
            if (num <= (currentPercentageExtraLong * chanceForBar) / 100) {
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

                if(point >= addingPoints) {
                    addingPoints += 50;
                    if(BarScript.chanceSpeed <= BarScript.maxChanceSpeed) {
                        BarScript.chanceSpeed += 0; //point / 500000f;
                        chanceSpeedForSave = BarScript.chanceSpeed;
                    }
                }
                imgBackBar.enabled = false;
                imgForeBar.enabled = false;

                pointDisplay.text = point.ToString();
                ballDisplay.text = numberBalls.ToString();

                SaveScript.SaveGame(); //Сохранение данных
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
                imgBackBar.enabled = false;
                imgForeBar.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Ball" && !ballInHands) {  //Подбор мяча
            ballInHands = true;
            rigidBodyBall.isKinematic = true;
        }

        if (other.gameObject.tag == "Limits") { //Выход за пределы площадки
            player.position = teleportPosition.position;
        }

        if (other.gameObject.tag == "DeadZones") { //Мертвая зона под кольцом
            ban2 = true;

            BarScript.chance = 0;
            imgBackBar.enabled = false;
            imgForeBar.enabled = false;
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

            BarScript.chance = 0;
            imgBackBar.enabled = false;
            imgForeBar.enabled = false;
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