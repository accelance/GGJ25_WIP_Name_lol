using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public enum BubbleKind {
        Normal,
        Tank,
        Mini,
    }

    public class Bubble {
        public bool alive;
        public BubbleKind kind;
        public GameObject o;
        public float speedOffset; 

        // TODO: add a method void onMouseDown()
        public SpriteRenderer Sprite() {
            return o.GetComponent<SpriteRenderer>();
        }
    }
    public Bubble[] bubbles = new Bubble[256];
    public BoxCollider2D spawnArea;
    public BoxCollider2D dangerZone;
    public GameObject bubble;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnTimer = spawnDelay;
        for(int i = 0; i< bubbles.Length; i++){
            bubbles[i] = makeBubble(Instantiate(bubble, this.transform));
        }
    }

    public float spawnDelay = 0.3f;
    float spawnTimer;
    // Update is called once per frame

    Bubble makeBubble(GameObject o) {
        var it = new Bubble();
        it.o = o;
        return it;
    }

    void spawn(Bubble it, Vector3 position, BubbleKind kind) {
        it.alive = true;

        it.kind = kind;

        float scale = Random.Range(0.1f, 1f);
        // TODO: scale, per kind
        // TODO: expose, per kind
        it.speedOffset = Random.Range(0.9f, 1.1f);
        switch(it.kind) {
            case BubbleKind.Normal: 
                it.Sprite().color = Color.white; 
                break;
            case BubbleKind.Mini: 
                it.Sprite().color = Color.blue; 
                break;
            case BubbleKind.Tank: 
                it.Sprite().color = Color.red; 
                break;
            default:
                Debug.Log("ERROR: Invalid switch case ");
                break;
        }

        it.o.transform.position = position;
        it.o.transform.localScale = new(scale, scale, 0);
        it.Sprite().enabled = true;
    }

    void leftGameArea(Bubble it) {
        it.alive = false;
        it.o.transform.position = Vector3.zero;
        it.Sprite().enabled = false;
    }

    void move(Bubble it) {
        float speed = 0;
        // TODO: parameterize    
        switch(it.kind) {
            case BubbleKind.Normal: speed = 3; break;
            case BubbleKind.Mini: speed = 6; break;
            case BubbleKind.Tank: speed = 1; break;
            default:
                Debug.Log("ERROR: Invalid switch case ");
                break;
        }

        speed *= it.speedOffset;

        it.o.transform.position = it.o.transform.position + new Vector3(0, speed * Time.deltaTime, 0);

    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        
        for(int i = 0; i< bubbles.Length; i++){
            var b = bubbles[i];
            if(b != null) {
                    if(b.alive) {
                    move(b);
                    if(dangerZone.bounds.Contains(b.o.transform.position)){
                        LoseBar.Instance.bubbleHit();
                        leftGameArea(b);
                    }
            } else {
                    if (spawnTimer <= 0) {
                        spawnTimer = spawnDelay;
                        // TODO: actual delays
                        Vector2 p = new(
                            Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                            Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y)
                        );

                        var kind = (BubbleKind)Random.Range(0, 3);
                        spawn(bubbles[i], p, kind);
                    }
                }
            }
        }
    }
}
