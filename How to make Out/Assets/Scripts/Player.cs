using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Controller2D))]
public class Player : Character
{
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

    [HideInInspector]
    public bool trig = false;

    [SerializeField]
    private Stat healthStat;
    [SerializeField]
    private Stat staminaStat;

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
    [SerializeField]
    private GameObject fireballPrefab;

    Vector2 directionalInput;
    bool wallSliding;
    int wallDirX;
    public bool noClimb;

    private bool canRegen;
    private float regenTimer;

    public override void Start()
    {
        myAnimator = GetComponent<Animator>();
        base.Start();
        healthStat.Initialize();
        staminaStat.Initialize();
        controller = GetComponent<Controller2D>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        print("Gravity: " + gravity + "  Jump Velocity: " + maxJumpVelocity);
    }

    void Update()
    {
        HandleInput();
        CalculateVelocity();
        controller.Move(velocity * Time.deltaTime, directionalInput);
        regenTimer += Time.deltaTime;

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
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            myAnimator.SetTrigger("fireball");
            Fireball();
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            facingRight = false;
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            facingRight = true;
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

    public override IEnumerator TakeDamage()
    {
        yield return null;
    }

    public override bool IsDead
    {
        get
        {
            return health <= 0;
        }
    }

    public void Fireball()
    {
        if (facingRight)
        {
            GameObject tmp = (GameObject)Instantiate(fireballPrefab, transform.position, Quaternion.identity);
            tmp.GetComponent<Fireball>().Initialize(Vector2.right);
        }
        else
        {
            GameObject tmp = (GameObject)Instantiate(fireballPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, -180)));
            tmp.GetComponent<Fireball>().Initialize(Vector2.left);
        }
    }
}