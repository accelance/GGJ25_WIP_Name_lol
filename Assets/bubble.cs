using UnityEngine;
using System.Collections.Generic;

public class Bubble
{
    // TODO just store the template
    public BubbleTemplate template;
    private Sprite _sprite;


    public Clicker.Waffe bonus;

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
            sr.sharedMaterial.SetTexture("_Texture2D", textureFromSprite(value));
            var collider = o.GetComponent<CircleCollider2D>();
            collider.radius = sr.sprite.bounds.extents.x;
        }
    }

    public Bubble(GameObject o, Material material)
    {
        List<Material> l = new List<Material>();
        l.Add(material);
        this.o = o;
        SpriteRenderer sr = o.AddComponent<SpriteRenderer>();
        sr.SetMaterials(l);
        o.AddComponent<Bubbles>();
        o.AddComponent<CircleCollider2D>();
        // var rb = o.AddComponent<Rigidbody2D>();
        // rb.gravityScale = 0;
    }

    // Stole from: https://discussions.unity.com/t/convert-sprite-image-to-texture/97618/2
    public static Texture2D textureFromSprite(Sprite sprite)
	{
		if(sprite.rect.width != sprite.texture.width){
			Texture2D newText = new Texture2D((int)sprite.rect.width,(int)sprite.rect.height);
			Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x, 
			                                             (int)sprite.textureRect.y, 
			                                             (int)sprite.textureRect.width, 
			                                             (int)sprite.textureRect.height );
			newText.SetPixels(newColors);
			newText.Apply();
			return newText;
		} else
			return sprite.texture;
	}
}