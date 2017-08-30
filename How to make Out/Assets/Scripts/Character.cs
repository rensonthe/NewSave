using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    SpriteRenderer spriteRenderer;

    public float moveSpeed = 3;

    [SerializeField]
    protected int health;

    public abstract bool IsDead { get; }

    public Animator MyAnimator { get; private set; }

    protected bool facingRight;

    // Use this for initialization
    public virtual void Start () {
        facingRight = true;
        spriteRenderer = GetComponent<SpriteRenderer>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeDirection()
    {
            facingRight = !facingRight;
            spriteRenderer.flipX = true;
    }

    public abstract IEnumerator TakeDamage();

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Damage")
        {
            StartCoroutine(TakeDamage());
        }
    }
}
