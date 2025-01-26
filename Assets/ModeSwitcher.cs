using UnityEngine;
using System.Collections;

public class ModeSwitcher : MonoBehaviour
{
    
    int childIndex = 0;
    int numberOfModes;
    bool switchCDv = false;
    public int[] timeIntervals = {5, 5, 5, 5};

    public Clicker clicker;

    public MusicPlayer musicPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        numberOfModes = transform.childCount;
        transform.GetChild(0).gameObject.SetActive(true);
        StartCoroutine(automaticSwitch());
    }
    /*void Update() {
        if(Input.GetKey("g") && !switchCDv)  {
            switchMode(childIndex + 1);
            StartCoroutine(switchCD());
        }
    }*/

    IEnumerator automaticSwitch() {
        while(true) {
            yield return new WaitForSeconds(timeIntervals[childIndex]);
            int randomLevel = (int) Random.Range(1, 3);
            childIndex = childIndex != 0 ? 0 : randomLevel;
            Debug.Log("switched to mode: " + randomLevel);
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
        for(int j = 0; j < numberOfModes; j++) {
            transform.GetChild(j).gameObject.SetActive(childIndex == j);
            if(j == i) {
                clicker.Bubblecontainer = transform.GetChild(j).gameObject;
            }
        }
    }


}
