using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaAnimation : MonoBehaviour {

    Animator anim;
    int standByHash = Animator.StringToHash("Standby");
    bool standByBool;
    int closeStepHash = Animator.StringToHash("CloseStep");
    bool closeStepBool;
    int farStepHash = Animator.StringToHash("FarStep");
    bool farStepBool;
    int waitingForStepHash = Animator.StringToHash("WaitingForStep");

    bool waitingForStep;
    float waitingStepTime = 0;
    public  float maxWaitingStepTime=1f;

    private void Awake()
    {
        anim = transform.GetComponent<Animator>();
    }

    private void Update()
    {
        standByBool = !Player.instance.volando && !Player.instance.walking;
        anim.SetBool(standByHash, standByBool);
        closeStepBool = Player.instance.walking && ((Player.instance.direction && Player.instance.currentWState == Player.WalkState.right) 
            || (!Player.instance.direction && Player.instance.currentWState == Player.WalkState.left)) ;

        anim.SetBool(closeStepHash, closeStepBool);

        farStepBool = Player.instance.walking && ((Player.instance.direction && Player.instance.currentWState == Player.WalkState.left)
            || (!Player.instance.direction && Player.instance.currentWState == Player.WalkState.right));
        anim.SetBool(farStepHash, farStepBool);

        anim.SetBool(waitingForStepHash, waitingForStep);

        if (waitingForStep)
        {
            waitingStepTime += Time.deltaTime;
            if(waitingStepTime>= maxWaitingStepTime)
            {
                StopWaitingStep();
            }
        }


        if (Input.GetKeyDown(KeyCode.T))
        {
            PrintStates();
        }
    }

    public void UpdateVariables()
    {
        standByBool = !Player.instance.volando && !Player.instance.walking;
        anim.SetBool(standByHash, standByBool);
        closeStepBool = Player.instance.walking && ((Player.instance.direction && Player.instance.currentWState == Player.WalkState.right)
            || (!Player.instance.direction && Player.instance.currentWState == Player.WalkState.left));

        anim.SetBool(closeStepHash, closeStepBool);

        farStepBool = Player.instance.walking && ((Player.instance.direction && Player.instance.currentWState == Player.WalkState.left)
            || (!Player.instance.direction && Player.instance.currentWState == Player.WalkState.right));
        anim.SetBool(farStepHash, farStepBool);

        anim.SetBool(waitingForStepHash, waitingForStep);
    }


    public void StartWaitingStep()
    {
        waitingForStep = true;
        waitingStepTime = 0;
    }

    public void StopWaitingStep()
    {
        waitingStepTime = 0;
        waitingForStep = false;
        Player.instance.currentWState = Player.WalkState.start;
    }

    public void PrintStates()
    {
        print("StandBy= " + standByBool + "; CloseStep= " + closeStepBool + "; FarStep= " + farStepBool);
    }
}
