using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public delegate void DeadEventHandler();

public class PlayerController : Character
{
    private static PlayerController instance;
    public static PlayerController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<PlayerController>();
            }
            return instance;
        }
    }
    public event DeadEventHandler Dead;
    [Header("Mode")]
    public bool Wraith;
    [Header("Stats")]
    [SerializeField]
    private Stat healthStat;
    public Stat mentalityStat;
    public Stat XPStat;
    public Stat soulsStat;
    public Stat corruptionStat;
    private float currentCorruption = 0;
    public float CurrentCorruption { get { return currentCorruption; } set { corruptionStat.CurrentVal = value; currentCorruption = value; } }
    private int currentXP;
    private int nextLevelXP = 100;
    private int level;
    private int skillPoint;
    private Dictionary<string, SkillPoints> skills = new Dictionary<string, SkillPoints>();
    private Animator skillAnimator;

    [Header("SkillCosts")]
    public float energyVal;
    public float healVal;
    public float orbLaunchVal;
    public float orbJauntVal;
    public float doubleJumpVal;
    [Header("Effects")]
    public ParticleSystem levelUpEffect;
    public ParticleSystem healthEffect;
    public ParticleSystem xpEffect;
    public ParticleSystem energyEffect;
    [Header("Assign")]
    public Image corruptionColor;
    public GameObject skillPointLight;
    public Vector2 startPos;
    public TrailRenderer trailRenderer;
    public GameObject playerSoul;
    public Transform[] groundPoints;
    public LayerMask whatIsGround;
    [Header("Controls")]
    public float jumpForce;
    public float groundRadius;
    public bool immortalty;
    private bool immortal = false;
    public float immortalTime;

    [Header("Don't Bother")]
    public bool airControl;
    public bool trig = false;
    public bool abilityTrig = false;
    public float alphaLevel = 1;
    public float totalTime = 0;
    private Enemy enemy;
    private float attackCooldown = 4f;
    private bool canThrow = true;
    private SpriteRenderer spriteRenderer;
    private bool isCreated;
    private Camera cam;
    private Vector3 mousePosition;
    public bool isInCorruption = false;
    private InteractableObject interactableObject;
    private bool canInteract = true;

    [Header("Basic Attacks")]
    public bool LMB;
    public bool RMB;
    [Header("Orb Jaunt")]
    public bool OrbJaunt;
    [Header("Bulwark")]
    public bool Bulwark;
    public bool bulwarkIsActive = false;
    public float bulwarkVal;
    public float bulwarkDuration;
    public float bulwarkCooldown;
    public Transform bulwarkPrefab;
    public GameObject bulwarkIndicator;
    private bool berserkIsActive;
    [Header("Double Jump")]
    public bool DoubleJump = false;
    public bool canDoubleJump = true;
    [Header("Corruption")]
    public float corruptionDuration = 30f;
    public GameObject vaultingCircle;

    public Rigidbody2D MyRigidBody { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; set; }
    public override bool IsDead
    {
        get
        {
            if (healthStat.CurrentVal <= 0)
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

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        trailRenderer.sortingOrder = 2;
        startPos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        MyRigidBody = GetComponent<Rigidbody2D>();
        if (Wraith)
        {
            healthStat.Initialize();
            mentalityStat.Initialize();
            soulsStat.Initialize();
            XPStat.Initialize();
            corruptionStat.Initialize();
            XPStat.CurrentVal = currentXP;
            skills.Add("OrbBlast", new SkillPoints(1));
            skills.Add("OrbJaunt", new SkillPoints(1, "OrbBlast"));
            skills.Add("Bulwark", new SkillPoints(1, "OrbBlast"));
            skills.Add("DoubleJump", new SkillPoints(1, "OrbBlast"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!IsDead)
        {
            HandleInput();
            if (Wraith)
            {
                if (skillPoint > 0)
                {
                    skillPointLight.SetActive(true);
                }
                else
                {
                    skillPointLight.SetActive(false);
                }
            }
        }
        if (!isInTransition)
            return;


        transition += isShowing ? Time.deltaTime * (1 / duration) : -Time.deltaTime * (1 / duration);
        corruptionColor.color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1 , 0.25f), transition);

        if (transition > 1 || transition < 0)
            isInTransition = false;
    }

    void FixedUpdate()
    {
        if (!IsDead && !TakingDamage || TakingDamage && bulwarkIsActive)
        {
            float horizontal = Input.GetAxis("Horizontal");

            HandleMovement(horizontal);

            OnGround = IsGrounded();

            Flip(horizontal);

            HandleLayers();
        }

    }

    public void OnDead()
    {
        if (Dead != null)
        {
            Dead();
        }
    }

    void HandleMovement(float horizontal)
    {
        if (IsFalling && !IsJumping)
        {
            gameObject.layer = 11;
            MyAnimator.SetBool("land", true);
        }
        if (OnGround)
        {
            if (canDoubleJump == false)
            {
                MyAnimator.SetBool("land", true);
            }
            canDoubleJump = true;
        }
        else
        {
            MyAnimator.SetBool("land", false);
        }
        if (!Attack && (OnGround || airControl))
        {
            MyRigidBody.velocity = new Vector2(horizontal * moveSpeed, MyRigidBody.velocity.y);
        }
        if (Jump && MyRigidBody.velocity.y == 0)
        {
            MyRigidBody.AddForce(new Vector2(0, jumpForce));
        }

        MyAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    public void FootstepsAudio()
    {
        AudioManager.instance.PlaySound("FootstepTest", transform.position);
    }

    public void HandleInput()
    {
        if (trig == false)
        {
            if (Wraith)
            {
                if (Input.GetKeyDown(KeyCode.V) && LMB == true)
                {
                    Corruption();
                }
                if (Input.GetMouseButtonDown(0) && LMB == true)
                {
                    MyAnimator.SetTrigger("attack");
                }
                if (abilityTrig == false)
                {
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
                }

                if (Input.GetKeyDown(KeyCode.Space) && !IsFalling)
                {
                    MyAnimator.SetTrigger("jump");
                }
                if (Input.GetKeyDown(KeyCode.Space) && canDoubleJump == true && !OnGround && soulsStat.CurrentVal >= doubleJumpVal)
                {
                    soulsStat.CurrentVal -= doubleJumpVal;
                    MyAnimator.SetTrigger("doublejump");
                    MyRigidBody.velocity = new Vector2(MyRigidBody.velocity.x, 6.5f);
                    vaultingCircle.SetActive(true);
                    vaultingCircle.GetComponent<Animator>().SetTrigger("player_double_jump");
                    canDoubleJump = false;
                }
                if (Input.GetKeyDown(KeyCode.Alpha1) && Bulwark == true)
                {
                    CooldownManager.Instance.Bulwark();
                }
                if (Input.GetKeyDown(KeyCode.L))
                {
                    GainXP(nextLevelXP);
                    CurrentCorruption = 100;
                    mentalityStat.CurrentVal += 25;
                }
            }            
            if(interactableObject != null)
            {
                StartCoroutine(Interact());
            }
        }
    }

    IEnumerator Interact()
    {
        if (Input.GetKeyDown(KeyCode.E) && StaticInteractableBehaviour.canInteract && canInteract)
        {
            canInteract = false;
            interactableObject.Interact();
            yield return new WaitForSeconds(0.5f);
            canInteract = true;
        }
    }

    public void Lunge()
    {
        if (facingRight)
        {
            MyRigidBody.AddForce(transform.right * 50);
        }
        if (!facingRight)
        {
            MyRigidBody.AddForce(-transform.right * 50);
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
        if (MyRigidBody.velocity.y <= 0)
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

    public void RunTrailPositionChange()
    {
        trailRenderer.transform.localPosition = Vector3.Lerp(new Vector2(-0.1f, trailRenderer.transform.localPosition.y), new Vector2(-0.98f, trailRenderer.transform.localPosition.y), 1);
    }

    public void IdleTrailPositionChange()
    {
        trailRenderer.transform.localPosition = new Vector2(-0.1f, trailRenderer.transform.localPosition.y);
    }

    public override void MeleeAttack()
    {
        if (!isInCorruption)
        {
            base.MeleeAttack();
        }
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

    public void Corruption()
    {
        if (!isInCorruption && CooldownManager.Instance.corruptionCharges == 1)
        {
            corruptionColor.gameObject.SetActive(true);
            StartCoroutine(FadeCheck());
            abilityTrig = true;
            MyAnimator.SetLayerWeight(2, 1);
            MyAnimator.SetLayerWeight(3, 1);
            MyAnimator.SetTrigger("corruption");
            isInCorruption = true;
            StartCoroutine(CorruptionActive());
            CooldownManager.Instance.Corruption();
            trailRenderer.gameObject.SetActive(true);
        }
    }

    IEnumerator FadeCheck()
    {
        Fade(true, 1.25f);
        yield return new WaitForSeconds(corruptionDuration);
        Fade(false, 2f);
        StopCoroutine("FadeCheck");
    }

    public void DeCorruption()
    {
        corruptionColor.gameObject.SetActive(false);
        abilityTrig = false;
        trailRenderer.gameObject.SetActive(false);
        MyAnimator.SetTrigger("corruption_deform");
        CurrentCorruption = 0;
        isInCorruption = false;
    }

    public IEnumerator CorruptionActive()
    {
        if (isInCorruption)
        {
            yield return new WaitForSeconds(corruptionDuration);
            DeCorruption();
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
        if (CooldownManager.Instance.OrbLaunchIsAllowed && soulsStat.CurrentVal > 0)
        {
            CooldownManager.Instance.LaunchOrb();
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
        if (berserkIsActive)
        {
            if (!IsDead)
            {
                yield return null;
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
        if (other.tag == "EnemySword1")
        {
            healthStat.CurrentVal -= 5;
            StartCoroutine(TakeDamage());
        }
        if (other.tag == "EnemyOrb2")
        {
            healthStat.CurrentVal -= 10;
            StartCoroutine(TakeDamage());
        }
        if(other.tag == "Interactable")
        {
            interactableObject = other.GetComponent<InteractableObject>();
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Interactable")
        {
            interactableObject = null;
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
        if (other.gameObject.tag == "Health")
        {
            if (healthStat.CurrentVal < healthStat.MaxVal)
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
            GainXP(UnityEngine.Random.Range(11, 15));
            Destroy(Instantiate(xpEffect.gameObject, other.transform.position, Quaternion.identity) as GameObject, healthEffect.startLifetime);
            Destroy(other.gameObject);
        }
    }

    public void SpendSkillPoints(string skillName)
    {
        if (UIManager.Instance.isUpgrading == false)
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
                    case "DoubleJump":
                        DoubleJump = true;
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
            GetComponent<SkillPointHandler>().LevelUp(level);
        }
    }

    public void SetSkillAnimation(Animator skillAnimator)
    {
        this.skillAnimator = skillAnimator;
    }
}