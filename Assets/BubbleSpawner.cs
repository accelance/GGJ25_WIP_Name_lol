using UnityEngine;
using System.Collections;

public class BubbleSpawner : MonoBehaviour
{
    
    [SerializeField]
    public Sprite[] bear  = new Sprite[3];
    public Sprite[] snake  = new Sprite[3];
    public Sprite[] idle  = new Sprite[3];
    [SerializeField]
    public Sprite[] death = new Sprite[5];
    [SerializeField]
    public BubbleTemplate[] templates = new BubbleTemplate[3];
    public Bubble[] bubbles = new Bubble[64];
    public BoxCollider2D spawnArea;
    public BoxCollider2D dangerZone;
    [SerializeField]
    public BubbleKind baseKind;
    public BubbleRule[] bubbleRules;
    public BubbleRule[] bonusBubbles;
    int ruleCursor;
    int bonusRuleCursor;


    public Material material;

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

                switch(it.bonus) {
                    case Clicker.Waffe.Normal:
                        break;
                    case Clicker.Waffe.Bear:
                    case Clicker.Waffe.Snake:
                        Clicker.Instance.getUpgradeAvailable(it.bonus);
                        break;
                    
                }
            }
            else
            {
                it.sprite = death[it.template.maxHP - it.hp];

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
                        // TODO: fix this
                        it.animationIndex = (it.animationIndex+1) % 3;
                        switch(it.bonus) {
                            case Clicker.Waffe.Normal:
                                it.sprite = idle[it.animationIndex];
                                break;
                            case Clicker.Waffe.Bear:
                                it.sprite = bear[it.animationIndex];
                                break;
                            case Clicker.Waffe.Snake:
                                it.sprite = snake[it.animationIndex];
                                break;
                        }
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

                            Vector2 p = new(
                                Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                                Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y)
                            );

                            var kind = BubbleKind.Normal;
                            Clicker.Waffe bonus = Clicker.Waffe.Normal;
                            bool overwritten = false;
                            if (bubbleRules.Length > 0) {
                                var everyXBubblesIsA = bubbleRules[ruleCursor];
                                everyXBubblesIsA.index--;
                                if (everyXBubblesIsA.index <= 0) {
                                    everyXBubblesIsA.index = everyXBubblesIsA.count;
                                    kind = everyXBubblesIsA.kind;
                                    overwritten = true;
                                    ruleCursor++;
                                    if (ruleCursor>= bubbleRules.Length) {
                                        ruleCursor = 0;
                                    }
                                }
                            }

                            if (!overwritten && bonusBubbles.Length > 0) {
                                var everyXBubblesIsA = bonusBubbles[bonusRuleCursor];
                                everyXBubblesIsA.index--;
                                if (everyXBubblesIsA.index <= 0) {
                                    everyXBubblesIsA.index = everyXBubblesIsA.count;
                                    kind = everyXBubblesIsA.kind;
                                    bonus = everyXBubblesIsA.bonus;
                                    overwritten = true;
                                    bonusRuleCursor++;
                                    if (bonusRuleCursor>= bonusBubbles.Length) {
                                        bonusRuleCursor = 0;
                                    }
                                }
                            }
                            
                            spawn(bubbles[i], p, getTemplate(kind), bonus);
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
        it.sprite = death[death.Length - 2];
        yield return new WaitForSeconds(0.16f);
        it.sprite = death[death.Length - 1];
        yield return new WaitForSeconds(0.16f);
        despawn(it);
    }

    void spawn(Bubble it, Vector3 position, BubbleTemplate template, Clicker.Waffe bonus)
    {
        it.state = BubbleState.Alive;

        it.template = template;

        it.bonus = bonus;


        for(int octave = 0; octave < it.swayFactors.Length; octave++) {
            it.swayFactors[octave]   = Mathf.Pow(2, octave + Random.Range(-template.speedVariance, template.speedVariance));
            it.swayStrengths[octave] = 1 / it.swayFactors[octave];
        }

        float speedOffset = Random.Range(-template.speedVariance, template.speedVariance);
        it.p = position;
        it.dp = Vector3.zero;
        it.ddp = new Vector3(0, it.template.averageSpeed + speedOffset, 0);

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
        Debug.DrawRay(it.p, it.dp);
        // var rb = it.o.GetComponent<Rigidbody2D>();
        // rb.MovePosition(it.p);
    }
}
