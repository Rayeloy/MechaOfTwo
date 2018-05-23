using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetParticleSystemLayer : MonoBehaviour
{

    public string sortingLayer;
    public int orderInLayer = 0;
    public bool setLayers = false;
    public float volume = 0.3f;
    public bool setVolumes = false;
    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor)
        {
            if (setLayers)
            {
                setLayers = false;
                SetLayersToAll();
            }
            if (setVolumes)
            {
                setVolumes = false;
                SetVolumesToAll();
            }
        }
    }

    public void SetLayersToAll()
    {
        ParticleSystemRenderer[] components = GetComponentsInChildren<ParticleSystemRenderer>();
        for (int i = 0; i < components.Length; i++)
        {
            components[i].sortingLayerName = sortingLayer;
            components[i].sortingOrder = orderInLayer;
        }
    }

    public void SetVolumesToAll()
    {
        AudioSource[] components = GetComponentsInChildren<AudioSource>();
        for (int i = 0; i < components.Length; i++)
        {
            components[i].volume = volume;
        }
    }
}
