using UnityEngine;
using System.Collections;

public class ModeSwitcher : MonoBehaviour
{
    
    int childIndex = 0;
    int numberOfModes;
    bool switchCDv = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        numberOfModes = transform.childCount;
        StartCoroutine(automaticSwitch());
    }
    void Update() {
        if(Input.GetKey("g") && !switchCDv)  {
            switchMode();
            StartCoroutine(switchCD());
        }
    }

    IEnumerator automaticSwitch() {
        while(true) {
            yield return new WaitForSeconds(60.0f);
            switchMode();
        }
    }

    IEnumerator switchCD() {
        switchCDv = true;
        yield return new WaitForSeconds(1);
        switchCDv = false; 
    }

    void switchMode() {
        childIndex = (childIndex + 1) % numberOfModes;
        for(int i = 0; i < numberOfModes; i++) {
            transform.GetChild(i).gameObject.SetActive(childIndex == i);
        }
    }


}
