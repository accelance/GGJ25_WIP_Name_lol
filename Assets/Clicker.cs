using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Clicker : MonoBehaviour
{

   public static Clicker Instance;
   Camera m_Camera;
   public GameObject Bubblecontainer;

   public GameObject Sprayer;
   SpriteRenderer sprayerRenderer;

   public static int mode = 0;

    bool sprayerCooldown = false;

    bool bigShotCooldown = false;
    
    float bigShotRange = 4.0f;
    float sprayerRange = 1.0f;

    bool sprayerAvailable = false;

    bool bearAvailable = false;

    public GameObject sprayAvailableIndicator;
    SpriteRenderer sprayAvailableIndicatorSprite;

    public GameObject bearAvailableIndicator;

    SpriteRenderer bearAvailableIndicatorSprite;
    
    public Waffe waffe = Waffe.Normal;

    public enum Waffe {
        Normal,
        Bear,
        Snake,
    }

    void Start() {
        Instance = this;
        waffe = Waffe.Normal;
        Debug.Log(waffe);
        sprayAvailableIndicatorSprite = sprayAvailableIndicator.GetComponent<SpriteRenderer>();
        sprayAvailableIndicatorSprite.enabled = false;
        bearAvailableIndicatorSprite = bearAvailableIndicator.GetComponent<SpriteRenderer>();
        bearAvailableIndicatorSprite.enabled = false;

        sprayerRenderer = Sprayer.GetComponent<SpriteRenderer>();

        sprayerRenderer.enabled = false;
    }

   void Update()
   {

    if(Input.GetKey("e") && sprayerAvailable) {
        waffe = Waffe.Snake;
        StartCoroutine(sprayDuration());
    }

    if(Input.GetKey("r") && bearAvailable) {
        waffe = Waffe.Bear;
        StartCoroutine(bearDuration());
    }



    Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    if(waffe == Waffe.Snake) {
        if(!sprayerCooldown) {
            sprayerCooldown = true;
            int childCount = Bubblecontainer.transform.childCount;
            for(int i = 0; i < childCount; i++) {
                Transform bubble = Bubblecontainer.transform.GetChild(i);
                Vector3 bubblePosition = bubble.position;
                float Distance2D = (new Vector2(bubblePosition.x, bubblePosition.y) - new Vector2(worldMousePosition.x, worldMousePosition.y)).magnitude;
                if(Distance2D <= sprayerRange && bubble.gameObject.activeSelf) {
                    bubble.gameObject.GetComponent<Bubbles>().onHit();
                }
            }
            StartCoroutine(setSprayerCooldown());

        } 
    }
    if(waffe == Waffe.Bear) {
        
            if(Mouse.current.leftButton.wasPressedThisFrame && !bigShotCooldown) {
                int childCount = Bubblecontainer.transform.childCount;
                Sprayer.transform.position = new Vector3(worldMousePosition.x, worldMousePosition.y, 0.0f);
                Sprayer.transform.localScale = new Vector3(bigShotRange * 2, bigShotRange * 2, 1.0f);
                sprayerRenderer.enabled = true;
                bigShotCooldown = true;
                for(int i = 0; i < childCount; i++) {
                    Transform bubble = Bubblecontainer.transform.GetChild(i);
                    Vector3 bubblePosition = bubble.position;
                    float Distance2D = (new Vector2(bubblePosition.x, bubblePosition.y) - new Vector2(worldMousePosition.x, worldMousePosition.y)).magnitude;
                    if(Distance2D <= bigShotRange && bubble.gameObject.activeSelf) {
                        bubble.gameObject.GetComponent<Bubbles>().onHit();
                    }
                }
                StartCoroutine(pawDuration());
                StartCoroutine(setBigShotCooldown());
            }
            
        }
    }  



    public void getUpgradeAvailable(Waffe waffe) {
        if(waffe == Waffe.Bear) {
            bearAvailable = true;
            bearAvailableIndicatorSprite.enabled = true;
        }
        if(waffe == Waffe.Snake) {
            sprayerAvailable = true;
            Debug.Log("snake unlockend");
            sprayAvailableIndicatorSprite.enabled = true;
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
        bearAvailableIndicatorSprite.enabled = false;
    } 

    IEnumerator sprayDuration() {
        yield return new WaitForSeconds(5.0f);
        waffe = Waffe.Normal;
        sprayAvailableIndicatorSprite.enabled = false;
    }

    IEnumerator pawDuration() {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("disable");
        sprayerRenderer.enabled = false;

    }  
   
}


