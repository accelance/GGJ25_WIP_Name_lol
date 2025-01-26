using UnityEngine;

public class HueSh : MonoBehaviour
{
    SpriteRenderer sr;
    int index = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        index = (index + 1) % 255;

        sr.sharedMaterial.SetFloat("_HueShift", (float) index);
        
    }
}
