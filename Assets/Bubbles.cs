using UnityEngine;
using System.Collections;


public class Bubbles : MonoBehaviour
{

    public Sprite[] idleAnimation;

    public Sprite[] frames;

    public SpriteRenderer spriteRenderer;

    public CircleCollider2D collider;

    int idleIndex = 0;

    int animationIndex = 0;

    public bool idle = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = idleAnimation[0];

    }


    void Update() {

        idleIndex++;

        if(idleIndex % 16 == 0 && idle) {
            animationIndex = (animationIndex + 1) % 3;
            spriteRenderer.sprite = idleAnimation[animationIndex];
        }
        
        

    }
    public virtual void onHit() {
        idle = false;
        StartCoroutine(popBubble());
    }

    void OnMouseDown() {
        Debug.Log(transform.GetSiblingIndex());
        if(Clicker.Instance.waffe == Clicker.Waffe.Normal) {
            
            transform.parent.gameObject.GetComponent<BubbleSpawner>().onHit(transform.GetSiblingIndex());
            onHit();
        }
    }

    public IEnumerator popBubble() {
        spriteRenderer.sprite = frames[frames.Length - 2];
        yield return new WaitForSeconds(0.16f);
        spriteRenderer.sprite = frames[frames.Length - 1];
        yield return new WaitForSeconds(0.16f);
        this.gameObject.SetActive(false);
    }

    




}
