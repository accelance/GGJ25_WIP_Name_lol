using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoseBar : MonoBehaviour
{

    public static LoseBar Instance;

    public VFXPlayer vfx;

    Image image;

    public int maxBubbles = 20;
    int currentlyHitBubbles = 0;

    bool gameOver = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
        image.fillAmount = 0.0f;
        Instance = this;
        vfx = VFXPlayer.Instance;
    }

    public void bubbleHit() {
        currentlyHitBubbles++;
        image.fillAmount = (float) currentlyHitBubbles / (float) maxBubbles;
        if(!gameOver) {
            vfx.playVFX(2);
        if(currentlyHitBubbles >= maxBubbles && !gameOver) {
            gameOver = true;
            StartCoroutine(endGame());
        }
        }
    }

    IEnumerator endGame() {
        vfx.playVFX(3);
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene("UI");
    }
}
