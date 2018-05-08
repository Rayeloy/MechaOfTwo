using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{

    //esto es para el piloto

    float currentOverheat;
    public float maxOverheat = 100;
    float coolingOverheatTime = 0;
    public float coolingFrecuency = 1;
    public float coolingAmount = -10;//negative value always

    bool direction;//true=right, false=left;

    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    public float moveSpeed = 6;

    float gravity = -10;
    float jumpVelocity = 8;
    Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;

    bool volando = false;
    public float accVolar = 4f;
    public float maxVerticalVel = 15f;
    float startHeight;
    public float maxHeight = 6f;
    float deltaHeight;
    float flyingTime;
    public float flyOverheatFrecuency = 0.5f;
    public float flyOverheatAmount = 10f;

    WalkState currentWState;
    WalkState lastWState;
    enum WalkState
    {
        left,
        right,
        error
    }

    bool walking = false;
    public float stepDistance = 1f;
    public float stepVelocity = 5f;
    float startX;
    float currentXtraveled;


    void Start()
    {
        controller = GetComponent<Controller2D>();
        currentWState = WalkState.left;
        direction = true;
        //gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        //jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        //print("Gravity: " + gravity + " Jump Velovity: " + jumpVelocity);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!volando)
            {
                StartFly();
            }

        }
        if (Input.GetButtonUp("Jump"))
        {
            StopFly();
        }

        if (Input.GetButtonDown("LeftStep"))
        {
            print("leftstep");
            Walk(WalkState.left);
        }
        if (Input.GetButtonDown("RightStep"))
        {
            print("rightstep");
            Walk(WalkState.right);
        }

        if (controller.collisions.above || controller.collisions.below)
        {
            if (!volando)
            {
                velocity.y = 0;
            }

        }

        MovHorizontal();
        MovVertical();


        /*if(Input.GetKeyDown (KeyCode.Space) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }*/


        CoolOverheat();
        controller.Move(velocity * Time.deltaTime);
    }

    void Walk(WalkState step)
    {
        if (!volando && controller.collisions.below)
        {
            if (currentWState == WalkState.left)
            {
                if (step == WalkState.left)
                {
                    lastWState = currentWState;
                    currentWState = WalkState.error;
                    print("Step Error");
                }
                else
                {
                    //animación paso dcha
                    StartWalking();
                    currentWState = WalkState.right;
                    print("Step Right");
                }
            }
            else if (currentWState == WalkState.right)
            {
                if (step == WalkState.right)
                {
                    lastWState = currentWState;
                    currentWState = WalkState.error;
                    print("Step Error");
                }
                else
                {
                    //animación paso izda
                    StartWalking();
                    currentWState = WalkState.left;
                    print("Step Left");
                }
            }
        }
    }

    void StartWalking()
    {
        walking = true;
        currentXtraveled = 0;
        startX = transform.position.x;
    }

    void Walking()
    {
        if (walking)
        {
            if (direction)
            {
                velocity.x = stepVelocity;
            }
            else
            {
                velocity.x = -stepVelocity;
            }
            currentXtraveled = Mathf.Abs(transform.position.x - startX);
            if (currentXtraveled >= stepDistance)
            {
                walking = false;
                velocity.x = 0;
            }
        }
    }

    void MovVertical()
    {
        if (volando && currentOverheat < maxOverheat)
        {
            deltaHeight = transform.position.y - startHeight;
            flyingTime += Time.deltaTime;
            if (flyingTime >= flyOverheatFrecuency)
            {
                flyingTime = 0;
                IncreaseOverheat(flyOverheatAmount);
            }
            if (deltaHeight < startHeight + maxHeight)
            {
                if (velocity.y >= maxVerticalVel)
                {
                    velocity.y = maxVerticalVel;
                }
                else
                {
                    velocity.y += accVolar * Time.deltaTime;
                }
            }
            else
            {
                velocity.y = 0;
            }

        }
        else
        {
            if (currentOverheat >= maxOverheat && volando)
            {
                StopFly();
            }
            velocity.y += gravity * Time.deltaTime;
        }
    }

    void MovHorizontal()
    {
        if (volando)
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            float targetVelocityX = input.x * moveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        }
        else
        {
            Walking();
        }
    }

    void StartFly()
    {
        if (currentOverheat <= maxOverheat - 10)
        {
            volando = true;
            walking = false;
            startHeight = transform.position.y;
            IncreaseOverheat(10);
            flyingTime = 0;
        }
    }

    void StopFly()
    {
        if (volando)
        {
            volando = false;
            coolingOverheatTime = 0;
            velocity.x = 0;
        }
    }

    void CoolOverheat()//controls cooling when not flying or fucking up
    {
        if (!volando && currentOverheat>0)
        {
            coolingOverheatTime += Time.deltaTime;
            if (coolingOverheatTime >= coolingFrecuency)
            {
                IncreaseOverheat(coolingAmount);
                coolingOverheatTime = 0;
            }
        }
    }

    void IncreaseOverheat(float amount)
    {
        currentOverheat += amount;
        currentOverheat = Mathf.Clamp(currentOverheat, 0, maxOverheat);
        coolingOverheatTime = 0;
        print("CurrentOverheat: " + currentOverheat);
    }
}
