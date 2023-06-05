using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private bool developerMode;
    [SerializeField] private bool isPC;
    [SerializeField] private bool isPhone;

    [SerializeField] private float speed; //��������

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

    private bool ballInHands = false; //��� � �����
    private bool ballFlying = false; //��� ����� (�� � �����)
    private bool ballStart = true; //��� ��� ������
    private float t0 = 0;

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
                }
            }

            if (ballFlying && !ballStart) {
                t0 += Time.deltaTime;
                float duration = 0.5f; //������������
                float time = t0 / duration; //����� ������

                Vector3 A = posOverHead.position;
                Vector3 B = target.position;
                Vector3 posFly = Vector3.Lerp(A, B, time); //��������� ������� ������
                Vector3 arc = Vector3.up * 5 * Mathf.Sin(time * 3.14f);
                ball.position = posFly + arc;

                ball.Rotate(Random.Range(-0.85f, -0.4f), Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f)); //�������� ����

                if (time >= 1) {
                    ballFlying = false;
                    ball.GetComponent<Rigidbody>().isKinematic = false;
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

        if (other.gameObject.tag == "Walls") { //�������� ������ ��� ������ �� ��������

        }
    }
}
