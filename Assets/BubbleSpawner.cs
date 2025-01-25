using UnityEngine;
using System.Collections;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField]
    public BubbleTemplate[] templates = new BubbleTemplate[3];
    public Bubble[] bubbles = new Bubble[64];
    public BoxCollider2D spawnArea;
    public BoxCollider2D dangerZone;

    public Material material;

    public MonoBehaviour bubbleScript;
    //TODO: per kind in template
    public float spawnsPerSecond = 3f;
    float spawnTimer;

    void Start()
    {
        spawnTimer = 1 / spawnsPerSecond;
        for (int i = 0; i < bubbles.Length; i++)
        {
            var o = new GameObject("Bubble");
            o.transform.parent = this.transform;
            bubbles[i] = new Bubble(o, material);
        }
    }

    public void onHit(int index)
    {
        var it = bubbles[index];
        if (it.hp > 0 && it.state != BubbleState.Invincible)
        {
            it.hp -= 1;
            if (it.hp == 0)
            {
                StartCoroutine(popBubble(it));
                it.desiredScale = 0.1f;
                it.state = BubbleState.Popped;
            }
            else
            {
                it.sprite = it.template.death[0];

                it.state = BubbleState.Invincible;
                it.desiredScale = it.currentScale * 0.5f;
                it.tinvicible = 0.4f; // TODO expose

                it.ddp = new Vector3(0, 0.5f, 0);
            }
            it.dp = new Vector3(Random.Range(-1f, 1f), Random.Range(-0.8f, 0.4f), 0);
        }
    }

    void FixedUpdate()
    {
        spawnTimer -= Time.deltaTime;

        for (int i = 0; i < bubbles.Length; i++)
        {
            var it = bubbles[i];
            if (it == null)
            {
                Start();
            }
            else
            {
                it.age += Time.deltaTime;

                if (it.state == BubbleState.Alive) {
                    it.idleIndex++;
                    if (it.idleIndex % 16 == 0)
                    {
                        it.animationIndex = (it.animationIndex + 1) % 3;
                        it.sprite = it.template.idle[it.animationIndex];
                    }
                }




                if (it.tscale < 1)
                {
                    it.tscale += Time.deltaTime;
                    float scale = Mathf.Lerp(it.currentScale, it.desiredScale, it.tscale);
                    it.o.transform.localScale = new Vector3(scale, scale, 0);
                }
                else
                {
                    it.currentScale = it.desiredScale;
                    it.tscale = 0;
                }
                
                var collider = it.o.GetComponent<CircleCollider2D>();
                

                switch (it.state)
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
                        move(it);
                        if (dangerZone.bounds.Contains(it.o.transform.position))
                        {
                            LoseBar.Instance.bubbleHit();
                            despawn(it);
                        }
                        break;
                    case BubbleState.Invincible:
                        move(it);
                        it.tinvicible -= Time.deltaTime;
                        if (it.tinvicible > 0)
                        {
                            float t = (Mathf.Sin(it.tinvicible * 20f) + 1) * 0.5f;
                            t = t * t;
                            it.currentColor = Color.Lerp(new Vector4(1,1,1, 0.5f), Color.white, t);
                        }
                        else
                        {
                            it.state = BubbleState.Alive;
                            it.currentColor = Color.white;
                        }
                        break;
                    case BubbleState.Popped:
                        move(it);

                        if (it.desiredScale == it.currentScale)
                        {
                            foreach(var other in bubbles) {
                                var direction = other.p - it.p;
                                var length = Mathf.Max(1f, direction.magnitude);
                                direction *= 1/(length*length*length);

                                Debug.DrawRay(other.p, direction, Color.yellow);
                                other.dp += direction*0.2f;
                                other.ddp -= direction*0.25f;
                            }
                            despawn(it);
                        }
                        break;
                    default:
                        Debug.Log("ERROR: Invalid switch case ");
                        break;
                }
            }
        }
    }

    public IEnumerator popBubble(Bubble it)
    {
        var frames = it.template.death;
        it.sprite = frames[frames.Length - 2];
        yield return new WaitForSeconds(0.16f);
        it.sprite = frames[frames.Length - 1];
        yield return new WaitForSeconds(0.16f);
        it.o.SetActive(false);
    }

    void spawn(Bubble it, Vector3 position, BubbleTemplate template)
    {
        it.state = BubbleState.Alive;

        it.template = template;


        it.p = position;

        float speedOffset = Random.Range(-template.speedVariance, template.speedVariance);
        for(int octave = 0; octave < it.swayFactors.Length; octave++) {
            it.swayFactors[octave]   = Mathf.Pow(2, octave + Random.Range(-template.speedVariance, template.speedVariance));
            it.swayStrengths[octave] = 1 / it.swayFactors[octave];
        }

        it.dp = new Vector3(0, 0.5f, 0);
        it.ddp = new Vector3(0, 0.3f + speedOffset, 0);
        it.o.transform.position = position;

        float scale = template.averageSize + Random.Range(-template.sizeVariance, template.sizeVariance);
        it.currentScale = scale;
        it.desiredScale = scale;
        it.tscale = 0;

        it.hp = template.maxHP;

    }

    void despawn(Bubble it)
    {
        it.state = BubbleState.None;
        it.p = new Vector3(0, -100, 0);
        it.dp = Vector3.zero;
        it.ddp = Vector3.zero;

        it.o.transform.position = it.p;
    }

    BubbleTemplate getTemplate(BubbleKind kind)
    {
        return templates[(int)kind];
    }

    void move(Bubble it)
    {
        float sway = 0;
        for(int octave = 0; octave < it.swayFactors.Length; octave++) {
            sway += Mathf.Sin(it.age * it.swayFactors[octave]) * it.swayStrengths[octave];
        }

        float timeSquared = Time.deltaTime * Time.deltaTime;

        it.ddp +=  it.ddp * -0.1f * Time.deltaTime;
        it.dp  += it.ddp * 0.5f * timeSquared + it.ddp * Time.deltaTime;
        it.dp  += new Vector3(sway * 0.2f, 0, 0) * Time.deltaTime;

        it.p += it.ddp * 0.5f * timeSquared + it.dp * Time.deltaTime;
        it.o.transform.position = it.p;
        // var rb = it.o.GetComponent<Rigidbody2D>();
        // rb.MovePosition(it.p);
    }
}
