using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoseBar : MonoBehaviour
{

    public static LoseBar Instance;

    Image image;

    int maxBubbles = 20;
    int currentlyHitBubbles = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
        image.fillAmount = 0.0f;
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void bubbleHit() {
        currentlyHitBubbles++;
        image.fillAmount = (float) currentlyHitBubbles / (float) maxBubbles;
        if(currentlyHitBubbles >= maxBubbles) {
             SceneManager.LoadScene(3);
        }
    }
}
