using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [System.Serializable]
    public class BubbleTemplate
    {
        public BubbleKind kind;
        public int maxHP;
        public float averageSpeed;
        public float speedVariance;
        public float averageSize;
        public float sizeVariance;

        public Sprite sprite;
        public Color color;

        public BubbleTemplate(BubbleKind kind, int maxHP, float averageSpeed, float speedVariance, float averageSize, float sizeVariance){
            this.kind = kind;
            this.averageSpeed = averageSpeed;
            this.speedVariance = speedVariance;
            this.averageSize = averageSize;
            this.sizeVariance = sizeVariance;
            this.maxHP = maxHP;
        }
    }

    

    [SerializeField]
    public BubbleTemplate[] templates = new BubbleTemplate[] {
        new(BubbleKind.Normal, 1, 2.0f, 0.2f, 2.0f, 0.2f),
        new(BubbleKind.Mini,   1, 2.5f, 0.2f, 1.3f, 0.1f),
        new(BubbleKind.Tank,   3, 1.0f, 0.4f, 3.5f, 0.5f),
    };
    public Bubble[] bubbles = new Bubble[64];
    public BoxCollider2D spawnArea;
    public BoxCollider2D dangerZone;

    //TODO: per kind in template
    public float spawnsPerSecond = 3f;
    float spawnTimer;

    void Start()
    {
        spawnTimer = 1 / spawnsPerSecond;
        for (int i = 0; i < bubbles.Length; i++)
        {
            var o = new GameObject();
            o.transform.parent = this.transform;
            bubbles[i] = new Bubble(o);
        }
    }

    public void onHit(int index) {
        var b = bubbles[index];
        if (b.hp > 0 && b.state != BubbleState.Invincible ){
            b.hp -= 1;
            if(b.hp == 0){
                b.desiredScale = 0.1f;
                b.state = BubbleState.Popped;
            }else {
                b.state = BubbleState.Invincible;
                b.desiredScale = b.currentScale * 0.5f;
                b.tinvicible = 0.4f; // TODO expose
            }
        }
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        for (int i = 0; i < bubbles.Length; i++)
        {
            var b = bubbles[i];
            if (b == null){
                int childCount = transform.childCount;
                for(int count = 0; count < childCount; count++ ) {
                    Destroy(transform.GetChild(0));
                }
                Start();
            } else 
            {
                if(b.tscale < 1) {
                    b.tscale += Time.deltaTime;
                    float scale = Mathf.Lerp(b.currentScale, b.desiredScale, b.tscale);
                    b.o.transform.localScale = new Vector3(scale, scale, 0);
                } else {
                    b.currentScale = b.desiredScale;
                    b.tscale = 0;
                }

                switch (b.state)
                {
                    case BubbleState.None:
                        if (spawnTimer <= 0)
                        {
                            spawnTimer = 1 / spawnsPerSecond;
                            // TODO: actual delays
                            Vector2 p = new(
                                Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                                Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y)
                            );

                            var kind = (BubbleKind)Random.Range(0, 3);
                            spawn(bubbles[i], p, getTemplate(kind));
                        }
                        break;
                    case BubbleState.Alive:
                        move(b);
                        if (dangerZone.bounds.Contains(b.o.transform.position))
                        {
                            // LoseBar.Instance.bubbleHit();
                            despawn(b);
                        }
                        break;
                    case BubbleState.Invincible:
                        move(b);
                        b.tinvicible -= Time.deltaTime;
                        if(b.tinvicible > 0) {
                            float t = (Mathf.Sin(b.tinvicible*20f) + 1) *0.5f;
                            t = t*t;
                            b.currentColor = Color.Lerp(b.color, Color.white, t);
                        } else {
                            b.state = BubbleState.Alive;
                            b.currentColor = b.color;
                        }
                        break;
                    case BubbleState.Popped:
                        if(b.desiredScale == b.currentScale) {
                            despawn(b);
                        }
                        break;
                    default:
                        Debug.Log("ERROR: Invalid switch case ");
                        break;
                }
            }
        }
    }

    
    void spawn(Bubble it, Vector3 position, BubbleTemplate template)
    {
        it.state = BubbleState.Alive;

        it.sprite = template.sprite;
        it.color = template.color;
        it.currentColor = template.color;

        it.kind = template.kind;

        it.o.transform.position = position;

        float scale = template.averageSize + Random.Range(-template.sizeVariance , template.sizeVariance);
        it.currentScale = scale;
        it.desiredScale = scale;
        it.tscale = 0;

        it.hp = template.maxHP;

        it.speedOffset = Random.Range(-template.speedVariance, template.speedVariance);
    }

    void despawn(Bubble it)
    {
        it.state = BubbleState.None;
        it.o.transform.position = new Vector3(0, -100, 0);
    }

    BubbleTemplate getTemplate(BubbleKind kind) {
        return  templates[(int) kind];
    }

    void move(Bubble it)
    {
        float speed = getTemplate(it.kind).averageSpeed;
        speed += it.speedOffset;

        //TODO: swaying
        it.o.transform.position = it.o.transform.position + new Vector3(0, speed * Time.deltaTime, 0);
    }
}
