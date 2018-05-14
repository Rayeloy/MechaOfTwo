using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    static public GameController instance;

    public GameObject[] enemies;
    public Transform enemyParent;
    public Transform[] spawnPositions;

    private void Awake()
    {
        instance = this;
    }

    public void GameOver()
    {

    }

    public void StartGame()
    {

    }


    public enum EnemyType
    {
        none,
        basic
    }
    void SpawnEnemy(EnemyType enemy)
    {
        int i = 0;
        switch (enemy)
        {
            case EnemyType.basic:
                i = 0;
                break;
            case EnemyType.none:
                i = -1;
                break;
        }

        int j=Random.Range(0,spawnPositions.Length);
        GameObject enemyAux = Instantiate(enemies[i], spawnPositions[j].position, Quaternion.Euler(0, 0, 0), enemyParent);
    }
}
