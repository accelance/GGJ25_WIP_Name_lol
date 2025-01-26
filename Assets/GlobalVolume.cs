using UnityEngine;

public class GlobalVolume : MonoBehaviour
{
    public AudioSource[] audioSources;


    public void changeGlobalVol(float volume) {
        for (int i = 0; i < audioSources.Length; i++) {
            audioSources[i].volume	= volume;
        }
    }


}
