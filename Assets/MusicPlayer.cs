using UnityEngine;
using UnityEngine.Audio;
public class MusicPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioResource[] MusikTracks;
    AudioSource player;

    public static MusicPlayer Instance;
    void Start()
    {
        Instance = this;
        player = GetComponent<AudioSource>();
        player.resource = MusikTracks[0];
        player.Play();
    }

    // Update is called once per frame
    public void switchToMusic(int index) {
        if(index < MusikTracks.Length) {
            player.resource = MusikTracks[index];
        }
    }
}
