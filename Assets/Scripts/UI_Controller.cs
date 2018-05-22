using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
    public static UI_Controller instance;

    public GameObject comboInputs;
    public Image[] comboInputButtons;//length=4
    public Image[] plusSigns;//length=3;
    [Tooltip("0=A,1=B,2=X,3=Y,4=right,5=down,6=left,7=up,8=LB,9=RB,10=LT,11=RT")]
    public Sprite[] buttonSprites;//0=A,1=B,2=X,3=Y,4=right,5=down,6=left,7=up,8=LB,9=RB,10=LT,11=RT
    private void Awake()
    {
        instance = this;
        comboInputs.SetActive(false);
    }
    public void ShowComboInput(string[] pilotInput, string[] gunnerInput)
    {
        comboInputs.SetActive(true);
        foreach(Image im in comboInputButtons)
        {
            im.enabled = true;
        }
        foreach(Image im in plusSigns)
        {
            im.enabled = true;
        }

        AdjustButtonScale(0, pilotInput[0]);
        AdjustButtonScale(1, gunnerInput[0]);
        AdjustButtonScale(2, pilotInput[1]);
        AdjustButtonScale(3, gunnerInput[1]);

        ChangeButtonSprite(0,pilotInput[0]);
        ChangeButtonSprite(1, gunnerInput[0]);
        ChangeButtonSprite(2, pilotInput[1]);
        ChangeButtonSprite(3, gunnerInput[1]);
    }

    void AdjustButtonScale(int buttonIndex, string buttonType)
    {
        if(buttonType=="RB" || buttonType == "LB")
        {
            comboInputButtons[buttonIndex].GetComponent<RectTransform>().localScale = new Vector3(1, 0.5f, 1);
        }
        else
        {
            comboInputButtons[buttonIndex].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }

    void ChangeButtonSprite(int buttonIndex, string buttonType)
    {
        switch (buttonType)
        {
            case "Jump":
                comboInputButtons[buttonIndex].sprite = buttonSprites[0];
                break;
            case "RightStep":
                comboInputButtons[buttonIndex].sprite = buttonSprites[1];
                break;
            case "X":
                comboInputButtons[buttonIndex].sprite = buttonSprites[2];
                break;
            case "LeftStep":
                comboInputButtons[buttonIndex].sprite = buttonSprites[3];
                break;
            case "WeaponFront":
                comboInputButtons[buttonIndex].sprite = buttonSprites[4];
                break;
            case "WeaponBottom":
                comboInputButtons[buttonIndex].sprite = buttonSprites[5];
                break;
            case "WeaponRear":
                comboInputButtons[buttonIndex].sprite = buttonSprites[6];
                break;
            case "WeaponTop":
                comboInputButtons[buttonIndex].sprite = buttonSprites[7];
                break;
            case "LB":
                comboInputButtons[buttonIndex].sprite = buttonSprites[8];
                break;
            case "RB":
                comboInputButtons[buttonIndex].sprite = buttonSprites[9];
                break;
            case "Shoot":
                comboInputButtons[buttonIndex].sprite = buttonSprites[10];
                break;
            case "RT":
                comboInputButtons[buttonIndex].sprite = buttonSprites[11];
                break;

        }
    }

    public void DisableButtonImage(int n)
    {
        switch (n)
        {
            case 0:
                comboInputButtons[0].enabled = false;
                plusSigns[0].enabled = false;
                break;
            case 1:
                comboInputButtons[1].enabled = false;
                plusSigns[1].enabled = false;
                break;
            case 2:
                comboInputButtons[2].enabled = false;
                plusSigns[2].enabled = false;
                break;
            case 3:
                comboInputButtons[3].enabled = false;
                break;
        }
    }
}
