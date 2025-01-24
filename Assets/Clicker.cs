using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Clicker : MonoBehaviour
{
   Camera m_Camera;
   public GameObject Bubblecontainer;

   public GameObject Sprayer;

   public static int mode = 0;

    bool modeSwitchCoolDown = false;

    bool sprayerCooldown = false;

    bool bigShotCooldown = false;
    
    float bigShotRange = 4.0f;
    float sprayerRange = 1.0f;

   void Update()
   {
    if (Input.GetKey("e") && !modeSwitchCoolDown) {
        mode = (mode + 1) % 3;
        modeSwitchCoolDown = true;
        StartCoroutine(switchCooldown());
    }
    IEnumerator switchCooldown() {
        yield return new WaitForSeconds(1.0f);
        modeSwitchCoolDown = false;

    }

    IEnumerator setSprayerCooldown() {
        yield return new WaitForSeconds(0.1f);
        sprayerCooldown = false;

    }

    IEnumerator setBigShotCooldown() {
        yield return new WaitForSeconds(1.0f);
        bigShotCooldown = false;

    }
    
    Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    if(mode == 1) {
        Sprayer.transform.position = new Vector3(worldMousePosition.x, worldMousePosition.y, 0.0f);
        Sprayer.transform.localScale = new Vector3(sprayerRange, sprayerRange, 1.0f);
        if(!sprayerCooldown) {
            sprayerCooldown = true;
            int childCount = Bubblecontainer.transform.childCount;
            for(int i = 0; i < childCount; i++) {
                Transform bubble = Bubblecontainer.transform.GetChild(i);
                Vector3 bubblePosition = bubble.position;
                float Distance2D = (new Vector2(bubblePosition.x, bubblePosition.y) - new Vector2(worldMousePosition.x, worldMousePosition.y)).magnitude;
                if(Distance2D <= sprayerRange) {
                    bubble.gameObject.GetComponent<BubbleScript>().onHit();
                }
            }
            StartCoroutine(setSprayerCooldown());

        } 
    }

    if(mode == 2) {
            int childCount = Bubblecontainer.transform.childCount;
            Sprayer.transform.position = new Vector3(worldMousePosition.x, worldMousePosition.y, 0.0f);
            Sprayer.transform.localScale = new Vector3(bigShotRange, bigShotRange, 1.0f);
            if(Mouse.current.leftButton.wasPressedThisFrame && !bigShotCooldown) {
                bigShotCooldown = true;
                for(int i = 0; i < childCount; i++) {
                Transform bubble = Bubblecontainer.transform.GetChild(i);
                Vector3 bubblePosition = bubble.position;
                float Distance2D = (new Vector2(bubblePosition.x, bubblePosition.y) - new Vector2(worldMousePosition.x, worldMousePosition.y)).magnitude;
                if(Distance2D <= bigShotRange) {
                    bubble.gameObject.GetComponent<BubbleScript>().onHit();
                }
            }
            StartCoroutine(setBigShotCooldown());
            }
            
        }
   

    }    
   
}


