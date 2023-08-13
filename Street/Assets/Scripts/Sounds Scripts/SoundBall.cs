using UnityEngine;

public class SoundBall : MonoBehaviour
{
    [SerializeField] private AudioClip[] Kicks;
    [SerializeField] private AudioClip[] Hits;
    [SerializeField] private AudioClip Miss;

    [SerializeField] private AudioSource Sound;

    public void OnHitSound() {
        Sound.pitch = Random.Range(0.85f, 1.15f);
        Sound.PlayOneShot(Hits[Random.Range(0, Hits.Length - 1)], 1.1f);
    }

    public void OnKickSound() {
        Sound.pitch = Random.Range(0.85f, 1.15f);
        Sound.PlayOneShot(Kicks[Random.Range(0, Kicks.Length - 1)], 0.65f);
    }

    public void OnMissSound() {
        Sound.pitch = Random.Range(0.8f, 1.2f);
        Sound.PlayOneShot(Miss, 0.6f);
    }
}