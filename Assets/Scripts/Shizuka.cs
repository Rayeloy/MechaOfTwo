using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Shizuka : MonoBehaviour {

    public static Shizuka instance;

    public Shizuka_Sound[] allShizukaSounds;
    public bool generateAudioSources;
    public bool updateAudioSources;
    [SerializeField]
    public List<Shizuka_AS> allShizukaAS;

    [System.Serializable]
    public struct Shizuka_AS
    {
        public Shizuka_Sound shizukaSound;
        public AudioSource AS;
        public Shizuka_AS(Shizuka_Sound _shizukaSound, AudioSource _AS)
        {
            shizukaSound = _shizukaSound;
            AS = _AS;
        }
        public void SetShizukaSound(Shizuka_Sound _sS)
        {
            shizukaSound = _sS;
        }
        public void SetAS(AudioSource _AS)
        {
            AS = _AS;
        }
    }

    private void Awake()
    {
        if (Application.isPlaying)
        {
            instance = this;
            //GenerateAudioSources();
        }
    }


    private void Update()
    {
        if (Application.isEditor)
        {
            if (generateAudioSources)
            {
                GenerateAudioSources();
            }

            if (updateAudioSources)
            {
                UpdateAudioSources();
            }
        }

    }

    public void Play(string name)
    {
        print(allShizukaAS);
        print("allShizukaAS.Count= " + allShizukaAS.Count);
        for (int i = 0; i < allShizukaAS.Count; i++)
        {
            if (allShizukaAS[i].shizukaSound.soundName == name)
            {
                allShizukaAS[i].AS.Play();
            }
            else
            {
                Debug.LogError("Sound " + name + " not found.");
            }
        }
    }

    public void Stop(string name)
    {
        for (int i = 0; i < allShizukaAS.Count; i++)
        {
            if (allShizukaAS[i].shizukaSound.soundName == name)
            {
                if (allShizukaAS[i].AS.isPlaying)
                {
                    allShizukaAS[i].AS.Stop();
                }
            }
            else
            {
                Debug.LogError("Sound " + name + " not found.");
            }
        }
    }

    void GenerateAudioSources()
    {
        if (allShizukaAS == null)
        {
            print("allShizukaAS = null");
            allShizukaAS = new List<Shizuka_AS>();
        }
        else
        {
            print("allShizukaAS count= " + allShizukaAS.Count);
        }

        generateAudioSources = false;
        //Component[] components = GetComponents(typeof(AudioSource));
        AudioSource[] components = GetComponents<AudioSource>();
        for (int i = 0; i < allShizukaSounds.Length; i++)
        {
            bool found = false;
            for (int j = 0; j < components.Length; j++)
            {
                if (allShizukaSounds[i].clip == components[j].clip)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                CreateASAndShizukaAS(i);
            }
            else
            {
                bool hasSAS = false;
                for(int k = 0; k < allShizukaAS.Count; k++)
                {
                    if (allShizukaSounds[i].soundName == allShizukaAS[k].shizukaSound.soundName)
                    {
                        hasSAS = true;
                        break;
                    }
                }
                if (!hasSAS)
                {
                    CreateASAndShizukaAS(i);
                }
            }
        }
        //REMOVE THE ONES THAT WERE NOT FOUND
        components = GetComponents<AudioSource>();
        for (int j = 0; j < components.Length; j++)
        {
            bool found = false;
            for (int i = 0; i < allShizukaSounds.Length; i++)
            {
                if (allShizukaSounds[i].clip == components[j].clip)
                {
                    found = true;
                    break;
                }
            }
            if (!found)//remove component from gameObject and Shizuka_AS from allShizukaAS
            {
                for(int h = 0; h < allShizukaAS.Count; h++)
                {
                    if (allShizukaAS[h].shizukaSound == allShizukaSounds[j])
                    {
                        allShizukaAS.RemoveAt(h);
                        h = -1;
                    }
                }
                DestroyImmediate(components[j]);

            }
        }
    }

    void CreateASAndShizukaAS(int i)
    {
        AudioSource aux = gameObject.AddComponent<AudioSource>();
        aux.clip = allShizukaSounds[i].clip;
        aux.playOnAwake = allShizukaSounds[i].playOnAwake;
        aux.volume = allShizukaSounds[i].volume;
        aux.pitch = allShizukaSounds[i].pitch;
        aux.mute = allShizukaSounds[i].mute;
        aux.loop = allShizukaSounds[i].loop;
        allShizukaAS.Add(new Shizuka_AS(allShizukaSounds[i], aux));
    }

    public void UpdateAudioSources()
    {
        //allShizukaAS = new List<Shizuka_AS>();
        updateAudioSources = false;
        //Component[] components = GetComponents(typeof(AudioSource));
        //AudioSource[] components = GetComponents<AudioSource>();
        for (int i = 0; i < allShizukaSounds.Length; i++)
        {
            int index = 0;
            bool found = false;
            print("allShizukaAS.Count= " + allShizukaAS.Count);
            for (int j = 0; j < allShizukaAS.Count; j++)
            {
                if (allShizukaSounds[i].soundName == allShizukaAS[j].shizukaSound.soundName)
                {
                    found = true;
                    index = j;
                    break;
                }
            }

            if (found)//Update AS
            {
                AudioSource[] components = GetComponents<AudioSource>();
                for(int k = 0; k < components.Length; k++)
                {
                    if (components[k] == allShizukaAS[index].AS)
                    {
                        components[k].clip = allShizukaSounds[i].clip;
                        components[k].playOnAwake = allShizukaSounds[i].playOnAwake;
                        components[k].volume = allShizukaSounds[i].volume;
                        components[k].pitch = allShizukaSounds[i].pitch;
                        components[k].mute = allShizukaSounds[i].mute;
                        components[k].loop = allShizukaSounds[i].loop;
                        allShizukaAS[index].SetShizukaSound(allShizukaSounds[i]);
                        allShizukaAS[index].SetAS(components[k]); 
                    }
                }
            }
        }
    }
}
