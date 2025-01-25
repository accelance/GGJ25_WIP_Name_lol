using UnityEngine;

public class Bubble
{
    // TODO just store the template
    public BubbleTemplate template;
    private Sprite _sprite;


    public BubbleState state;
    public GameObject o;

    public float age;

    public Vector3 p;
    public Vector3 dp;
    public Vector3 ddp;

    public float[] swayFactors = new float[3];
    public float[] swayStrengths = new float[3];



    public int idleIndex = 0;
    public int animationIndex = 0;



    public int hp;
    //TODO: maybe by template
    public float currentScale;
    public float desiredScale;
    public float tscale;
    public float tinvicible;
    public Color _currentColor;
    public Color currentColor
    {
        get
        {
            return _currentColor;
        }
        set
        {
            _currentColor = value;
            var sr = o.GetComponent<SpriteRenderer>();
            sr.color = value;
        }
    }
    public Sprite sprite
    {
        get
        {
            return _sprite;
        }
        set
        {
            _sprite = value;
            var sr = o.GetComponent<SpriteRenderer>();
            sr.sprite = value;
            var collider = o.GetComponent<CircleCollider2D>();
            collider.radius = sr.sprite.bounds.extents.x;
        }
    }

    public Bubble(GameObject o)
    {
        this.o = o;
        o.AddComponent<SpriteRenderer>();
        o.AddComponent<Bubbles>();
        o.AddComponent<CircleCollider2D>();
        // var rb = o.AddComponent<Rigidbody2D>();
        // rb.gravityScale = 0;
    }
}