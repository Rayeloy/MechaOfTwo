using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public static MainMenu instance;

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            StartButton();
        }
        if (Input.GetButtonDown("Cancel"))
        {
            ExitButton();
        }
    }
    private void Start()
    {
        Shizuka.instance.Play("MusicMenu");
    }
    public void StartButton()
    {
        Shizuka.instance.Play("ButtonSound");
        SceneManager.LoadScene("tutorial");
    }
    public void OptionsButton()
    {
        Shizuka.instance.Play("ButtonSound");
    }
    public void ExitButton()
    {
        Shizuka.instance.Play("ButtonSound");
        Application.Quit();
    }
}
