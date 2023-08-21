using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour {
    [SerializeField] private string[] messagesRU;
    [SerializeField] private string[] messagesEN;
    [SerializeField] private float speedText;
    [SerializeField] private Text dialogueText;
    private int index;

    [SerializeField] private CoachTimelinePlayer coachTimelinePlayerScript;
    [SerializeField] private UIManager UIManagerScript;

    public void StartDialogue() {
        dialogueText.text = string.Empty;
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine() {
        if(UIManagerScript.GetLanguage() == true) {
            foreach (char c in messagesRU[index].ToCharArray()) {
                dialogueText.text += c;
                yield return new WaitForSeconds(speedText);
            }
        }
        else {
            foreach (char c in messagesEN[index].ToCharArray()) {
                dialogueText.text += c;
                yield return new WaitForSeconds(speedText);
            }
        }
    }

    public void ScipTextClick() {
        if (UIManagerScript.GetLanguage() == true) {
            if (dialogueText.text == messagesRU[index]) {
                NextLines();
            }
            else {
                StopAllCoroutines();
                dialogueText.text = messagesRU[index];
            }
        }
        else {
            if (dialogueText.text == messagesEN[index]) {
                NextLines();
            }
            else {
                StopAllCoroutines();
                dialogueText.text = messagesEN[index];
            }
        }
    }

    private void NextLines() {
        if (UIManagerScript.GetLanguage() == true) {
            if (index < messagesRU.Length - 1) {
                index++;
                dialogueText.text = string.Empty;
                StartCoroutine(TypeLine());
            }
            else {
                gameObject.SetActive(false);
                coachTimelinePlayerScript.TurningOffCoachCutscene();
            }
        }
        else {
            if (index < messagesEN.Length - 1) {
                index++;
                dialogueText.text = string.Empty;
                StartCoroutine(TypeLine());
            }
            else {
                gameObject.SetActive(false);
                coachTimelinePlayerScript.TurningOffCoachCutscene();
            }
        }
    }
}