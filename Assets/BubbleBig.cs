using UnityEngine;
using System.Collections;


public class BubbleBig : Bubbles
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int HP = 3;


    void Start()
    {
        collider = GetComponent<CircleCollider2D>();
        HP = frames.Length - 2;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = frames[0];
        collider.radius = spriteRenderer.sprite.bounds.extents.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    

    public override void onHit() {
        Debug.Log("bubble was hit");
        HP -= 1;
        if(HP <= 0) {
            StartCoroutine(popBubble());
        } else {
            spriteRenderer.sprite = frames[frames.Length - (HP + 2)];
            collider.radius = spriteRenderer.sprite.bounds.extents.x;
        }
    }
}
