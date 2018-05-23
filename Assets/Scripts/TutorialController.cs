using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour {

    public GameObject enemigo;
    public GameObject mechaStandby;
    public GameObject clases;
    public GameObject controles;
    public GameObject ciudad;
    public GameObject overheatBars;
    public GameObject comboSkillBar;

    public float mechaStandbyTime;
    public float clasesTime;
    public float controlesTime;
    public float ciudadTime;
    public float overheatBarsTime;
    public float comboSkillBarTime;

    float timeline = 0;
    int paso = 0;

    private void Start()
    {
        Shizuka.instance.Stop("MusicMenu");
        Shizuka.instance.Play("MusicTurorial");
        Shizuka.instance.Play("VozTutorial");
        timeline = 0;
        paso = 0;
        enemigo.SetActive(true);
        mechaStandby.SetActive(false);
        clases.SetActive(false);
        controles.SetActive(false);
        ciudad.SetActive(false);
        overheatBars.SetActive(false);
        comboSkillBar.SetActive(false);

    }

    private void Update()
    {
        timeline += Time.deltaTime;
        if (timeline >= mechaStandbyTime && paso==0)
        {
            enemigo.SetActive(false);
            mechaStandby.SetActive(true);
            paso++;
        }
        if (timeline >= clasesTime && paso == 1)
        {
            mechaStandby.SetActive(false);
            clases.SetActive(true);
            paso++;
        }
        if (timeline >= controlesTime && paso == 2)
        {
            clases.SetActive(false);
            controles.SetActive(true);
            paso++;
        }
        if (timeline >= ciudadTime && paso == 3)
        {
            controles.SetActive(false);
            ciudad.SetActive(true);
            paso++;
        }
        if (timeline >= overheatBarsTime && paso == 4)
        {
            ciudad.SetActive(false);
            overheatBars.SetActive(true);
            paso++;
        }
        if (timeline >= comboSkillBarTime && paso == 5)
        {
            overheatBars.SetActive(false);
            comboSkillBar.SetActive(true);
            paso++;
        }
        if (!Shizuka.instance.IsPlaying("VozTutorial") || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Start"))
        {
            //FUNDIDO EN NEGRO
            Shizuka.instance.Stop("MusicTutorial");
            Shizuka.instance.Stop("VozTutorial");
            SceneManager.LoadScene("1");
        }
    }
}
