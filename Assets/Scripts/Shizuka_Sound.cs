using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Shizuka_Sound", menuName = "Shizuka_Sound")]
public class Shizuka_Sound : ScriptableObject {

    public string soundName;
    public AudioClip audio;
    public float volume;
    public float pitch;
    public bool loop;
    public bool startOnAwake;

}
