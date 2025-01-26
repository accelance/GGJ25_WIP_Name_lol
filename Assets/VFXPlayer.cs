using UnityEngine;
using UnityEngine.Audio;
public class VFXPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioResource[] VXF;
    AudioSource player;
    AudioSource player2;

    public static VFXPlayer Instance;
    void Start()
    {
        Instance = this;
        player = GetComponents<AudioSource>()[0];
        player2 = GetComponents<AudioSource>()[1];

        
    }

    // Update is called once per frame
    public void playVFX(int index) {
        if(index < VXF.Length) {
            player.resource = VXF[index];
            player.Play();
        }
    }

    public void playVFXpassive(int index) {
        if(index < VXF.Length) {
            player2.resource = VXF[index];
            player2.Play();
        }
    }
}
