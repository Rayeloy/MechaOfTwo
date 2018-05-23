using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Shizuka_Sound", menuName = "Shizuka_Sound")]
public class Shizuka_Sound : ScriptableObject {

    [Tooltip("Sound ID.Important.")]
    public string soundName;
    public AudioClip clip;
    [Tooltip("From 0 to 1. Default=1")]
    public float volume = 1;
    [Tooltip("From -3 to 3. Default=1")]
    public float pitch = 1;
    public bool loop = false;
    public bool playOnAwake = false;
    [Tooltip("Mute this sound?")]
    public bool mute = false;

}
