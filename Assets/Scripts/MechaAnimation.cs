using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaAnimation : MonoBehaviour
{

    Animator anim;
    int standByHash = Animator.StringToHash("Standby");
    bool standByBool;
    int waitingForStepHash = Animator.StringToHash("WaitingForStep");
    bool waitingForStep;

    //triggers
    int closeStepHash = Animator.StringToHash("CloseStep");
    bool closeStepBool = false;
    int farStepHash = Animator.StringToHash("FarStep");
    bool farStepBool = false;

    //states hash
    int standbyStateHash = Animator.StringToHash("Base Layer.standby");
    int stepCloseLegStateHash = Animator.StringToHash("Base Layer.Step_Close_Leg");
    int stepFarLegStateHash = Animator.StringToHash("Base Layer.Step_Far_Leg");


    float waitingStepTime = 0;
    public float maxWaitingStepTime = 1f;

    private void Awake()
    {
        anim = transform.GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateVariables();
        //anim.ResetTrigger(farStepHash);
        //anim.ResetTrigger(closeStepHash);
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.fullPathHash == standbyStateHash)
        {
            if (closeStepBool)
            {
                anim.ResetTrigger(farStepHash);
                anim.SetTrigger(closeStepHash);
            }
            else if(farStepBool)
            {
                anim.ResetTrigger(closeStepHash);
                anim.SetTrigger(farStepHash);
            }
        }
        else if (stateInfo.fullPathHash == stepCloseLegStateHash)
        {
            if (closeStepBool)
            {
                anim.ResetTrigger(farStepHash);
                anim.SetTrigger(closeStepHash);
            }
            else if (farStepBool)
            {
                anim.ResetTrigger(closeStepHash);
                anim.SetTrigger(farStepHash);
            }
        }
        else if (stateInfo.fullPathHash == stepFarLegStateHash)
        {
            if (closeStepBool)
            {
                anim.ResetTrigger(farStepHash);
                anim.SetTrigger(closeStepHash);
            }
            else if (farStepBool)
            {
                anim.ResetTrigger(closeStepHash);
                anim.SetTrigger(farStepHash);
            }
        }

        if (waitingForStep)
        {
            waitingStepTime += Time.deltaTime;
            if (waitingStepTime >= maxWaitingStepTime)
            {
                StopWaitingStep();
            }
        }
    }

    public void UpdateVariables()
    {
        closeStepBool = Player.instance.walking && ((Player.instance.direction && Player.instance.currentWState == Player.WalkState.right)
            || (!Player.instance.direction && Player.instance.currentWState == Player.WalkState.left));

        farStepBool = Player.instance.walking && ((Player.instance.direction && Player.instance.currentWState == Player.WalkState.left)
            || (!Player.instance.direction && Player.instance.currentWState == Player.WalkState.right));

        standByBool = !Player.instance.volando && !Player.instance.walking;
        anim.SetBool(standByHash, standByBool);
        anim.SetBool(waitingForStepHash, waitingForStep);
    }

    public void StartWaitingStep()
    {
        waitingForStep = true;
        waitingStepTime = 0;
    }

    public void StopWaitingStep()
    {
        UpdateVariables();
        waitingStepTime = 0;
        waitingForStep = false;
        Player.instance.currentWState = Player.WalkState.start;
    }

    /*public void PrintStates()
    {
        print("StandBy= " + standByBool + "; CloseStep= " + closeStepBool + "; FarStep= " + farStepBool);
    }*/
}
