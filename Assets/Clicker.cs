using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Clicker : MonoBehaviour
{

public static Clicker Instance;
   Camera m_Camera;
   public GameObject Bubblecontainer;

   public GameObject Sprayer;

   public static int mode = 0;

    bool sprayerCooldown = false;

    bool bigShotCooldown = false;
    
    float bigShotRange = 4.0f;
    float sprayerRange = 1.0f;

    bool sprayerAvailable = false;

    bool bearAvailable = false;

    public GameObject sprayAvailableIndicator;

    public GameObject bearAvailableIndicator;
    
    public Waffe waffe = Waffe.Normal;

    public enum Waffe {
        Normal,
        Bear,
        Snake,
    }

    void Start(
        Instance = this;
    )

   void Update()
   {

    if(Input.GetKey("e") && sprayerAvailable) {


    }

    if(Input.GetKey("r") && bearAvailable) {

        
    }



    Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    if(waffe = Waffe.Snake) {
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

    if(waffe = Waffe.Bear) {
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



    public void getUpgradeAvailable(Waffe waffe) {
        SpriteRenderer availabilitySprite
        if(waffe = Waffe.Bear) {
            bearAvailable = true;
        }
        if(waffe = Waffe.Snake) {
            sprayerAvailable = true;
        }
    }
    IEnumerator setSprayerCooldown() {
    yield return new WaitForSeconds(0.1f);
    sprayerCooldown = false;
    }

    IEnumerator setBigShotCooldown() {
        yield return new WaitForSeconds(1.0f);
        bigShotCooldown = false;

    }
    IEnumerator bearDuration() {
        yield return new WaitForSeconds(5.0f);
        waffe = Waffe.Normal;
    } 

    IEnumerator sprayDuration() {
        yield return new WaitForSeconds(5.0f);
        waffe = Waffe.Normal;
    }  
   
}


