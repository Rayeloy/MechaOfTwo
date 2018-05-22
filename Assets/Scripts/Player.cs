using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//este script es para el piloto y para el mecha en general
[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    static public Player instance;

    public SpriteRenderer[] weapons;
    public SpriteRenderer mecha;

    float currentOverheat;
    public float maxOverheat = 100;
    float coolingOverheatTime = 0;
    public float coolingFrecuency = 1;
    public float coolingAmount = -10;//negative value always

    [HideInInspector]
    public bool direction = true;//true=right, false=left;
    public Transform turnAround;
    public Transform turnAroundWeapons;

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

    [HideInInspector]
    public bool volando = false;
    public float accVolar = 4f;
    public float maxVerticalVel = 15f;
    float startHeight;
    public float maxHeight = 6f;
    float deltaHeight;
    float flyingTime;
    public float flyOverheatFrecuency = 0.5f;
    public float flyOverheatAmount = 10f;

    [HideInInspector]
    public WalkState currentWState;
    WalkState lastWState;
    public enum WalkState
    {
        start,
        left,
        right,
        error
    }

    [HideInInspector]
    public bool walking = false;
    public float stepDistance = 1f;
    public float stepVelocity = 5f;
    public float maxStepDelayTime = 0.1f;
    float stepDelayTime = 0;
    bool stepDelay = false;
    float startX;
    float currentXtraveled;
    [HideInInspector]
    public bool stoppu = false;

    public MechaAnimation myMechaAnim;

    private void Awake()
    {
        instance = this;
        direction = true;
    }

    void Start()
    {
        controller = GetComponent<Controller2D>();
        currentWState = WalkState.start;
        //gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        //jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        //print("Gravity: " + gravity + " Jump Velovity: " + jumpVelocity);
    }

    private void Update()
    {
        if (!stoppu)
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

            ProcessError();
            ManageDir();
            MovHorizontal();
            MovVertical();


            /*if(Input.GetKeyDown (KeyCode.Space) && controller.collisions.below)
            {
                velocity.y = jumpVelocity;
            }*/


            CoolOverheat();
            controller.Move(velocity * Time.deltaTime);
        }

    }

    void Walk(WalkState step)
    {
        if (!volando && controller.collisions.below && !walking)
        {
            print("currentWState= " + currentWState);
            switch (currentWState)
            {
                case WalkState.start:
                    if (step == WalkState.left)
                    {
                        StartWalking();
                        currentWState = WalkState.left;
                        print("Step Left");
                    }
                    else if (step == WalkState.right)
                    {
                        StartWalking();
                        currentWState = WalkState.right;
                        print("Step Right");
                    }
                    break;
                case WalkState.left:
                    if (step == WalkState.left)
                    {
                        StartError();
                    }
                    else
                    {
                        //animación paso dcha
                        StartWalking();
                        currentWState = WalkState.right;
                        print("Step Right");
                    }
                    break;
                case WalkState.right:

                    if (step == WalkState.right)
                    {
                        StartError();
                    }
                    else
                    {
                        //animación paso izda
                        StartWalking();
                        currentWState = WalkState.left;
                        print("Step Left");
                    }
                    break;
            }
        }
    }

    void StartWalking()
    {
        walking = true;
        currentXtraveled = 0;
        startX = transform.position.x;
        stepDelay = true;
        stepDelayTime = 0;
        myMechaAnim.UpdateVariables();
        myMechaAnim.StopWaitingStep();

    }

    void Walking()
    {
        if (walking)
        {
            if (!stepDelay)//ACABO DELAY, nos movemos
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
                if (currentXtraveled >= stepDistance || (controller.collisions.left && !direction) || (controller.collisions.right && direction))
                {
                    walking = false;
                    velocity.x = 0;
                    myMechaAnim.StartWaitingStep();
                }
            }
            else//ESPERANDO DELAY
            {
                stepDelayTime += Time.deltaTime;
                if (stepDelayTime >= maxStepDelayTime)
                {
                    stepDelayTime = 0;
                    stepDelay = false;
                }
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
            if (deltaHeight < maxHeight)
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
            Vector2 input = new Vector2(Input.GetAxisRaw("jHorizontalD"), Input.GetAxisRaw("jVerticalD"));
            float targetVelocityX = input.x * moveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        }
        else
        {
            Walking();
        }
    }

    void ManageDir()
    {
        if (!walking)
        {
            float dir = Input.GetAxisRaw("jHorizontalD");
            switch (direction)
            {
                case true:
                    if (dir < -0.4f)
                    {
                        direction = false;
                        //GIRAR PERSONAJE A LA IZDA
                        turnAround.rotation = Quaternion.Euler(0, 180, 0);
                        turnAroundWeapons.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    break;
                case false:
                    if (dir > 0.4f)
                    {
                        direction = true;
                        //GIRAR PERSONAJE A LA DCHA
                        turnAround.rotation = Quaternion.Euler(0, 0, 0);
                        turnAroundWeapons.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    break;
            }
            //OrganizeLayers();
        }
    }
    //useless
    void OrganizeLayers()
    {
        if (direction)
        {
            mecha.sortingOrder = 1;
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].sortingOrder = 0;
            }
        }
        else
        {
            mecha.sortingOrder = 0;
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].sortingOrder = 1;
            }
        }
    }

    void StartFly()
    {
        print("current velocity= " + velocity);
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

    public float maxErrorTime = 1;
    float errorTime = -1;
    void StartError()
    {
        lastWState = currentWState;
        currentWState = WalkState.error;
        errorTime = 0;
        print("Step Error");
    }

    void ProcessError()
    {
        if (errorTime >= 0)
        {
            errorTime += Time.deltaTime;
            if (errorTime >= maxErrorTime)
            {
                errorTime = -1;
                currentWState = lastWState;
                print("End Error");
            }
        }
    }

    void CoolOverheat()//controls cooling when not flying or fucking up
    {
        if (!volando && currentOverheat > 0)
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

    bool cSkillSuccess = false;
    [HideInInspector]
    public bool doingCombo = false;
    public float inputComboMaxTime = 10;
    float inputComboTime = 0;
    string[] allPilotInputs = { "RightStep", "X", "LeftStep", "Jump", "RB", "RT" };//B,X,Y,A,RB,RT
    string[] allGunnerInputs = { "WeaponFront", "WeaponRear", "WeaponTop", "WeaponBottom", "LB", "Shoot", };//left,right,up,down,LB,LT
    string[] pilotInputs = new string[2];
    string[] gunnerInputs = new string[2];
    void StartComboSkill()
    {
        stoppu = true;
        inputComboTime = 0;
        doingCombo = true;
        GameController.instance.StopAllEnemies();
        GameController.instance.playing = false;
        comboStep = 0;
        //2 random buttons for pilot and gunner:
        int r = Random.Range(0, allPilotInputs.Length);
        pilotInputs[0] = allPilotInputs[r];
        r = Random.Range(0, allPilotInputs.Length);
        pilotInputs[1] = allPilotInputs[r];

        r = Random.Range(0, allGunnerInputs.Length);
        gunnerInputs[0] = allGunnerInputs[r];
        r = Random.Range(0, allGunnerInputs.Length);
        gunnerInputs[1] = allGunnerInputs[r];
        //MOSTRAR BOTONES EN PANTALLA
    }

    int comboStep = 0;
    void InputComboProcess()
    {
        switch (comboStep)
        {
            case 0:
                if (Input.GetButtonDown(pilotInputs[0]))
                {
                    //quitar botón 1 de pantalla
                    comboStep++;
                }
                else if (Input.anyKeyDown)
                {
                    cSkillSuccess = false;
                    EndComboSkill();
                }
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;

        }
    }

    void EndComboSkill()
    {
        if (cSkillSuccess)//DO COMBO SKILL
        {
            //combo skill animation
            GameController.instance.DestroyAllEnemiesAnimStart();
        }
        else//error
        {
            StopComboSkill();
            StartError();
        }
    }

    public void StopComboSkill()
    {
        GameController.instance.playing = true;
        stoppu = false;
        GameController.instance.ResumeAllEnemies();
        doingCombo = false;
    }

}
