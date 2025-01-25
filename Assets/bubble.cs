using UnityEngine;

public class Bubble
{
    public BubbleState state;
    public GameObject o;
    public BubbleKind kind;
    public float speedOffset;
    public int hp;
    //TODO: maybe by template
    public float currentScale;
    public float desiredScale;
    public float tscale;
    public float tinvicible;
    public Color color;
    public Color _currentColor;
    public Color currentColor {
        get {
            return _currentColor;
        }
        set {
            _currentColor = value;
            var sr = o.GetComponent<SpriteRenderer>();
            sr.color = value;
        }
    }
    private Sprite _sprite;
    public Sprite sprite {
        get {
            return _sprite;
        }
        set {
            _sprite = value;
            var sr = o.GetComponent<SpriteRenderer>();
            sr.sprite = value;
        }
    }

    public Bubble(GameObject o)
    {
        this.o = o;
        o.AddComponent<SpriteRenderer>();
    }
}