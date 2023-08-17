using UnityEngine;
using UnityEngine.Playables;

public class TimelinePlayer : MonoBehaviour
{
    private PlayableDirector director_StartCutscene;
    private bool start;

    [SerializeField] private GameObject CanvasStart;

    [SerializeField] private Canvas ControlPhone;
    [SerializeField] private Canvas Output;

    private void Awake() {
        ControlPhone.enabled = false;
        Output.enabled = false;

        director_StartCutscene = GetComponent<PlayableDirector>();
        start = true;
    }

    private void Update() {
        if ((Input.touchCount > 0 || Input.GetMouseButton(0)) && start) {
            PlayStartTimeLine();
            start = false;
            CanvasStart.SetActive(false);
        }
    }

    public void PlayStartTimeLine() {
        director_StartCutscene.Play();
    }

    public bool GetBoolStart() {
        return start;
    }
}