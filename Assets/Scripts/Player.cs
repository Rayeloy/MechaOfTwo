using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//este script es para el piloto y para el mecha en general
[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    static public Player instance;

    [HideInInspector]
    public Controller2D controller;

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

    public void KonoUpdate()
    {
        //CheckForAxisUp();
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
                print("Step Left Input");
                Walk(WalkState.left);
            }
            if (Input.GetButtonDown("RightStep"))
            {
                print("Step Right Inpy");
                Walk(WalkState.right);
            }
            if (Input.GetButton("LB") && Input.GetButton("RB"))
            {
                StartComboSkill();
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
        else//stoppu=true
        {
            if (doingCombo)
            {
                if (!GameController.instance.destroyAllEnemiesAnim)
                {
                    inputComboTime += Time.deltaTime;
                }

                if (inputComboTime >= inputComboMaxTime)
                {
                    FailComboSkill();
                }
                else
                {
                    InputComboProcess();
                }
            }
        }

    }

    void Walk(WalkState step)
    {
        if (!volando && controller.collisions.below && !walking)
        {
            //print("currentWState= " + currentWState);
            switch (currentWState)
            {
                case WalkState.start:
                    if (step == WalkState.left)
                    {
                        currentWState = WalkState.left;
                        StartWalking();
                        print("Step Left");
                    }
                    else if (step == WalkState.right)
                    {
                        currentWState = WalkState.right;
                        StartWalking();
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
                        currentWState = WalkState.right;
                        StartWalking();
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
                        currentWState = WalkState.left;
                        StartWalking();
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
        //myMechaAnim.UpdateVariables();
        myMechaAnim.StopWaitingStep();
        if((Player.instance.direction && Player.instance.currentWState == Player.WalkState.right)
            || (!Player.instance.direction && Player.instance.currentWState == Player.WalkState.left))
        {
            myMechaAnim.framesPassed = 0;
            myMechaAnim.SetCloseStepBool(true);

        }
        else if(((Player.instance.direction && Player.instance.currentWState == Player.WalkState.left)
            || (!Player.instance.direction && Player.instance.currentWState == Player.WalkState.right)))
        {
            myMechaAnim.framesPassed = 0;
            myMechaAnim.SetFarStepBool(true);

        }

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
        myMechaAnim.StopWaitingStep();
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
                currentWState = WalkState.start;
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

    //-------------------COMBO SKILL-------------------
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
        gameObject.GetComponentInChildren<Weapons>().stoppu = true;
        inputComboTime = 0;
        doingCombo = true;
        GameController.instance.StopAllEnemies();
        GameController.instance.playing = false;
        comboStep = 0;
        next = false;

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
        UI_Controller.instance.ShowComboInput(pilotInputs, gunnerInputs);

        print(pilotInputs);
        print(gunnerInputs);
    }

    int comboStep = 0;
    bool next = false;
    void ICPNext(int i)//InputComboProcess Next
    {
        //quitar botón 1 de pantalla
        UI_Controller.instance.DisableButtonImage(i);
        comboStep++;
        next = false;
    }
    void InputComboProcess()
    {
        switch (comboStep)
        {
            case 0:
                if (pilotInputs[0] == "RT")
                {
                    if (InputManager.instance.isRTDown)
                    {
                        print("RTDOWN");
                        next = true;
                    }
                    else if ((Input.anyKeyDown || InputManager.instance.anyAxisDown) && InputManager.instance.isRTFree)
                    {
                        FailComboSkill();
                    }
                    if (next && InputManager.instance.isRTUp)
                    {
                        ICPNext(0);
                    }
                }
                else
                {
                    print("Combo step 0, normal button");
                    if (Input.GetButtonDown(pilotInputs[0]))
                    {
                        ICPNext(0);
                    }
                    else if ((Input.anyKeyDown || InputManager.instance.anyAxisDown) && !Input.GetButtonDown(pilotInputs[0]))
                    {
                        FailComboSkill();
                    }
                }
                break;
            case 1:
                switch (gunnerInputs[0])
                {
                    case "Shoot":
                        if (InputManager.instance.isLTDown)
                        {
                            next = true;
                        }
                        else if ((Input.anyKeyDown || InputManager.instance.anyAxisDown) && InputManager.instance.isLTFree)
                        {
                            FailComboSkill();
                        }
                        if (next && InputManager.instance.isLTUp)
                        {
                            ICPNext(1);
                        }
                        break;
                    case "WeaponFront":
                        if (InputManager.instance.isRightDown)
                        {
                            next = true;
                        }
                        else if ((Input.anyKeyDown || InputManager.instance.anyAxisDown) && InputManager.instance.isDPadRightFree)
                        {
                            FailComboSkill();
                        }
                        if (next && InputManager.instance.isRightUp)
                        {
                            ICPNext(1);
                        }
                        break;
                    case "WeaponRear":
                        if (InputManager.instance.isLeftDown)
                        {
                            next = true;
                        }
                        else if ((Input.anyKeyDown || InputManager.instance.anyAxisDown) && InputManager.instance.isDPadLeftFree)
                        {
                            FailComboSkill();
                        }
                        if (next && InputManager.instance.isLeftUp)
                        {
                            ICPNext(1);
                        }
                        break;
                    case "WeaponTop":
                        if (InputManager.instance.isUpDown)
                        {
                            next = true;
                        }
                        else if ((Input.anyKeyDown || InputManager.instance.anyAxisDown) && InputManager.instance.isDPadUpFree)
                        {
                            FailComboSkill();
                        }
                        if (next && InputManager.instance.isUpUp)
                        {
                            ICPNext(1);
                        }
                        break;
                    case "WeaponBottom":
                        if (InputManager.instance.isDownDown)
                        {
                            next = true;
                        }
                        else if ((Input.anyKeyDown || InputManager.instance.anyAxisDown) && InputManager.instance.isDPadDownFree)
                        {
                            FailComboSkill();
                        }
                        if (next && InputManager.instance.isDownUp)
                        {
                            ICPNext(1);
                        }
                        break;
                    default:
                        if (Input.GetButtonDown(gunnerInputs[0]))
                        {
                            ICPNext(1);
                        }
                        else if ((Input.anyKeyDown || InputManager.instance.anyAxisDown) && !Input.GetButtonDown(gunnerInputs[0]))
                        {
                            FailComboSkill();
                        }
                        break;
                }
                break;
            case 2:
                if (pilotInputs[1] == "RT")
                {
                    if (InputManager.instance.isRTDown)
                    {
                        next = true;
                    }
                    else if ((Input.anyKeyDown || InputManager.instance.anyAxisDown) && InputManager.instance.isRTFree)
                    {
                        FailComboSkill();
                    }
                    if (next && InputManager.instance.isRTUp)
                    {
                        ICPNext(2);
                    }
                }
                else
                {
                    print("Combo step 0, normal button");
                    if (Input.GetButtonDown(pilotInputs[1]))
                    {
                        ICPNext(2);
                    }
                    else if ((Input.anyKeyDown || InputManager.instance.anyAxisDown) && !Input.GetButtonDown(pilotInputs[1]))
                    {
                        FailComboSkill();
                    }
                }
                break;
            case 3:
                switch (gunnerInputs[1])
                {
                    case "Shoot":
                        if (InputManager.instance.isLTDown)
                        {
                            next = true;
                        }
                        else if ((Input.anyKeyDown || InputManager.instance.anyAxisDown) && InputManager.instance.isLTFree)
                        {
                            FailComboSkill();
                        }
                        if (next && InputManager.instance.isLTUp)
                        {
                            cSkillSuccess = true;
                            EndComboSkill();
                        }
                        break;
                    case "WeaponFront":
                        if (InputManager.instance.isRightDown)
                        {
                            next = true;
                        }
                        else if ((Input.anyKeyDown || InputManager.instance.anyAxisDown) && InputManager.instance.isDPadRightFree)
                        {
                            FailComboSkill();
                        }
                        if (next && InputManager.instance.isRightUp)
                        {
                            cSkillSuccess = true;
                            EndComboSkill();
                        }
                        break;
                    case "WeaponRear":
                        if (InputManager.instance.isLeftDown)
                        {
                            next = true;
                        }
                        else if ((Input.anyKeyDown || InputManager.instance.anyAxisDown) && InputManager.instance.isDPadLeftFree)
                        {
                            FailComboSkill();
                        }
                        if (next && InputManager.instance.isLeftUp)
                        {
                            cSkillSuccess = true;
                            EndComboSkill();
                        }
                        break;
                    case "WeaponTop":
                        if (InputManager.instance.isUpDown)
                        {
                            next = true;
                        }
                        else if ((Input.anyKeyDown || InputManager.instance.anyAxisDown) && InputManager.instance.isDPadUpFree)
                        {
                            FailComboSkill();
                        }
                        if (next && InputManager.instance.isUpUp)
                        {
                            cSkillSuccess = true;
                            EndComboSkill();
                        }
                        break;
                    case "WeaponBottom":
                        if (InputManager.instance.isDownDown)
                        {
                            next = true;
                        }
                        else if ((Input.anyKeyDown || InputManager.instance.anyAxisDown) && InputManager.instance.isDPadDownFree)
                        {
                            FailComboSkill();
                        }
                        if (next && InputManager.instance.isDownUp)
                        {
                            cSkillSuccess = true;
                            EndComboSkill();
                        }
                        break;
                    default:
                        if (Input.GetButtonDown(gunnerInputs[1]))
                        {
                            cSkillSuccess = true;
                            EndComboSkill();
                        }
                        else if ((Input.anyKeyDown || InputManager.instance.anyAxisDown) && !Input.GetButtonDown(gunnerInputs[1]))
                        {
                            FailComboSkill();
                        }
                        break;
                }
                break;

        }
    }

    void FailComboSkill()
    {
        print("COMBO FAILED");
        print("pilotInputs[0]= " + pilotInputs[0]);
        cSkillSuccess = false;
        EndComboSkill();
    }
    void EndComboSkill()
    {
        if (cSkillSuccess)//DO COMBO SKILL
        {
            print("COMBO SUCCEDED");
            UI_Controller.instance.comboInputs.SetActive(false);
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
        gameObject.GetComponentInChildren<Weapons>().stoppu = false;
        GameController.instance.ResumeAllEnemies();
        doingCombo = false;
        //DISABLE UI
        UI_Controller.instance.comboInputs.SetActive(false);
    }

    void CheckForAxisDown()
    {
        if (InputManager.instance.isRTDown)
        {
            print("RTDown");
        }
        if (InputManager.instance.isLTDown)
        {
            print("LTDown");
        }
        if (InputManager.instance.isRightDown)
        {
            print("RightDown");
        }
        if (InputManager.instance.isLeftDown)
        {
            print("LeftDown");
        }
        if (InputManager.instance.isDownDown)
        {
            print("DownDown");
        }
        if (InputManager.instance.isUpDown)
        {
            print("UpDown");
        }
    }
    void CheckForAxisUp()
    {
        if (InputManager.instance.isRTUp)
        {
            print("RTUp");
        }
        if (InputManager.instance.isLTUp)
        {
            print("LTUp");
        }
        if (InputManager.instance.isRightUp)
        {
            print("RightUp");
        }
        if (InputManager.instance.isLeftUp)
        {
            print("LeftUp");
        }
        if (InputManager.instance.isDownUp)
        {
            print("DownUp");
        }
        if (InputManager.instance.isUpUp)
        {
            print("UpUp");
        }
    }
}
