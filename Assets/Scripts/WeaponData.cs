using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New weapon", menuName = "Weapon")]
public class WeaponData : ScriptableObject {

    public Sprite sprite;
    public string weaponName;
    public Weapons.WeaponTypes weaponType;
    public Weapons.WeaponPosition weaponPosition;
    public GameObject bulletPrefab;
    [Tooltip("Damage per bullet")]
    public float damage;
    [Tooltip("Time between each bullet shot. The smaller the higher the firing rate")]
    public float firingRate;
    public float bulletSpeed;
    [Tooltip("Overheating per bullet shot")]
    public float overheat;
    //rotate speed or move speed
    [Tooltip("rotate speed or move speed")]
    public float rotateSpeed;
    //Range in degrees that the weapon can be rotated upwards from 0 degrees (X axis)
    [Tooltip("Range in degrees that the weapon can be rotated upwards and downwards from 0 degrees (X axis)")]
    public float rotateRange;

}
