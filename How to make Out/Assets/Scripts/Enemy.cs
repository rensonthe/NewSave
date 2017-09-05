using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

    private IEnemyState currentState;

	// Use this for initialization
	public override void Start () {
        base.Start();   

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
        transform.Translate(GetDirection() * (moveSpeed * Time.deltaTime));
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
            MyAnimator.SetTrigger("death");
            yield return null;
        }
    }

    public override bool IsDead
    {
        get
        {
            return health <= 0;         

        }
    }
}
