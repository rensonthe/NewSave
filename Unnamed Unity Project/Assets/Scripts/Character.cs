using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Character : MonoBehaviour {

    SpriteRenderer spriteRenderer;

    [Header("Character")]
    public GameObject Orb;

    public float moveSpeed = 3;

    public Transform rangedPos;

    public int health;

    public Transform speechBubble;

    public bool isSpeech;

    public EdgeCollider2D []swordCollider;

    private int colliderIndex;

    public List<string> damageSources;

    public abstract bool IsDead { get; }

    protected bool facingRight;

    public bool Attack { get; set; }

    public bool TakingDamage { get; set; }

    public Animator MyAnimator { get; private set; }

    public EdgeCollider2D []SwordCollider
    {
        get
        {
            return swordCollider;
        }
    }

    public virtual void Start()
    {
        facingRight = true;
        spriteRenderer = GetComponent<SpriteRenderer>();

        MyAnimator = GetComponent<Animator>();
    }

    void Update()
    {

    }

    public abstract IEnumerator TakeDamage();

    public abstract void Death();

    public void ChangeDirectionLeft()
    {
        if (isSpeech)
        {
            Vector3 speechBubblePos = speechBubble.position;
            speechBubble.SetParent(null);
            facingRight = !facingRight;
            spriteRenderer.flipX = true;
            speechBubble.SetParent(transform);
            speechBubble.position = speechBubblePos;
        }
        else
        {
            facingRight = !facingRight;
            spriteRenderer.flipX = true;
        }
    }

    public void ChangeDirectionRight()
    {
        if (isSpeech)
        {
            Vector3 speechBubblePos = speechBubble.position;
            speechBubble.SetParent(null);
            facingRight = !facingRight;
            spriteRenderer.flipX = false;
            speechBubble.SetParent(transform);
            speechBubble.position = speechBubblePos;
        }
        else
        {
            facingRight = !facingRight;
            spriteRenderer.flipX = false;
        }
    }

    public virtual void SpawnOrb(int value)
    {
        if (facingRight)
        {
            GameObject tmp = (GameObject)Instantiate(Orb, rangedPos.position, Quaternion.identity);
            tmp.GetComponent<Orb>().Initialize(Vector2.left);
        }
        else
        {
            GameObject tmp = (GameObject)Instantiate(Orb, rangedPos.position, Quaternion.identity);
            tmp.GetComponent<Orb>().Initialize(Vector2.right);
        }
    }

    public virtual void MeleeAttack()
    {
        SwordCollider[0].enabled = true;
    }

    public void CorruptedMeleeAttack(int c)
    {
        colliderIndex = c;
        SwordCollider[c].enabled = true;
    }

    public void DisableCollider()
    {
        SwordCollider[colliderIndex].enabled = false;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(damageSources.Contains(other.tag))
        {
            StartCoroutine(TakeDamage());
        }
    }
}
