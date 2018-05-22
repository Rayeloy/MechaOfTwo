using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaAnimation : MonoBehaviour
{
    public static MechaAnimation instance;

    Animator anim;
    int standByHash = Animator.StringToHash("Standby");
    bool standByBool;
    int waitingForStepHash = Animator.StringToHash("WaitingForStep");
    bool waitingForStep;

    //bools
    int closeStepHash = Animator.StringToHash("CloseStep");
    bool closeStepBool = false;
    int farStepHash = Animator.StringToHash("FarStep");
    bool farStepBool = false;
    int hasChangedStateHash = Animator.StringToHash("HasChangedState");
    int framesPassedHash = Animator.StringToHash("FramesPassed");
    [HideInInspector]
    public int framesPassed = 0;
    int errorBoolHash = Animator.StringToHash("Error");
    bool errorBool = false;

    //states hash
    int standbyStateHash = Animator.StringToHash("Base Layer.standby");
    int stepCloseLegStateHash = Animator.StringToHash("Base Layer.Step_Close_Leg");
    int stepFarLegStateHash = Animator.StringToHash("Base Layer.Step_Far_Leg");


    float waitingStepTime = 0;
    public float maxWaitingStepTime = 1f;

    private void Awake()
    {
        instance = this;
        anim = transform.GetComponent<Animator>();
    }

    public void KonoUpdate()
    {
        //print(hasChangedState);
        UpdateVariables();
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (waitingForStep)
        {
            waitingStepTime += Time.deltaTime;
            if (waitingStepTime >= maxWaitingStepTime)
            {
                StopWaitingStepTimeOut();
            }
        }
    }

    public void SetCloseStepBool(bool val)
    {
        closeStepBool = val;
        print("CloseStepBool = " + val + "; FramesPassed = " + framesPassed);
    }

    public void SetFarStepBool(bool val)
    {
        farStepBool = val;
        print("FarStepBool = " + val+"; FramesPassed = "+framesPassed);
    }

    public void SetErrorBool(bool val)
    {
        errorBool = val;
        print("ErrorBool = " + val);
    }

    public void UpdateVariables()
    {

        if(closeStepBool && framesPassed >= 1)
        {
            SetCloseStepBool(false);
        }
        if (farStepBool && framesPassed >= 1)
        {
            SetFarStepBool(false);
        }
        errorBool = Player.instance.currentWState == Player.WalkState.error;

        anim.SetBool(closeStepHash, closeStepBool);
        anim.SetBool(farStepHash, farStepBool);
        anim.SetBool(hasChangedStateHash, hasChangedState);
        anim.SetBool(errorBoolHash, errorBool);

        standByBool = !Player.instance.volando && !Player.instance.walking && Player.instance.currentWState!=Player.WalkState.error;
        anim.SetBool(standByHash, standByBool);
        anim.SetBool(waitingForStepHash, waitingForStep);

        if (framesPassed == 0)
        {
            framesPassed++;
        }


    }

    public void StartWaitingStep()
    {
        waitingForStep = true;
        waitingStepTime = 0;
    }

    public void StopWaitingStep()
    {
        //UpdateVariables();
        waitingStepTime = 0;
        waitingForStep = false;
    }
    public void StopWaitingStepTimeOut()
    {
        waitingStepTime = 0;
        waitingForStep = false;
        Player.instance.currentWState = Player.WalkState.start;
    }

    /*public void PrintStates()
    {
        print("StandBy= " + standByBool + "; CloseStep= " + closeStepBool + "; FarStep= " + farStepBool);
    }*/


    int lastState;
    bool hasChangedState
    {
        get
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.fullPathHash == lastState)
            {
                return false;
            }
            else
            {
                lastState = stateInfo.fullPathHash;
                return true;
            }

        }
    }


}
