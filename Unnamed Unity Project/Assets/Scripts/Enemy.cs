using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum ENEMYTYPE { RANGED, MELEE, RANGEDMELEE}

public class Enemy : Character {

    [SerializeField]
    private ENEMYTYPE myType;

    public Collider2D[] other;

    public GameObject redSoul;

    public ParticleSystem deathEffect;

    private IEnemyState currentState;

    public GameObject Target { get; set; }

    public float meleeRange;

    public bool InMeleeRange
    {
        get
        {
            if(Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;
            }

            return false;
        }
    }

    public float throwRange;

    public bool InThrowRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= throwRange;
            }

            return false;
        }
    }

    public float MeleeTimer { get; set; }

    public float RangedTimer { get; set; }

    private Vector3 startPos;

    public Transform leftEdge;
    public Transform rightEdge;

    public override bool IsDead
    {
        get
        {
            return health <= 0;
        }
    }

    // Use this for initialization
    public override void Start () {

        base.Start();
        PlayerController.Instance.Dead += new DeadEventHandler(RemoveTarget);
        ChangeState(new IdleState());
        for (int i = 0; i < other.Length; i++)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other[i], true);
        }
        SetState();
    }

    public void SetState()
    {
        switch (myType)
        {
            case ENEMYTYPE.RANGED:
                ChangeState(new RangedState());
                break;
            case ENEMYTYPE.MELEE:
                ChangeState(new MeleeState());
                break;
            case ENEMYTYPE.RANGEDMELEE:
                ChangeState(new RangedMeleeState());
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!IsDead)
        {
            if (!TakingDamage)
            {
                currentState.Execute();
            }

            LookAtTarget();
        }
	}

    void FixedUpdate()
    {
        MeleeTimer += Time.deltaTime;
        RangedTimer += Time.deltaTime;
    }

    IEnumerator SpawnChance()
    {
        yield return new WaitForSeconds(1f);
        if (UnityEngine.Random.value > 0.5f)
        {
            Instantiate(redSoul, transform.position, Quaternion.identity);
        }
        StopCoroutine("SpawnChance");
    }

    public void RemoveTarget()
    {
        Target = null;
        ChangeState(new PatrolState());
    }

    private void LookAtTarget()
    {
        if (Target != null)
        {
            float xDir = Target.transform.position.x - transform.position.x;

            if (xDir > 0 && facingRight || xDir < 0 && !facingRight)
            {
                ChangeDirection();
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
        if (!Attack)
        {
            if((GetDirection().x > 0 && transform.position.x < rightEdge.position.x) || (GetDirection().x < 0 && transform.position.x > leftEdge.position.x))
            {
                MyAnimator.SetFloat("speed", 1);

                transform.Translate(GetDirection() * (moveSpeed * Time.deltaTime), Space.World);
            }
            else if(currentState is PatrolState)
            {
                ChangeDirection();
            }
        }
    }

    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.left : Vector2.right;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        currentState.OnTriggerEnter(other);
        if (other.tag == "Sword")
        {
            health -= 10;
            StartCoroutine(TakeDamage());
        }
        if (other.tag == "Orb")
        {
            health -= 10;
            StartCoroutine(TakeDamage());
        }
        if(other.tag == "OrbJaunt")
        {
            health -= 20;
            StartCoroutine(TakeDamage());
        }
    }

    public override IEnumerator TakeDamage()
    {
        if (!IsDead)
        {
            MyAnimator.SetTrigger("damage");
        }
        else
        {
            MyAnimator.SetTrigger("death");
            StartCoroutine("SpawnChance");
            yield return new WaitForSeconds(1f);
            Destroy(Instantiate(deathEffect.gameObject, transform.position, Quaternion.identity) as GameObject, deathEffect.startLifetime);            
            yield return null;
        }
    }

    public override void Death()
    {        
        Destroy(gameObject);
    }
}
