using UnityEngine;

public class SoundStep : MonoBehaviour
{
    [SerializeField] private AudioClip[] Steps; 
    [SerializeField] private AudioSource StepSound;

    public void OnStep() {
        StepSound.pitch = Random.Range(0.85f, 1.15f);
        StepSound.PlayOneShot(Steps[Random.Range(0, Steps.Length - 1)], 0.5f);
    }
}