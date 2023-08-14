using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{
    [SerializeField] private AudioClip[] Musics;
    [SerializeField] private AudioSource MusicAS;

    [Tooltip("Минимум 2 раза")]
    [SerializeField] private int Repeats;

    private int NumberRepeats = 1;
    private int AudioIndex;

    private void Start() {
        AudioIndex = Random.Range(0, 4);
        MusicAS.PlayOneShot(Musics[AudioIndex], 0.6f);
    }

    private void FixedUpdate() {
        if (!MusicAS.isPlaying) {
            MusicAS.PlayOneShot(Musics[AudioIndex], 0.6f);

            if (NumberRepeats + 1 == Repeats) {
                NumberRepeats = 0;

                if (AudioIndex + 1 > Musics.Length - 1) AudioIndex = 0;
                else AudioIndex++;
            }
            else {
                NumberRepeats++;
            }
        }
    }
}