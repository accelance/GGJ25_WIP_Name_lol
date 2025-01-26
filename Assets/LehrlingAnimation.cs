using UnityEngine;
using System.Collections;

public class LehrlingAnimation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Sprite[] Frames;

    public Sprite[] fireFrames;
    SpriteRenderer sr;
    int index = 0;
    int frameIndex = 0;
    public int speed = 4;

    float fireAnimationRate = 0.1f;

    bool idle = true;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        
    }

    public void Fire() {
        StartCoroutine(fireAnimation());
    }

    IEnumerator fireAnimation() {
        idle = false;
        for(int i = 0; i < fireFrames.Length; i++) {
            sr.sprite = fireFrames[i];
            yield return new WaitForSeconds(fireAnimationRate);
            if(i == fireFrames.Length - 1) {
                Vector3 deltaVector =  new Vector3 (.5f, 0.0f, 0.0f);
                transform.position -= deltaVector;
                yield return new WaitForSeconds(.2f);
                transform.position += deltaVector;
            }
        }
        idle = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(idle) {
            index = (index + 1) % 48;

        if((index % speed) == 0 && Frames != null) {
            frameIndex++;
            frameIndex = (frameIndex) % Frames.Length;
            sr.sprite = Frames[frameIndex];
        }
        }
        
    }
}