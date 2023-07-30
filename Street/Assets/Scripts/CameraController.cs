using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraT;
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;
    private Vector3 newCamPosition;

    [SerializeField] private float camPositionSpeed;
    [SerializeField] private float maxZPosCam;
    [SerializeField] private float minZPosCam;
    [SerializeField] private float maxXPosCam;
    [SerializeField] private float minXPosCam;

    [SerializeField] private float amount;
    [SerializeField] private float speed;
    private Vector3 rotation = new Vector3(34.5f, 0, 0);

    private void FixedUpdate() {
        rotation.z = Mathf.Sin(Time.time * speed) * amount;
        rotation.y = Mathf.Cos(Time.time * speed) * amount;
        cameraT.localEulerAngles = cameraT.rotation * rotation;

        if (player.position.x > minXPosCam && player.position.x < maxXPosCam) {
            if (player.position.z > minZPosCam && player.position.z < maxZPosCam) { //Если находится в квадрате
                newCamPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, player.position.z + offset.z);
            }
            else { //Если находится по Х, не по Z
                if (player.position.z < minZPosCam) {
                    newCamPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, minZPosCam + offset.z);
                }
                else if (player.position.z > maxZPosCam) {
                    newCamPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, maxZPosCam + offset.z);
                }
            }
        }
        else {
            if (player.position.z > minZPosCam && player.position.z < maxZPosCam) { //Если находится по Z, не по X
                if (player.position.x < minXPosCam) {
                    newCamPosition = new Vector3(minXPosCam + offset.x, player.position.y + offset.y, player.position.z + offset.z);
                }
                else if (player.position.x > maxXPosCam) {
                    newCamPosition = new Vector3(maxXPosCam + offset.x, player.position.y + offset.y, player.position.z + offset.z);
                }
            }
            else { //Если не находится в квадрате
                if (player.position.x < minXPosCam) {

                    if (player.position.z < minZPosCam) {
                        newCamPosition = new Vector3(minXPosCam + offset.x, player.position.y + offset.y, minZPosCam + offset.z);
                    }
                    else if (player.position.z > maxZPosCam) {
                        newCamPosition = new Vector3(minXPosCam + offset.x, player.position.y + offset.y, maxZPosCam + offset.z);
                    }
                }
                else if (player.position.x > maxXPosCam) {

                    if (player.position.z < minZPosCam) {
                        newCamPosition = new Vector3(maxXPosCam + offset.x, player.position.y + offset.y, minZPosCam + offset.z);
                    }
                    else if (player.position.z > maxZPosCam) {
                        newCamPosition = new Vector3(maxXPosCam + offset.x, player.position.y + offset.y, maxZPosCam + offset.z);
                    }
                }
            }
        }
        cameraT.position = Vector3.Lerp(cameraT.position, newCamPosition, camPositionSpeed * Time.deltaTime);
    }
}