using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

    private SpriteRenderer fadeImage;
    private IEnemyState currentState;

    [SerializeField]
    private Transform leftEdge;
    [SerializeField]
    private Transform rightEdge;

	// Use this for initialization
	public override void Start () {
        base.Start();
        fadeImage = GetComponent<SpriteRenderer>();

        ChangeState(new IdleState());     		
	}
	
	// Update is called once per frame
	void Update () {
        if (!IsDead && !IsDead)
        {
            if (!TakingDamage)
            {
                currentState.Execute();
            }
        }

        if (!isInTransition)
            return;


        transition += isShowing ? Time.deltaTime * (1 / duration) : -Time.deltaTime * (1 / duration);
        fadeImage.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, transition);

        if (transition > 1 || transition < 0)
            isInTransition = false;
    }

    public void ChangeState(IEnemyState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter(this);
    }

    public void Move()
    {
        if ((GetDirection().x > 0 && transform.position.x < rightEdge.position.x) || (GetDirection().x < 0 && transform.position.x > leftEdge.position.x))
        {
            transform.Translate(GetDirection() * (moveSpeed * Time.deltaTime));
        }
        else if(currentState is PatrolState)
        {
            ChangeDirectionTryangle();
        }
    }

    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        currentState.OnTriggerEnter(other);
    }

    public override IEnumerator TakeDamage()
    {
        health -= 1;

        if (!IsDead)
        {
            MyAnimator.SetTrigger("damage");
        }
        else
        {
            Destroy(gameObject.GetComponent<Collider2D>());
            StartCoroutine("FadeCheckOut");
            MyAnimator.SetTrigger("death");
            yield return null;
        }
    }

    public IEnumerator FadeCheckOut()
    {
        Fade(false, 1f);
        StopCoroutine("FadeCheckOut");
        yield return null;
    }

    private bool isInTransition;
    private float transition;
    private bool isShowing;
    private float duration;

    public void Fade(bool showing, float duration)
    {
        isShowing = showing;
        isInTransition = true;
        this.duration = duration;
        transition = (isShowing) ? 0 : 1;
    }

    public override void Death()
    {
        Destroy(gameObject);
    }

    public override bool IsDead
    {
        get
        {
            return health <= 0;         

        }
    }
}
