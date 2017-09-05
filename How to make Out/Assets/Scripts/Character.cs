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
    
    [SerializeField]
    protected int health;

    public abstract bool IsDead { get; }

    public bool isSpeech;

    public bool TakingDamage { get; set; }

    public Animator MyAnimator { get; private set; }

    [SerializeField]
    private List<string> damageSources;

    protected bool facingRight;

    // Use this for initialization
    public virtual void Start () {
        facingRight = true;
        spriteRenderer = GetComponent<SpriteRenderer>();

        MyAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeDirectionPlayer()
    {
        if (isSpeech)
        {
            for (int i = 0; i < speechBubble.Length; i++)
            {
                Vector3 speechBubblePos = speechBubble[1].position;
                speechBubble[1].SetParent(null);
                facingRight = !facingRight;
                transform.localScale = new Vector3(transform.localScale.x * -1, 0.4f, 0.4f);
                speechBubble[1].SetParent(transform);
                speechBubble[1].position = speechBubblePos;
            }
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

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(damageSources.Contains(other.tag))
        {
            StartCoroutine(TakeDamage());
        }
    }
}
