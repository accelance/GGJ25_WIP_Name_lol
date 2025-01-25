using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Clicker : MonoBehaviour
{

    public static Clicker Instance;
    Camera m_Camera;
    public GameObject Bubblecontainer;

    public GameObject Lehrling;
    LehrlingAnimation lehrlingSkript;

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

    public GameObject sprayParticleSource;

    public Sprite[] PawAnimation;
    public GameObject Paw;
    SpriteRenderer PawRenderer;
    SpriteRenderer sprayAvailableIndicatorSprite;

    public GameObject bearAvailableIndicator;

    SpriteRenderer bearAvailableIndicatorSprite;

    public Waffe waffe = Waffe.Normal;

    bool fireAnimationCoolDown = false;

    public enum Waffe
    {
        Normal,
        Bear,
        Snake,
    }

    void Start()
    {
        Instance = this;
        waffe = Waffe.Normal;
        Debug.Log(waffe);
        sprayAvailableIndicatorSprite = sprayAvailableIndicator.GetComponent<SpriteRenderer>();
        sprayAvailableIndicatorSprite.enabled = false;
        bearAvailableIndicatorSprite = bearAvailableIndicator.GetComponent<SpriteRenderer>();
        bearAvailableIndicatorSprite.enabled = false;

        sprayerRenderer = Sprayer.GetComponent<SpriteRenderer>();

        sprayerRenderer.enabled = false;
        sprayParticleSource.SetActive(false);
        PawRenderer = Paw.GetComponent<SpriteRenderer>();
        lehrlingSkript = Lehrling.GetComponent<LehrlingAnimation>();
    }

    void Update()
    {

        if (Input.GetKey("e") /* && sprayerAvailable */)
        {
            waffe = Waffe.Snake;
            sprayParticleSource.SetActive(true);
            StartCoroutine(sprayDuration());
        }

        if (Input.GetKey("r") /* && bearAvailable */)
        {
            waffe = Waffe.Bear;
            StartCoroutine(bearDuration());
        }

        if(Mouse.current.leftButton.wasPressedThisFrame && (waffe == Waffe.Normal || waffe == Waffe.Bear) && !fireAnimationCoolDown) {
            lehrlingSkript.Fire();
            StartCoroutine(fireShotCooldown());
        }



        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (waffe == Waffe.Snake)
        {
            sprayParticleSource.transform.position = new Vector3(worldMousePosition.x, worldMousePosition.y, 0.0f);
            if (!sprayerCooldown)
            {
                sprayerCooldown = true;
                int childCount = Bubblecontainer.transform.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    Transform bubble = Bubblecontainer.transform.GetChild(i);
                    Vector3 bubblePosition = bubble.position;
                    float Distance2D = (new Vector2(bubblePosition.x, bubblePosition.y) - new Vector2(worldMousePosition.x, worldMousePosition.y)).magnitude;
                    if (Distance2D <= sprayerRange && bubble.gameObject.activeSelf)
                    {
                        Bubblecontainer.gameObject.GetComponent<BubbleSpawner>().onHit(i);
                    }
                }
                StartCoroutine(setSprayerCooldown());

            }
        }
        if (waffe == Waffe.Bear)
        {

            if (Mouse.current.leftButton.wasPressedThisFrame && !bigShotCooldown)
            {
                int childCount = Bubblecontainer.transform.childCount;
                Paw.transform.position = new Vector3(worldMousePosition.x, worldMousePosition.y, 0.0f);
                Paw.transform.localScale = new Vector3(bigShotRange / 4, bigShotRange / 4, 1.0f);
                PawRenderer.enabled = true;
                bigShotCooldown = true;
                for (int i = 0; i < childCount; i++)
                {
                    Transform bubble = Bubblecontainer.transform.GetChild(i);
                    Vector3 bubblePosition = bubble.position;
                    float Distance2D = (new Vector2(bubblePosition.x, bubblePosition.y) - new Vector2(worldMousePosition.x, worldMousePosition.y)).magnitude;
                    if (Distance2D <= bigShotRange && bubble.gameObject.activeSelf)
                    {
                        bubble.gameObject.transform.parent.GetComponent<BubbleSpawner>().onHit(i);
                    }
                    StartCoroutine(pawDuration());
                    StartCoroutine(setBigShotCooldown());
                }

            }
        }
    }


    public void getUpgradeAvailable(Waffe waffe)
    {
        if (waffe == Waffe.Bear)
        {
            bearAvailable = true;
            bearAvailableIndicatorSprite.enabled = true;
        }
        if (waffe == Waffe.Snake)
        {
            sprayerAvailable = true;
            Debug.Log("snake unlockend");
            sprayAvailableIndicatorSprite.enabled = true;
        }
    }

    IEnumerator setSprayerCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        sprayerCooldown = false;
    }

    IEnumerator setBigShotCooldown()
    {
        yield return new WaitForSeconds(1.0f);
        bigShotCooldown = false;

    }

    IEnumerator fireShotCooldown()
    {
        yield return new WaitForSeconds(3.0f);
        fireAnimationCoolDown = false;

    }
    IEnumerator bearDuration()
    {
        yield return new WaitForSeconds(5.0f);
        waffe = Waffe.Normal;
        bearAvailableIndicatorSprite.enabled = false;
    }

    IEnumerator sprayDuration()
    {
        yield return new WaitForSeconds(50.0f);
        waffe = Waffe.Normal;
        sprayAvailableIndicatorSprite.enabled = false;
        sprayParticleSource.SetActive(false);
    }

    IEnumerator pawDuration()
    {
        PawRenderer.sprite = PawAnimation[0];
        yield return new WaitForSeconds(0.16f);
        PawRenderer.sprite = PawAnimation[1];
        yield return new WaitForSeconds(0.16f);
        PawRenderer.sprite = PawAnimation[2];
        yield return new WaitForSeconds(0.16f);
        Debug.Log("disable");
        PawRenderer.enabled = false;

    }  
   
}
