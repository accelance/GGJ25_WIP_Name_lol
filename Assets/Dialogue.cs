using UnityEngine;
using System.Collections;
using UnityEngine.Audio;


public class Dialogue : MonoBehaviour
{
    public AudioResource[] VoiceLines;
    public GameObject speechBubble1;
    public GameObject speechBubble2;

    public GameObject Cauldron;

    public AudioSource player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cauldron.SetActive(false);
        speechBubble1.SetActive(false);
        speechBubble2.SetActive(false);

        StartCoroutine(playDialogue());
    }

    IEnumerator playDialogue() {

        player.resource = VoiceLines[0];
        player.Play();
        speechBubble1.SetActive(true);
        yield return new WaitForSeconds(4.0f);
        speechBubble1.SetActive(false);

        speechBubble2.SetActive(true);
        player.resource = VoiceLines[1];
        player.Play();
        yield return new WaitForSeconds(2.0f);
        speechBubble2.SetActive(false);

        speechBubble1.SetActive(true);        
        player.resource = VoiceLines[2];
        player.Play();
        yield return new WaitForSeconds(4.0f);
        speechBubble1.SetActive(false);


        speechBubble2.SetActive(true);
        player.resource = VoiceLines[3];
        player.Play();
        yield return new WaitForSeconds(4.0f);
        speechBubble2.SetActive(false);

        speechBubble1.SetActive(true); 
        player.resource = VoiceLines[4];
        player.Play();
        yield return new WaitForSeconds(2.0f);
        speechBubble1.SetActive(false); 

        speechBubble2.SetActive(true);
        player.resource = VoiceLines[5];
        player.Play();
        yield return new WaitForSeconds(2.0f);
        speechBubble2.SetActive(false);

        Cauldron.SetActive(true);

    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
