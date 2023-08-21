using UnityEngine;
using UnityEngine.Playables;

public class StartTimelinePlayer : MonoBehaviour
{
    private PlayableDirector director_StartingCutscene;

    private bool startCutscene;

    [SerializeField] private GameObject canvasStart;
    [SerializeField] private Canvas canvasControlPhone;
    [SerializeField] private Canvas canvasOutput;

    private void Awake() {
        director_StartingCutscene = GetComponent<PlayableDirector>();
        startCutscene = true;

        canvasControlPhone.enabled = false;
        canvasOutput.enabled = false;
    }

    private void Update() {
        if ((Input.touchCount > 0 || Input.GetMouseButton(0)) && startCutscene) {
            PlayStartTimeLine();
            startCutscene = false;
            canvasStart.SetActive(false);
        }
    }

    private void PlayStartTimeLine() {
        director_StartingCutscene.Play();
    }

    public bool GetBoolStart() {
        return startCutscene;
    }
}