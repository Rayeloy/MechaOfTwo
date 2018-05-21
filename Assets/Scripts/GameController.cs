using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    static public GameController instance;

    bool playing = false;
    bool choosingSpawnPos = false;

    public GameObject[] enemiesPrefabs;
    public Transform enemyParent;
    //public Transform[] spawnPositions;
    public Transform[] cities;
    [HideInInspector]
    public int citiesDestroyed;
    public List<konoCity> konoCities;
    public struct konoCity
    {
        public City city;

        public konoCity(City _city)
        {
            city = _city;
        }

    }

    public int enemiesPerWave = 3;
    int currentEnemiesInWave = 0;
    Spawn currentSpawn;
    Vector2 currentSpawnPos;
    Spawn[] spawns;

    public struct Spawn
    {
        public Vector2[] spawnPositions;

        public Spawn(Vector2[] _spawnPositions)
        {
            spawnPositions = _spawnPositions;
        }
    }


    public float timeBetweenSpawns = 3;
    float actTimeBetweenSpawns = 0;


    private void Awake()
    {
        instance = this;
        citiesDestroyed = 0;
        konoCities = new List<konoCity>();
    }

    private void Start()
    {
        for (int i = 0; i <= cities.Length; i++)
        {
            konoCities.Add(new konoCity(cities[i].GetComponent<City>()));
        }
    }

    bool slowMo = false;
    private void Update()
    {
        if (playing)
        {
            if (currentEnemiesInWave >= enemiesPerWave)//si se spawneo el max enemigos por oleada
            {
                ChooseSpawn();
            }

            if (choosingSpawnPos == false)
            {
                ChooseSpawnPos();
            }

            if (!choosingSpawnPos)
            {
                SpawnEnemy(EnemyType.kamikaze);
            }



        }


        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            if (slowMo)
            {
                Time.timeScale = 1;
                slowMo = false;
            }
            else
            {
                Time.timeScale = 0.25f;
                slowMo = true;
            }

        }
    }

    public void GameOver()
    {
        if (citiesDestroyed >= 2)
        {
            playing = false;
        }
    }

    public void StartGame()
    {
        playing = true;
        ChooseSpawn();
        SpawnEnemy(EnemyType.kamikaze);
    }

    void ChooseSpawn()
    {

        int r = Random.Range(0, cities.Length);
        currentSpawn = spawns[r];
        currentEnemiesInWave = 0;
        actTimeBetweenSpawns = 0;//innecesario
        ChooseSpawnPos();
    }

    void ChooseSpawnPos()
    {

        int r = Random.Range(0, currentSpawn.spawnPositions.Length);
        currentSpawnPos = currentSpawn.spawnPositions[r];
        choosingSpawnPos = true;

    }


    public enum EnemyType
    {
        none,
        kamikaze
    }
    void SpawnEnemy(EnemyType enemy)
    {
        actTimeBetweenSpawns += Time.deltaTime;
        if (actTimeBetweenSpawns >= timeBetweenSpawns)
        {
            int i = 0;
            switch (enemy)
            {
                case EnemyType.kamikaze:
                    i = 0;
                    break;
                case EnemyType.none:
                    i = -1;
                    break;
            }

            GameObject enemyAux = Instantiate(enemiesPrefabs[i], currentSpawnPos, Quaternion.Euler(0, 0, 0), enemyParent);
            currentEnemiesInWave += 1;
            actTimeBetweenSpawns = 0;
            choosingSpawnPos = false;
        }
    }
}
