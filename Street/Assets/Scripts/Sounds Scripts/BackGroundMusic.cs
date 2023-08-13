using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{
    [SerializeField] private AudioClip[] Musics;
    [SerializeField] private AudioSource MusicAS;

    [SerializeField] private int Repeats;

    private int NumberRepeats = 0;
    private int AudioNumber = 0;

    private void Start() {
        MusicAS.PlayOneShot(Musics[AudioNumber], 0.3f);
    }

    private void Update() {
        if (!MusicAS.isPlaying) {
            MusicAS.PlayOneShot(Musics[AudioNumber], 0.3f);
            NumberRepeats++;

            if (NumberRepeats == Repeats - 1) {
                NumberRepeats = 0;
                AudioNumber++;

                if(AudioNumber > Musics.Length) AudioNumber = 0;
            }
        }
    }
}