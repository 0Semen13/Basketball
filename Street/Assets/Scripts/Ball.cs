using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    [Header("Объекты")]
    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bar;
    [SerializeField] private GameObject stamina;
    [SerializeField] private Transform target;
    [SerializeField] private Transform positionBall;
    [SerializeField] private Transform posOverHead;
    [SerializeField] private Transform posDribble;
    [SerializeField] private Transform[] misses;
    [SerializeField] private GameObject UIManager;

    [Header("Другие поля")]
    [SerializeField] private int startAddingPoints;
    [SerializeField] private int increasingAddingPoints;
    private int addingPoints;

    private int point;
    private int numberBalls;

    private bool ballInHands = false;
    private bool ballFlying = false;
    private bool ballStart = true;
    private bool banThrowOutsideSquare = false;
    private bool banThrowInDeadZone = false;

    private bool twoPoint = false;
    private bool threePoint = false;
    private bool isHit = false;
    private int hitSuperPoint = 0;
    private float t0 = 0;

    private bool buttonUpB = false;
    private bool buttonDownB = false;

    private float ballHeightFactor;
    private float ballTimeRatio;

    private int num = 0;
    private int pnt = 0;
    private int mss = 0;

    private Stamina staminaScript; //Объекты для кеширования
    private Player playerScript;
    private Bar barScript;
    private SaveAndLoad saveScript;
    private UIManager UIManagerScript;
    private Rigidbody rigidBodyBall;
    private Rigidbody rigidBodyPlayer;

    private void Awake() {
        rigidBodyBall = GetComponent<Rigidbody>();
        rigidBodyPlayer = player.GetComponent<Rigidbody>();

        staminaScript = stamina.GetComponent<Stamina>();
        playerScript = player.GetComponent<Player>();
        barScript = bar.GetComponent<Bar>();
        UIManagerScript = UIManager.GetComponent<UIManager>();
        saveScript = GameObject.Find("Save And Load").GetComponent<SaveAndLoad>();
    }

    private void Start() {
        ball.transform.position = positionBall.position;

        saveScript.LoadPointsAndBalls();
        point = saveScript.GetPoints();
        numberBalls = saveScript.GetBalls();
        UIManagerScript.SetTextPoints(point, numberBalls);

        if (PlayerPrefs.GetInt("firstStart") == 0) {
            addingPoints = startAddingPoints;
        }

        saveScript.LoadAddingPoints();
        addingPoints = saveScript.GetAddingPoint();

    }

    private void FixedUpdate() {
        if (ballInHands) {
            isHit = false;
            ballStart = false;
            pnt = 0;

            if (staminaScript.StaminaCheck() == 1) {
                if ((buttonDownB || Input.GetKey(KeyCode.Space)) && !banThrowOutsideSquare && !banThrowInDeadZone) {
                    ball.transform.position = posOverHead.position;
                }
                else {
                    ball.transform.position = posDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5) * 2);

                    buttonDownB = false;
                }

                if ((buttonUpB || Input.GetKeyUp(KeyCode.Space)) && !banThrowOutsideSquare && !banThrowInDeadZone && !ballFlying) {
                    rigidBodyPlayer.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

                    t0 = 0;
                    num = Random.Range(1, 101);
                    mss = Random.Range(1, 7);

                    staminaScript.StaminaReduction();

                    ballInHands = false;
                    ballFlying = true;
                    buttonDownB = false;
                    buttonUpB = false;
                }
                else {
                    buttonUpB = false;
                }
            }
            else {
                ball.transform.position = posDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5) * 2);
            }
        }
        else {
            buttonDownB = false;
            buttonUpB = false;
        }

        if (ballFlying && !ballStart) {
            ThrowFunction();
        }
    }

    private void ThrowFunction() {
        if (threePoint && twoPoint) {
            if (num <= (playerScript.GetCurrentPercentage(2) * barScript.GetChance()) / 100) {
                isHit = true;
                pnt = 2;
            }
            ballHeightFactor = 3.6f;
            ballTimeRatio = 0.45f;
        }
        else if (threePoint && !twoPoint) {
            if (num <= (playerScript.GetCurrentPercentage(3) * barScript.GetChance()) / 100) {
                isHit = true;
                pnt = 3;
            }
            ballHeightFactor = 4.8f;
            ballTimeRatio = 0.525f;
        }
        else if (!threePoint && !twoPoint) {
            if (num <= (playerScript.GetCurrentPercentage(4) * barScript.GetChance()) / 100) {
                isHit = true;
                pnt = 4;
            }
            else {
                hitSuperPoint = 0;
            }
            ballHeightFactor = 6.2f;
            ballTimeRatio = 0.61f;
        }

        if (isHit) {
            t0 += Time.deltaTime;
            float duration = ballTimeRatio; //Длительность
            float time = t0 / duration; //Время полета

            Vector3 A = posOverHead.position;
            Vector3 B = target.position;
            Vector3 posFly = Vector3.Lerp(A, B, time);
            Vector3 arc = Vector3.up * ballHeightFactor * Mathf.Sin(time * 3.14f);
            ball.transform.position = posFly + arc;

            ball.transform.Rotate(Random.Range(-1f, -0.55f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));

            if (time >= 1) {
                ballFlying = false;
                rigidBodyBall.isKinematic = false;

                if (pnt == 2) {
                    point += 2;
                }
                else if (pnt == 3) {
                    point += 3;
                }
                else if (pnt == 4) {
                    point += 3 + hitSuperPoint;
                    hitSuperPoint += 1;
                }

                playerScript.SetCurrentPercentage(pnt);

                numberBalls += 1;
                barScript.SetChance(0f);

                if (point >= addingPoints) {
                    addingPoints += increasingAddingPoints;
                    if (barScript.GetCurrentChance() < barScript.GetMaxChance()) {
                        barScript.SetCurrentChance(barScript.GetCurrentChance() + barScript.GetIncreasingBarSpeed());
                        saveScript.SaveBarSpeed();
                    }
                    saveScript.SaveAddingPoints();
                }

                UIManagerScript.SetTextPoints(point, numberBalls);

                saveScript.SaveCharacteristicsAndPoints();
                bar.gameObject.SetActive(false);
            }
        }
        else {
            t0 += Time.deltaTime;
            float duration = ballTimeRatio; //Длительность
            float time = t0 / duration; //Время полета

            Vector3 A = posOverHead.position;
            Vector3 B = misses[mss - 1].position;
            Vector3 posFly = Vector3.Lerp(A, B, time);
            Vector3 arc = Vector3.up * ballHeightFactor * Mathf.Sin(time * 3.14f);
            ball.transform.position = posFly + arc;

            ball.transform.Rotate(Random.Range(-0.85f, -0.4f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));

            if (time >= 1) {
                ballFlying = false;
                rigidBodyBall.isKinematic = false;
                barScript.SetChance(0f);
                bar.gameObject.SetActive(false);
            }
        }
    }

    public void ButtonUp() {
        buttonUpB = true;
    }
    public void ButtonDown() {
        buttonDownB = true;
    }
    public bool GetButtonDown() {
        return buttonDownB;
    }
    public int GetPointsBallsAddingPoints(int N) {
        if (N == 1) {
            return point;
        }
        else if (N == 2) {
            return numberBalls;
        }
        else if (N == 3) {
            return addingPoints;
        }
        else { return 0; }
    }
    public bool GetBallPosition(int N) {
        if(N == 1) {
            return ballInHands;
        }
        else if (N == 2) {
            return ballFlying;
        }
        else if (N == 3) {
            return ballStart;
        }
        else { return false; }
    }
    public void SetBallPosition(int N, bool value) {
        if (N == 1) {
            ballInHands = value;
        }
        else if (N == 2) {
            ballFlying = value;
        }
    }
    public void SetZone(int N, bool value) {
        if( N == 2) {
            twoPoint = value;
        }
        else if (N == 3) {
            threePoint = value;
        }
        else if (N == 5) {
            banThrowInDeadZone = value;
        }
        else if (N == 6) {
            banThrowOutsideSquare = value;
        }
    }
    public bool GetZone(int N) {
        if (N == 2) {
            return twoPoint;
        }
        else if (N == 3) {
            return threePoint;
        }
        else if (N == 5) {
            return banThrowInDeadZone;
        }
        else if (N == 6) {
            return banThrowOutsideSquare;
        }
        else { return false; }
    }
}