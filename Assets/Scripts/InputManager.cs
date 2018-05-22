using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public static InputManager instance;

    private void Awake()
    {
        instance = this;
    }
    public void KonoUpdate()
    {
        //----------------isAxisDown----------------
        if (isRightDown)
        {
            isRightDown = false;
        }
        if (isDownDown)
        {
            isDownDown = false;
        }
        if (isLeftDown)
        {
            isLeftDown = false;
        }
        if (isUpDown)
        {
            isUpDown = false;
        }
        if (isLTDown)
        {
            isLTDown = false;
        }
        if (isRTDown)
        {
            isRTDown = false;
        }
        //----------------isAxisUp----------------
        if (isRightUp)
        {
            isRightUp = false;
        }
        if (isDownUp)
        {
            isDownUp = false;
        }
        if (isLeftUp)
        {
            isLeftUp = false;
        }
        if (isUpUp)
        {
            isUpUp = false;
        }
        if (isLTUp)
        {
            isLTUp = false;
        }
        if (isRTUp)
        {
            isRTUp = false;
        }


        //----------------------------------------------------
        float auxAxis = Input.GetAxisRaw("WeaponFront");
        if (auxAxis >= 0.2f && isDPadRightFree)
        {
            isDPadRightFree = false;
            isRightDown = true;
            //print("isDpadRightFree= " + isDPadRightFree);
        }
        else if (auxAxis < 0.2f && auxAxis >-0.2f && !isDPadRightFree)
        {
            isDPadRightFree = true;
            isRightUp = true;
            //print("isDpadRightFree= " + isDPadRightFree);
        }

        if (auxAxis <= -0.2f && isDPadLeftFree)
        {
            isDPadLeftFree = false;
            isLeftDown = true;
            //print("isDPadLeftFree= " + isDPadLeftFree);
        }
        else if (auxAxis > -0.2f && auxAxis < 0.2f && !isDPadLeftFree)
        {
            isDPadLeftFree = true;
            isLeftUp = true;
            //print("isDPadLeftFree= " + isDPadLeftFree);
        }

        auxAxis = Input.GetAxisRaw("WeaponTop");
        if (auxAxis >= 0.2f && isDPadUpFree)
        {
            isDPadUpFree = false;
            isUpDown = true;
            //print("isDPadUpFree= " + isDPadUpFree);
        }
        else if (auxAxis < 0.2f && auxAxis > -0.2f && !isDPadUpFree)
        {
            isDPadUpFree = true;
            isUpUp = true;
            //print("isDPadUpFree= " + isDPadUpFree);
        }

        if (auxAxis <= -0.2f && isDPadDownFree)
        {
            isDPadDownFree = false;
            isDownDown = true;
            //print("isDPadDownFree= " + isDPadDownFree);
        }
        else if (auxAxis > -0.2f && auxAxis < 0.2f && !isDPadDownFree)
        {
            isDPadDownFree = true;
            isDownUp = true;
            //print("isDPadDownFree= " + isDPadDownFree);
        }

        auxAxis = Input.GetAxisRaw("Shoot");
        if (auxAxis >= 0.2f && isLTFree)
        {
            isLTFree = false;
            isLTDown = true;
            //print("isLTFree= " + isLTFree);
        }
        else if (auxAxis < 0.2f && auxAxis > -0.2f && !isLTFree)
        {
            isLTFree = true;
            isLTUp = true;
            //print("isLTFree= " + isLTFree);
        }

        if (auxAxis <= -0.2f && isRTFree)
        {
            isRTFree = false;
            isRTDown = true;
            //print("isRTFree= " + isRTFree);
        }
        else if (auxAxis > -0.2f && auxAxis < 0.2f && !isRTFree)
        {
            isRTFree = true;
            isRTUp = true;
            //print("isRTFree= " + isRTFree);
        }
    }
    public bool anyAxisDown
    {
        get
        {
            bool result = false;
            if(isLTDown || isRTDown|| isRightDown || isDownDown || isLeftDown || isUpDown)
            {
                result = true;
            }
            return result;
        }
        
    }
    [HideInInspector]
    public bool isLTFree = true, isRTFree = true, isDPadRightFree = true, isDPadDownFree = true, isDPadLeftFree = true, isDPadUpFree = true;
    [HideInInspector]
    public bool isLTDown = false, isRTDown = false, isRightDown = false, isDownDown = false, isLeftDown = false, isUpDown = false;
    [HideInInspector]
    public bool isLTUp = false, isRTUp = false, isRightUp = false, isDownUp = false, isLeftUp = false, isUpUp = false;
}
