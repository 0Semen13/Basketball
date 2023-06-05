using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Player : MonoBehaviour {
    [SerializeField] private bool developerMode;
    [SerializeField] private bool isPC;
    [SerializeField] private bool isPhone;

    [SerializeField] private float speed; //��������

    [SerializeField] Transform player; //�����
    [SerializeField] Transform ball; //���
    [SerializeField] Transform hands; //����
    [SerializeField] Transform rightHand; //������� ����
    [SerializeField] Transform posOverHead; //������� ��� ��� �������
    [SerializeField] Transform posDribble; //������� ��� �����
    [SerializeField] Transform target; //����
    [SerializeField] Transform miss1; //�������
    [SerializeField] Transform miss2;
    [SerializeField] Transform miss3;
    [SerializeField] Transform miss4;
    [SerializeField] Transform miss5;
    [SerializeField] Transform miss6;
    [SerializeField] Transform teleportPosition; //������� ���������

    private bool ballInHands = false; //��� � �����
    private bool ballFlying = false; //��� ����� (�� � �����)
    private bool ballStart = true; //��� ��� ������
    private float t0 = 0;

    [SerializeField] private double Percentage2Point = 50;
    //[SerializeField] private double Percentage3Point = 40;
    //[SerializeField] private double PercentageExtraLong = 30;
    private bool TwoPoint = false;
    private bool ThreePoint = false;
    private bool SuperPoint = true;

    int num = 1;

    void Update() {
        if (isPC) {
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            transform.position += direction * speed * Time.deltaTime; //�������� ������
            transform.LookAt(transform.position + direction); //������� ������

            if (ballInHands) {
                ballStart = false;

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
                    num = Random.Range(1, 100);
                }
            }
            if (ballFlying && !ballStart) {

                if(SuperPoint && ThreePoint && TwoPoint) {
                    Debug.Log("2 ����!");
                }

                if(SuperPoint && ThreePoint && !TwoPoint) {
                    Debug.Log("3 ����!");
                }

                if(SuperPoint && !ThreePoint && !TwoPoint) {
                    Debug.Log("����� �������!");
                }

                if (num < Percentage2Point) { //����� � ������� ���������
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
                        Debug.Log("����� � ����! ������ ����� " + num + ", ������� ������ � ���������� " + Percentage2Point);
                    }
                }
                else {
                    int num2 = Random.Range(1, 6);

                    t0 += Time.deltaTime;
                    float duration = 0.5f; //������������
                    float time = t0 / duration; //����� ������

                    Vector3 A = posOverHead.position;
                    Vector3 B = miss1.position;
                    Vector3 posFly = Vector3.Lerp(A, B, time); //��������� ������� (�����)
                    Vector3 arc = Vector3.up * 5 * Mathf.Sin(time * 3.14f);
                    ball.position = posFly + arc;

                    ball.Rotate(Random.Range(-0.85f, -0.4f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f)); //�������� ����

                    if (time >= 1) {
                        ballFlying = false;
                        ball.GetComponent<Rigidbody>().isKinematic = false;
                        Debug.Log("������! ������ ����� " + num + ", ������� �� ������ � ���������� " + Percentage2Point);
                    }
                }
            }

            if (developerMode) {                    //��� ������������
                if (Input.GetKey(KeyCode.F1)) { //������ ���� �� ������ �����
                    ballInHands = true;
                    ballFlying = false;
                    ball.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Ball" && !ballInHands && !ballFlying) {  //������ ����
            ballInHands = true;
            ball.GetComponent<Rigidbody>().isKinematic = true;
        }

        if (other.gameObject.tag == "2 Point") {
            TwoPoint = true;
            Debug.Log("2 ���!");
        }

        if (other.gameObject.tag == "3 Point") {
            ThreePoint = true;
            Debug.Log("3 ���!");
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "2 Point") {
            TwoPoint = false;
            Debug.Log("2 ���!");
        }

        if (other.gameObject.tag == "3 Point") {
            ThreePoint = false;
            Debug.Log("3 ���!");
        }
    }
}