using UnityEngine;
using System.Collections;

public class ModeSwitcher : MonoBehaviour
{
    
    int childIndex = 0;
    int numberOfModes;
    bool switchCDv = false;
    public int[] timeIntervals = {5, 5, 5, 5};

    public Material brodelMat;

    public GameObject backgrounds;
    public GameObject Chili;
    public GameObject Eis;


    public LehrlingAnimation hex;

    public Clicker clicker;

    public MusicPlayer musicPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        brodelMat.SetFloat("_HueShift", 0.0f);
        numberOfModes = transform.childCount;
        transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(automaticSwitch());
    }

    IEnumerator automaticSwitch() {
        while(true) {
            yield return new WaitForSeconds(timeIntervals[childIndex] - 3.5f);
            
            int randomLevel = (int) Random.Range(1, timeIntervals.Length);
            childIndex = childIndex != 0 ? 0 : randomLevel;
            if(childIndex == 1) {
                hex.Fire();
                Chili.GetComponent<throwItem>().Fire();

            }
            if(childIndex == 2) {
                hex.Fire();
                Eis.GetComponent<throwItem>().Fire();
            }

            yield return new WaitForSeconds(3.5f);
            
            switchMode(childIndex);
        }
    }

    IEnumerator switchCD() {
        switchCDv = true;
        yield return new WaitForSeconds(1);
        switchCDv = false; 
    }

    void switchMode(int i) {
        musicPlayer.switchToMusic(i);
        switch(i) {
            case 0: brodelMat.SetFloat("_HueShift", 0.0f); break;
            case 1: brodelMat.SetFloat("_HueShift", 240.0f); break;
            case 2: brodelMat.SetFloat("_HueShift", 80.0f); break;
        }
        for(int j = 0; j < numberOfModes; j++) {
            transform.GetChild(j).gameObject.SetActive(childIndex == j);
            backgrounds.transform.GetChild(j).gameObject.SetActive(childIndex == j);

            if(j == i) {
                clicker.Bubblecontainer = transform.GetChild(j).gameObject;
            }
        }
    }


}
