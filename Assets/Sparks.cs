using UnityEngine;

public class Sparks : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Sprite[] Frames;
    SpriteRenderer sr;
    int index = 0;
    int frameIndex = 0;
    public int speed = 4;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        index = (index + 1) % 48;

        if((index % speed) == 0 && Frames != null) {
            frameIndex++;
            frameIndex = (frameIndex) % Frames.Length;
            Debug.Log(frameIndex);
            sr.sprite = Frames[frameIndex];
        }
    }
}