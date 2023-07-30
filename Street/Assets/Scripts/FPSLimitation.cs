using System.Collections;
using System.Threading;
using UnityEngine;

public class FPSLimitation : MonoBehaviour {
    [SerializeField] private int MaxRate = 9999;
    [SerializeField] private float TargetFrameRate = 120.0f;
    private float currentFrameTime;

    private void Awake() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = MaxRate;
        currentFrameTime = Time.realtimeSinceStartup;
        StartCoroutine(WaitForNextFrame());
    }

    IEnumerator WaitForNextFrame() {
        while (true) {
            yield return new WaitForEndOfFrame();
            currentFrameTime += 1.0f / TargetFrameRate;
            var t = Time.realtimeSinceStartup;
            var sleepTime = currentFrameTime - t - 0.01f;
            if (sleepTime > 0)
                Thread.Sleep((int)(sleepTime * 1000));
            while (t < currentFrameTime)
                t = Time.realtimeSinceStartup;
        }
    }
}