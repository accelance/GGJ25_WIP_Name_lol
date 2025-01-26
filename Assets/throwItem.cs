using UnityEngine;

public class throwItem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Rigidbody2D body;
    SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        sr.enabled = false;
    }

    public void Fire () {
        transform.localPosition = new Vector3(0.0f,0.0f,0.0f);
        body.WakeUp();
        sr.enabled = true;
        body.gravityScale = 1.0f;
        body.AddForce(new Vector2(.5f, 2.2f) * 7, ForceMode2D.Impulse);
    }

    public void OnCollisionEnter2D(Collision2D c) {
        Debug.Log(c.collider.gameObject.tag);
        if(c.collider.gameObject.tag == "checker") {
            sr.enabled = false;
            body.gravityScale = 0.0f;
            body.Sleep();
            transform.localPosition = new Vector3(0.0f,0.0f,0.0f);
        }
    }


}
