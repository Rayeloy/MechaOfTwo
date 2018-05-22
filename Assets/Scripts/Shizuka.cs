using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Shizuka : MonoBehaviour {

    public static Shizuka instance;

    public Shizuka_Sound[] allShizukaSounds;
    public bool generateAudioSources;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (generateAudioSources)
        {
            generateAudioSources = false;
            Component[] components = GetComponents(typeof(AudioSource));
            for(int i = 0; i < allShizukaSounds.Length; i++)
            {
                bool found = false;
                for (int j=0; j < components.Length; j++)
                {
                    /*if (allShizukaSounds[i].audio == components[j].audio)
                    {
                        found = true;
                        break;
                    }*/
                }
                if (!found)
                {
                    Component aux=gameObject.AddComponent(typeof(AudioSource));
                    //(AudioSource)aux.audio = allShizukaSounds[i].audio;
                }
            }
        }
    }

    public void Play(string name)
    {
        for(int i = 0; i < allShizukaSounds.Length; i++)
        {
            if (allShizukaSounds[i].soundName == name)
            {
                allShizukaSounds[i].audioSource.Play();
            }
            else
            {
                Debug.LogError("Sound " + name + " not found.");
            }
        }
    }

    public void Stop(string name)
    {

    }
}
