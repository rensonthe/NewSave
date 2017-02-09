﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Collections.Generic;

public delegate void DeadEventHandler();

public class PlayerController : Character {
    [SerializeField]
    private Stat healthStat;

    public Stat XPStat;
    public Stat soulsStat;

    private int currentXP;
    private int nextLevelXP = 100;
    private int level;
    private int skillPoint;
    private Dictionary<string,SkillPoints> skills = new Dictionary<string, SkillPoints>();

    private Animator skillAnimator;

    public float energyVal;
    public float healVal;
    public float orbLaunchVal;
    public float orbJauntVal;
    public ParticleSystem levelUpEffect;
    public ParticleSystem healthEffect;
    public ParticleSystem xpEffect;
    public ParticleSystem energyEffect;
    public GameObject playerSoul;
    public Transform[] groundPoints;
    public LayerMask whatIsGround;
    public float jumpForce;
    public float groundRadius;
    public bool airControl;
    public bool trig = false;

    private Enemy enemy;
    private float attackTimer;
    private float attackCooldown = 4f;
    private bool canThrow = true;

    public bool immortalty;
    private bool immortal = false;
    public float immortalTime;

    private SpriteRenderer spriteRenderer;
    private bool isCreated;
    private Camera cam;
    private Vector3 mousePosition;
    public float alphaLevel = 1;
    public float totalTime = 0;

    public event DeadEventHandler Dead;

    private static PlayerController instance;

    public static PlayerController Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<PlayerController>();
            }
            return instance;
        }
    }     

    public Rigidbody2D MyRigidBody { get; set; }

    public bool Jump { get; set; }

    public bool OnGround { get; set; }

    public override bool IsDead
    {
        get
        {
            if(healthStat.CurrentVal <= 0)
            {
                OnDead();
            }
            
            return healthStat.CurrentVal <= 0;
        }
    }

    public bool IsFalling
    {
        get
        {
            return MyRigidBody.velocity.y < 0;
        }
    }

    public bool IsJumping
    {
        get
        {
            return MyRigidBody.velocity.y > 0;
        }
    }

    public bool LMB;
    public bool RMB;
    public bool OrbJaunt;
    public bool Bulwark;

    public Vector2 startPos;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        startPos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        MyRigidBody = GetComponent<Rigidbody2D>();
        healthStat.Initialize();
        soulsStat.Initialize();
        XPStat.Initialize();
        XPStat.CurrentVal = currentXP;
        skills.Add("OrbBlast",new SkillPoints(1));
        skills.Add("OrbJaunt", new SkillPoints(1, "OrbBlast"));
        skills.Add("Bulwark", new SkillPoints(1, "OrbBlast"));
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsDead)
        {
            HandleInput();
        }
    }

    void FixedUpdate() {

        if (!IsDead)
        {
            float horizontal = Input.GetAxis("Horizontal");

            HandleMovement(horizontal);

            OnGround = IsGrounded();

            Flip(horizontal);

            HandleLayers();

            attackTimer += Time.deltaTime;
        }

    }

    public void OnDead()
    {
        if(Dead != null)
        {
            Dead();
        }
    }

    void HandleMovement(float horizontal)
    {        
        if (IsFalling)
        {
            gameObject.layer = 11;
            MyAnimator.SetBool("land", true);
        }
        if (!Attack && (OnGround || airControl))
        {
            MyRigidBody.velocity = new Vector2(horizontal * moveSpeed, MyRigidBody.velocity.y);
        }
        if(Jump && MyRigidBody.velocity.y == 0)
        {
            MyRigidBody.AddForce(new Vector2(0, jumpForce));
        }

        MyAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    public void HandleInput()
    {
        if (trig == false)
        {
            if (Input.GetMouseButtonDown(0) && LMB == true)
            {
                MyAnimator.SetTrigger("attack");
            }
            if (Input.GetKeyDown(KeyCode.Space) && !IsFalling)
            {
                MyAnimator.SetTrigger("jump");
            }
            if (Input.GetMouseButtonDown(1) && RMB == true)
            {
                OrbAttack();
            }
            if (Input.GetKeyDown(KeyCode.Q) && OrbJaunt == true)
            {
                if (Orb != null)
                {
                    transform.position = FindObjectOfType<PlayerOrb>().transform.position;
                }
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                GainXP(10);
            }
        }
    }

    void Flip(float horizontal)
    {
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        horizontal = mousePosition.x - transform.position.x;
        if ((horizontal > 0 && !facingRight && !Attack) || (horizontal < 0 && facingRight && !Attack))
        {
            ChangeDirection();
        }
    }

    private bool IsGrounded()
    {
        if(MyRigidBody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers()
    {
        if (!OnGround)
        {
            MyAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            MyAnimator.SetLayerWeight(1, 0);
        }
    }

    public override void SpawnOrb(int value)
    {
        {
            if (facingRight)
            {
                GameObject tmp = (GameObject)Instantiate(Orb, rangedPos.position, Quaternion.identity);
                tmp.GetComponent<PlayerOrb>().Initialize(Vector2.right);
            }
            else
            {
                GameObject tmp = (GameObject)Instantiate(Orb, rangedPos.position, Quaternion.identity);
                tmp.GetComponent<PlayerOrb>().Initialize(Vector2.left);
            }
        }
    }

    private void OrbAttack()
    {
        if (attackTimer >= attackCooldown)
        {
            canThrow = true;
            attackTimer = 0;
        }

        if (canThrow && soulsStat.CurrentVal > 0)
        {
            canThrow = false;
            MyAnimator.SetTrigger("throw");
            soulsStat.CurrentVal -= orbLaunchVal;
        }
    }

    public void SetActive()
    {
        trig = false;
        moveSpeed = 3;      
    }

    public void SetInactive()
    {
        moveSpeed = 0;
        trig = true;
    }

    private IEnumerator IndicateImmortal()
    {
        while (immortal)
        {
            spriteRenderer.enabled = false;

            yield return new WaitForSeconds(.2f);

            spriteRenderer.enabled = true;

            yield return new WaitForSeconds(.2f);
        }
    }

    public override IEnumerator TakeDamage()
    {
        if (!immortal && immortalty)
        {
            healthStat.CurrentVal -= 10;

            if (!IsDead)
            {
                MyAnimator.SetTrigger("damage");
                immortal = true;
                StartCoroutine(IndicateImmortal());
                yield return new WaitForSeconds(immortalTime);

                immortal = false;
            }
            else
            {
                MyAnimator.SetLayerWeight(1, 0);
                MyAnimator.SetTrigger("death");
            }
        }
        else
        {
            if (!IsDead)
            {
                MyAnimator.SetTrigger("damage");
            }
            else
            {
                MyAnimator.SetLayerWeight(1, 0);
                MyAnimator.SetTrigger("death");
            }
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "EnemySword1")
        {
            healthStat.CurrentVal -= 5;
            StartCoroutine(TakeDamage());
        }
        if (other.tag == "EnemyOrb2")
        {
            healthStat.CurrentVal -= 10;
            StartCoroutine(TakeDamage());
        }
    }

    public override void Death()
    {
        StartCoroutine("Checkpoint");
        MyRigidBody.velocity = Vector2.zero;

        if (!isCreated)
        {
            Instantiate(playerSoul, transform.position, Quaternion.identity);
            isCreated = true;
        }

        //if(alphaLevel >= 0)
        //{
        //    totalTime += Time.deltaTime;

        //    if (totalTime >= .09)
        //    {
        //        alphaLevel -= .025f;
        //        totalTime = 0;
        //    }

        //    GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, alphaLevel);
        //}
    }

    private IEnumerator NewLevel()
    {
        MyAnimator.SetTrigger("idle");
        healthStat.CurrentVal = healthStat.MaxVal;
        transform.position = startPos;
        yield return null;
    }

    private IEnumerator Checkpoint()
    {
        yield return new WaitForSeconds(5);
        transform.position = startPos;
        MyAnimator.SetTrigger("idle");
        healthStat.CurrentVal = healthStat.MaxVal;
        StopCoroutine("Checkpoint");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Health")
        {
            if(healthStat.CurrentVal < healthStat.MaxVal)
            {
                healthStat.CurrentVal += healVal;
                Destroy(Instantiate(healthEffect.gameObject, transform.position, Quaternion.identity) as GameObject, healthEffect.startLifetime);
                Destroy(other.gameObject);
            }
        }
        if (other.gameObject.tag == "Energy")
        {
            if (soulsStat.CurrentVal < soulsStat.MaxVal)
            {
                soulsStat.CurrentVal += energyVal;
                Destroy(Instantiate(energyEffect.gameObject, transform.position, Quaternion.identity) as GameObject, healthEffect.startLifetime);
                Destroy(other.gameObject);
            }
        }
        if (other.gameObject.tag == "XPOrb")
        {
            GainXP(UnityEngine.Random.Range(11,15));
            Destroy(Instantiate(xpEffect.gameObject, other.transform.position, Quaternion.identity) as GameObject, healthEffect.startLifetime);
            Destroy(other.gameObject);
        }
    }

    public void SpendSkillPoints(string skillName)
    {
        if(UIManager.Instance.isUpgrading == false)
        {
            int skillPointsRequired = skills[skillName].required;
            string parent = skills[skillName].parent;
            bool parentUnlocked = true;
            if (parent != string.Empty)
            {
                parentUnlocked = skills[parent].unlocked;
            }
            if (skillPoint >= skillPointsRequired && !skills[skillName].unlocked && parentUnlocked)
            {
                skillPoint -= skillPointsRequired;
                UIManager.Instance.currentSkillPoints.text = skillPoint.ToString();
                skillAnimator.SetTrigger("skill_fill");
                switch (skillName)
                {
                    case "OrbBlast":
                        RMB = true;
                        skills[skillName].unlocked = true;
                        break;
                    case "OrbJaunt":
                        OrbJaunt = true;
                        skills[skillName].unlocked = true;
                        break;
                    case "Bulwark":
                        Bulwark = true;
                        skills[skillName].unlocked = true;
                        break;
                }
            }
        }
    }

    public void GainXP(int XP)
    {
        currentXP += XP;
        XPStat.CurrentVal = currentXP;

        if (currentXP >= nextLevelXP)
        {
            level++;
            skillPoint++;
            UIManager.Instance.currentSkillPoints.text = skillPoint.ToString();
            currentXP = currentXP - nextLevelXP;
            nextLevelXP += 10;
            Destroy(Instantiate(levelUpEffect.gameObject, transform.position, Quaternion.identity) as GameObject, levelUpEffect.startLifetime);
            XPStat.MaxVal = nextLevelXP;
            XPStat.CurrentVal = currentXP;
        }
    }

    public void SetSkillAnimation(Animator skillAnimator)
    {
        this.skillAnimator = skillAnimator;
    }
}
