using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour {
    private float fps;
    [SerializeField] private Text fpsText;

    private void Start() {
        StartCoroutine(Fps());
    }

    private IEnumerator Fps() {
        while (true) {
            yield return new WaitForSeconds(0.3f);
            fps = 1.0f / Time.deltaTime;
            fpsText.text = "" + (int)fps;
        }
    }
}