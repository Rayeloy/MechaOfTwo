using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPos : MonoBehaviour {

    public Vector2 localSpawnPos;
    [HideInInspector]
    public Vector2 spawnPos;
    public SpawnDir mySpawnDir;
    public enum SpawnDir
    {
        down,
        left,
        up,
        right
    }
    [Tooltip("The SpawnPositions with the same spawn Number belong to the same Spawn (spawn area). Always start with spawnNumber 0")]
    public int spawnNumber;
    public List<City> cityWhiteList;

    private void Awake()
    {
        spawnPos = transform.TransformPoint(localSpawnPos);
    }

}
