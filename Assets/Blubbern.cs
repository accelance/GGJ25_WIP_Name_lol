using UnityEngine;

public class Blubbern : MonoBehaviour
{
    public Texture2D[] texturen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    SpriteRenderer sr;
    int index = 0;
    int frameIndex = 0;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        index = (index + 1) % 48;

        if((index % 7) == 0 && texturen != null) {
            frameIndex++;
            frameIndex = (frameIndex) % 3;
            sr.sharedMaterial.SetTexture("_Texture2D", texturen[frameIndex]);
        }
    }
}
