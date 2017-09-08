using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    SpriteRenderer spriteRenderer;

    public float moveSpeed = 3;
    [SerializeField]
    private GameObject fireballPrefab;
    [SerializeField]
    private Transform fireballPos;

    public Transform[] speechBubble;
    
    public int health;

    public abstract bool IsDead { get; }

    public bool isSpeech;

    public bool TakingDamage { get; set; }

    public Animator MyAnimator { get; private set; }

    public List<string> damageSources;

    protected bool facingRight;

    // Use this for initialization
    public virtual void Start () {
        facingRight = true;
        spriteRenderer = GetComponent<SpriteRenderer>();

        MyAnimator = GetComponent<Animator>();
    }
	
    public void ChangeDirectionPlayer()
    {
        if (isSpeech)
        {

            Vector3 speechBubblePos0 = speechBubble[0].position;
            speechBubble[0].SetParent(null);
            Vector3 speechBubblePos1 = speechBubble[1].position;
            speechBubble[1].SetParent(null);
            facingRight = !facingRight;
            transform.localScale = new Vector3(transform.localScale.x * -1, 0.4f, 0.4f);
            speechBubble[0].SetParent(transform);
            speechBubble[0].position = speechBubblePos0;
            speechBubble[1].SetParent(transform);
            speechBubble[1].position = speechBubblePos1;
        }
    }

    public void ChangeDirectionTryangle()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, 0.8f, 0.75f);
    }

    public void Fireball()
    {
        if (facingRight)
        {
            GameObject tmp = (GameObject)Instantiate(fireballPrefab, fireballPos.position, Quaternion.identity);
            tmp.GetComponent<Fireball>().Initialize(Vector2.right);
        }
        else
        {
            GameObject tmp = (GameObject)Instantiate(fireballPrefab, fireballPos.position, Quaternion.Euler(new Vector3(0, 0, -180)));
            tmp.GetComponent<Fireball>().Initialize(Vector2.left);
        }
    }

    public abstract IEnumerator TakeDamage();

    public abstract void Death();

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(damageSources.Contains(other.tag))
        {
            StartCoroutine(TakeDamage());
        }
    }
}
