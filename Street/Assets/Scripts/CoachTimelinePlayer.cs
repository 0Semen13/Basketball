using UnityEngine;
using UnityEngine.Playables;

public class CoachTimelinePlayer : MonoBehaviour
{
    private PlayableDirector director_CoachCutscene;

    [SerializeField] GameObject MainCamera;
    [SerializeField] GameObject Camera2;
    [SerializeField] GameObject Coach;
    [SerializeField] Canvas canvasControlPhone;
    [SerializeField] Canvas canvasOutput;
    [SerializeField] Canvas canvasDialog;

    private void Awake() {
        director_CoachCutscene = GetComponent<PlayableDirector>();
    }

    public void PlayCoachTimeLine() {
        director_CoachCutscene.Play();
    }

    public void TurningOffCoachCutscene() {
        MainCamera.SetActive(true);
        Camera2.SetActive(false);
        Coach.SetActive(false);

        canvasControlPhone.enabled = true;
        canvasOutput.enabled = true;
        canvasDialog.enabled = false;

        PlayerPrefs.SetInt("Coach", 1);
    }
}