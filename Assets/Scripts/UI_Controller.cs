using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Controller : MonoBehaviour
{
    public static UI_Controller instance;

    public GameObject comboInputs;
    public Image[] comboInputButtons;//length=4
    public Image[] plusSigns;//length=3;
    [Tooltip("0=A,1=B,2=X,3=Y,4=right,5=down,6=left,7=up,8=LB,9=RB,10=LT,11=RT")]
    public Sprite[] buttonSprites;//0=A,1=B,2=X,3=Y,4=right,5=down,6=left,7=up,8=LB,9=RB,10=LT,11=RT

    public Player player1;
    Weapons wepPlayer1;
    
    [Header("Overheat Bars")]
    public Transform overheatBarPilot;
    public Transform overheatBarGunner;
    public float xWhenFullPilot;
    public float xWhenEmptyPilot;
    float pilotOverheatBarDist;
    public float xWhenFullGunner;
    public float xWhenEmptyGunner;
    float gunnerOverheatBarDist;

    [Header("Combo Skill Bar")]
    public Transform comboSkillBar;
    public float xWhenFullComboSkill;
    public float xWhenEmptyComboSkill;
    float comboSkillBarDist;


    private void Awake()
    {
        instance = this;
        comboInputs.SetActive(false);
        wepPlayer1 = player1.transform.GetComponentInChildren<Weapons>();

        pilotOverheatBarDist = xWhenEmptyPilot - xWhenFullPilot;
        gunnerOverheatBarDist = xWhenEmptyGunner - xWhenFullGunner;
        overheatBarPilot.localPosition = new Vector3(xWhenEmptyPilot, 0, 0);
        overheatBarGunner.localPosition = new Vector3(xWhenEmptyGunner, 0, 0);

        comboSkillBarDist = xWhenEmptyComboSkill - xWhenFullComboSkill;
        comboSkillBar.localPosition = new Vector3(xWhenEmptyComboSkill, 0, 0);

        errorVeil.SetActive(false);

    }
    public void KonoUpdate()
    {
        if (Input.GetButtonDown("Start"))
        {
            if (gamePaused)
            {
                PauseMenuOff();
            }
            else
            {
                PauseMenuOn();
            }
        }
        UpdateComboSkill();
        ProcessErrorScreen();
    }

    public void UpdatePilotOverheat()
    {
        float overheatVal = player1.currentOverheat/ player1.maxOverheat;
        float currentX = xWhenEmptyPilot-(overheatVal * pilotOverheatBarDist);
        overheatBarPilot.localPosition = new Vector3(currentX,overheatBarPilot.localPosition.y,0);
        
    }
    public void UpdateGunnerOverheat()
    {
        float overheatVal = wepPlayer1.currentOverheat / wepPlayer1.maxOverHeat;
        float currentX = xWhenEmptyGunner-(overheatVal * gunnerOverheatBarDist);
        overheatBarGunner.localPosition = new Vector3(currentX, overheatBarGunner.localPosition.y, 0);
    }

    public void UpdateComboSkill()
    {
        float comboSkillVal = player1.timeComboSkillCD/player1.maxTimeComboSkillCD;
        float currentX = xWhenEmptyComboSkill - (comboSkillVal * comboSkillBarDist);
        comboSkillBar.localPosition = new Vector3(currentX,comboSkillBar.localPosition.y,0);
    }
    [Header("Error Veil")]
    public GameObject errorVeil;
    public float flickeringFrecuency;
    float flickeringTime;
    bool error = false;
    bool appearing = false;

    public void StartErrorScreen()
    {
        errorVeil.SetActive(true);
        error = true;
        appearing = true;
        flickeringTime = 0;
    }
    public void ProcessErrorScreen()
    {
        if (error)
        {
            flickeringTime += Time.deltaTime;
            if (flickeringTime >= flickeringFrecuency)
            {
                flickeringTime = 0;
                appearing = !appearing;
            }
            if (appearing)
            {
                float val = flickeringTime / flickeringFrecuency;
                float t = Mathf.Lerp(0, 1, val);
                Color c = errorVeil.GetComponent<SpriteRenderer>().color;
                Color c2 = new Color(c.r, c.g, c.b, t);
                errorVeil.GetComponent<SpriteRenderer>().color = c2;
            }
            else
            {
                float val = flickeringTime / flickeringFrecuency;
                float t = Mathf.Lerp(1, 0, val);
                Color c = errorVeil.GetComponent<SpriteRenderer>().color;
                Color c2 = new Color(c.r, c.g, c.b, t);
                errorVeil.GetComponent<SpriteRenderer>().color = c2;
            }
        } 
    }
    public void StopErrorScreen()
    {
        errorVeil.SetActive(false);
        error = false;
        flickeringTime = 0;
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

    //--------------------PAUSE MENU / GAME OVER MENU--------------------
    public GameObject veil;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    [HideInInspector]
    public bool gamePaused = false;

    void PauseMenuOn()
    {
        gamePaused = true;
        Time.timeScale = 0;
        veil.SetActive(true);
        pauseMenu.SetActive(true);
    }

    void PauseMenuOff()
    {
        gamePaused = false;
        Time.timeScale = 1;
        veil.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void GameOverMenuOn()
    {
        Time.timeScale = 0;
        gameOverMenu.SetActive(true);
    }

    public void ResumeButton()
    {
        Shizuka.instance.Play("ButtonSound");
        PauseMenuOff();
    }
    public void BackToMenuButton()
    {
        Shizuka.instance.Play("ButtonSound");
        Shizuka.instance.Stop("MusicGameplay");
        SceneManager.LoadScene("MainMenu");
    }
    public void ExitButton()
    {
        Shizuka.instance.Play("ButtonSound");
        Application.Quit();
    }
    public void RestartButton()
    {
        Shizuka.instance.Play("ButtonSound");
        SceneManager.LoadScene("1");
    }
}
