using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private bool developerMode;
    [SerializeField] private bool isPC;
    [SerializeField] private bool isPhone;

    [SerializeField] private float speed; //Скорость

    [SerializeField] Transform ball; //Мяч
    [SerializeField] Transform hands; //Руки
    [SerializeField] Transform rightHand; //Праввая рука
    [SerializeField] Transform posOverHead; //Позиция рук над головой
    [SerializeField] Transform posDribble; //Позиция рук внизу
    [SerializeField] Transform target; //Цель
    [SerializeField] Transform miss1; //Промахи
    [SerializeField] Transform miss2;
    [SerializeField] Transform miss3;
    [SerializeField] Transform miss4;
    [SerializeField] Transform miss5;
    [SerializeField] Transform miss6;

    private bool ballInHands = false; //Мяч в руках
    private bool ballFlying = false; //Мяч летит (Не в руках)
    private bool ballStart = true; //Мяч при старте
    private float t0 = 0;

    void Update() {
        if (isPC) {
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            transform.position += direction * speed * Time.deltaTime; //Движение игрока
            transform.LookAt(transform.position + direction); //Поворот игрока

            if (ballInHands) {
                ballStart = false;

                if (Input.GetKey(KeyCode.Space)) {
                    ball.position = posOverHead.position; //Поднятие рук и мяча при зажатом пробеле и мяче в руках
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
                }
            }

            if (ballFlying && !ballStart) {
                t0 += Time.deltaTime;
                float duration = 0.5f; //Длительность
                float time = t0 / duration; //Время полета

                Vector3 A = posOverHead.position;
                Vector3 B = target.position;
                Vector3 posFly = Vector3.Lerp(A, B, time); //Изменение позиции полета
                Vector3 arc = Vector3.up * 5 * Mathf.Sin(time * 3.14f);
                ball.position = posFly + arc;

                ball.Rotate(Random.Range(-0.85f, -0.4f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f)); //Вращение мяча

                if (time >= 1) {
                    ballFlying = false;
                    ball.GetComponent<Rigidbody>().isKinematic = false;
                }
            }

            if (developerMode) {                    //ДЛЯ РАЗРАБОТЧИКА
                if (Input.GetKey(KeyCode.F1)) { //Подбор мяча из любого места
                    ballInHands = true;
                    ballFlying = false;
                    ball.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Ball" && !ballInHands && !ballFlying) {  //Подбор мяча
            ballInHands = true;
            ball.GetComponent<Rigidbody>().isKinematic = true;
        }

        if (other.gameObject.tag == "Walls") { //Телепорт игрока при выходя за площадку

        }
    }
}
