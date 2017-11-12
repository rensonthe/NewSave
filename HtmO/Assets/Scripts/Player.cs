using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public delegate void DeadEventHandler();

[RequireComponent(typeof(Controller2D))]
public class Player : Character
{
    public bool test;

    private static Player instance;
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    public event DeadEventHandler Dead;

    [HideInInspector]
    public bool trig = false;

    public Stat healthStat;
    [SerializeField]
    private Stat staminaStat;

    public ParticleSystem deathEffect;
    public Image fadeImage;
    public Image circleyChargeImage;
    public Text circleyChargeText;

    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    [HideInInspector]
    public Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;
    private Animator myAnimator;

    Vector2 directionalInput;
    bool wallSliding;
    int wallDirX;
    public bool noClimb;

    private bool canRegen;
    private float regenTimer;

    private bool immortal = false;
    [SerializeField]
    private float immortalTime;

    [HideInInspector]
    public Vector2 startPos;

    [HideInInspector]
    public int circleyCharges;
    [HideInInspector]
    public bool canCircley;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;

    public override void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        controller = GetComponent<Controller2D>();
        base.Start();
        healthStat.Initialize();
        staminaStat.Initialize();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        print("Gravity: " + gravity + "  Jump Velocity: " + maxJumpVelocity);
        Cursor.visible = false;
    }

    void Update()
    {
        if (!TakingDamage && !IsDead)
        {
            HandleInput();
            CalculateVelocity();
            controller.Move(velocity * Time.deltaTime, directionalInput);
            regenTimer += Time.deltaTime;
        }
        if (!test)
        {
            if (transform.Find("Area_0_Bean"))
            {
                FreshBean.Instance.isBean = true;
            }
            else
            {
                FreshBean.Instance.isBean = false;
            }
        }
        if (circleyCharges >= 1)
        {
            circleyChargeText.text = circleyCharges.ToString();
            canCircley = true;
            circleyChargeImage.gameObject.SetActive(true);
        }
        else
        {
            canCircley = false;
            circleyChargeImage.gameObject.SetActive(false);
        }

        if (regenTimer >= 1.5f && staminaStat.CurrentVal != staminaStat.MaxVal)
        {
            canRegen = true;
            regenTimer = 0;
        }
        if (canRegen)
        {
            if (staminaStat.CurrentVal != staminaStat.MaxVal)
            {
                staminaStat.CurrentVal += Time.deltaTime * 4.5f;
            }
            else
            {
                canRegen = false;
            }
        }
        if (!trig)
        {
            HandleWallSliding();
            
            if (controller.collisions.above || controller.collisions.below)
            {
                if (controller.collisions.slidingDownMaxSlope)
                {
                    velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
                }
                else
                {
                    velocity.y = 0;
                }
            }
        }

        if (!isInTransition)
            return;


        transition += isShowing ? Time.deltaTime * (1 / duration) : -Time.deltaTime * (1 / duration);
        spriteRenderer.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, transition);
        fadeImage.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, transition);

        if (transition > 1 || transition < 0)
            isInTransition = false;

    }

    void FixedUpdate()
    {
        if (!trig)
        {
            float horizontal = Input.GetAxis("Horizontal");
            HandleMovement(horizontal);
            Flip(horizontal);
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
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        SetDirectionalInput(directionalInput);
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(1) && AbilityManager.Instance.fireball == true)
        {
            FireBall();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            AbilityManager.Instance.unlocked = true;
        }
    }

    private void FireBall()
    {
        if (CooldownManager.Instance.FireballIsAllowed)
        {
            CooldownManager.Instance.Fireball();
            myAnimator.SetTrigger("fireball");
        }
    }

    void Flip(float horizontal)
    {
        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
        {
            ChangeDirectionPlayer();
        }
    }

    public void SetActive()
    {
        trig = false;
        moveSpeed = 6;
    }

    public void SetInactive()
    {
        moveSpeed = 0;
        trig = true;
    }

    public void SetDirectionalInput (Vector2 input)
    {
        directionalInput = input;
    }

    public void OnJumpInputDown()
    {
        if (!trig && !noClimb)
        {
            if(staminaStat.CurrentVal != 0)
            {
                if (wallSliding)
                {
                    if (wallDirX == directionalInput.x)
                    {
                        velocity.x = -wallDirX * wallJumpClimb.x;
                        velocity.y = wallJumpClimb.y;
                        staminaStat.CurrentVal -= 1;
                        canRegen = false;
                        regenTimer = 0;
                    }
                    else if (directionalInput.x == 0)
                    {
                        velocity.x = -wallDirX * wallJumpOff.x;
                        velocity.y = wallJumpOff.y;
                        staminaStat.CurrentVal -= 1;
                        canRegen = false;
                        regenTimer = 0;
                    }
                    else
                    {
                        velocity.x = -wallDirX * wallLeap.x;
                        velocity.y = wallLeap.y;
                        staminaStat.CurrentVal -= 1;
                        canRegen = false;
                        regenTimer = 0;
                    }
                }
                if (controller.collisions.below)
                {
                    if (controller.collisions.slidingDownMaxSlope)
                    {
                        if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x))
                        {
                            velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
                            velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
                            staminaStat.CurrentVal -= 1;
                            canRegen = false;
                            regenTimer = 0;
                        }
                    }
                    else
                    {
                        velocity.y = maxJumpVelocity;
                        staminaStat.CurrentVal -= 1;
                        canRegen = false;
                        regenTimer = 0;
                    }
                }
            }
        }        
    }

    public void DoubleJump()
    {
        velocity.y = maxJumpVelocity;
    }

    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }

    void HandleWallSliding()
    {
        if (!noClimb)
        {
            wallDirX = (controller.collisions.left) ? -1 : 1;
            wallSliding = false;
            if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
            {
                wallSliding = true;

                if (velocity.y < -wallSlideSpeedMax)
                {
                    velocity.y = -wallSlideSpeedMax;
                }

                if (timeToWallUnstick > 0)
                {
                    velocityXSmoothing = 0;
                    velocity.x = 0;

                    if (directionalInput.x != wallDirX && directionalInput.x != 0)
                    {
                        timeToWallUnstick -= Time.deltaTime;
                    }
                    else
                    {
                        timeToWallUnstick = wallStickTime;
                    }
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }

            }
        }
    }

    void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
    }

    private IEnumerator IndicateImmortal()
    {
        while (immortal)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public override IEnumerator TakeDamage()
    {
        if (!immortal)
        {
            healthStat.CurrentVal -= 1;

            if (!IsDead)
            {
                if (transform.Find("Area_0_Bean"))
                {
                    FreshBean.Instance.StartCoroutine("Damaged");
                }
                MyAnimator.SetTrigger("damage");
                immortal = true;
                StartCoroutine(IndicateImmortal());
                yield return new WaitForSeconds(immortalTime);
                immortal = false;
            }
            else
            {
                StartCoroutine("FadeCheckOut");
                boxCollider2D.enabled = false;
                Destroy(Instantiate(deathEffect.gameObject, transform.position, Quaternion.identity) as GameObject, deathEffect.startLifetime);
                MyAnimator.SetTrigger("death");
                yield return null;
            }
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

    public IEnumerator FadeCheckIn()
    {
        Fade(true, 1f);
        fadeImage.gameObject.SetActive(false);
        StopCoroutine("FadeCheckIn");
        yield return null;
    }

    public IEnumerator FadeCheckOut()
    {
        fadeImage.gameObject.SetActive(true);
        Fade(false, 1f);
        StopCoroutine("FadeCheckOut");
        yield return null;
    }

    public virtual void OnTriggerStay2D(Collider2D other)
    {
        if (damageSources.Contains(other.tag))
        {
            StartCoroutine(TakeDamage());
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.tag == "Bounds")
        {
            immortal = false;
            healthStat.CurrentVal -= healthStat.MaxVal;
            TakeDamage();
        }
    }

    public override void Death()
    {
        OnTrigger.Instance.Died();
        StartCoroutine("FadeCheckIn");
        rb.velocity = Vector2.zero;
        boxCollider2D.enabled = true;
        MyAnimator.SetTrigger("idle");
        healthStat.CurrentVal = healthStat.MaxVal;
        transform.position = startPos;
    }

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

    public void GiveCharge()
    {
        circleyCharges++;
    }
}