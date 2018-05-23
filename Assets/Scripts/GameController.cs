using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    static public GameController instance;

    [HideInInspector]
    public bool playing = false;
    bool choosingSpawnPos = false;

    public GameObject[] enemiesPrefabs;
    [HideInInspector]
    public List<Enemy> enemies;
    public Transform enemyParent;
    //public Transform[] spawnPositions;

    //---------------CITIES------------
    public Transform[] cities;
    public float citiesHP = 20;
    [Tooltip("Max number of cities destroyed to lose")]
    public float maxCitiesDestroyed=2;
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
    //--------------SPAWNS---------------
    Spawn currentSpawn;
    SpawnPos currentSpawnPos;
    List<Spawn> spawns;
    public List<SpawnPos> spawnPositions;

    public struct Spawn
    {
        public List<SpawnPos> spawnPositions;

        public Spawn(List<SpawnPos> _spawnPositions = null)//relleno
        {
            spawnPositions = _spawnPositions;
        }
    }

    public float timeBetweenSpawns = 3;
    float actTimeBetweenSpawns = 0;
    public float timeBetweenWaves = 4;
    float actTimeBetweenWaves = 0;


    private void Awake()
    {
        instance = this;
        citiesDestroyed = 0;
        konoCities = new List<konoCity>();
        spawns = new List<Spawn>();
        enemies=new List<Enemy>();
        for (int i = 0; i < cities.Length; i++)
        {
            cities[i].GetComponent<City>().maxCityHP = citiesHP;
            konoCities.Add(new konoCity(cities[i].GetComponent<City>()));
        }
    }

    private void Start()
    {
        GenerateSpawns();
        StartGame();
    }

    List<SpawnPos> CopyList(List<SpawnPos> listToCopy)
    {
        List<SpawnPos> aux = new List<SpawnPos>();
        for (int i = 0; i < listToCopy.Count; i++)
        {
            aux.Add(listToCopy[i]);
        }
        return aux;
    }
    public void PrintListNames(List<SpawnPos> list)
    {
        string listPrint = list.ToString() + ": ";
        for (int i = 0; i < list.Count; i++)
        {
            listPrint += list[i].name + ", ";
        }
        print(listPrint);
    }
    void GenerateSpawns()//Rellena spawns
    {
        List<SpawnPos> cp1SpawnPositions = CopyList(spawnPositions);
        //PrintListNames(cp1SpawnPositions);
        int lastNumber = -1;
        while (cp1SpawnPositions.Count > 0 && lastNumber < 20)
        {
            List<SpawnPos> cp2SpawnPositions = CopyList(cp1SpawnPositions);
            //PrintListNames(cp2SpawnPositions);
            List<SpawnPos> auxSpawnPositions = new List<SpawnPos>();
            lastNumber++;//para empezar en 0
            //print("lastNumber= " + lastNumber + "; cpSpawnPositions.Count= " + cp1SpawnPositions.Count);
            for (int j = 0; j < cp1SpawnPositions.Count; j++)
            {
                //print("lastNumber = " + lastNumber + "; " + cp1SpawnPositions[j].name + " number = " + cp1SpawnPositions[j].spawnNumber + "; index= " + j);
                if (lastNumber == cp1SpawnPositions[j].spawnNumber)
                {
                    auxSpawnPositions.Add(cp1SpawnPositions[j]);
                    //print(cp1SpawnPositions[j].name + " removed at index: " + j);
                    cp2SpawnPositions.Remove(cp1SpawnPositions[j]);
                }
            }
            Spawn auxSpawn = new Spawn();
            auxSpawn.spawnPositions = auxSpawnPositions;
            spawns.Add(auxSpawn);
            //PrintListNames(cp2SpawnPositions);
            cp1SpawnPositions = CopyList(cp2SpawnPositions);
            //PrintListNames(cp1SpawnPositions);
        }
        /*for(int i=0; i < spawns.Count; i++)
        {
            print("Spawn "+i+":");
            PrintListNames(spawns[i].spawnPositions);
        }*/
    }

    bool slowMo = false;
    private void Update()
    {
        InputManager.instance.KonoUpdate();

        if (playing)
        {
            if (currentEnemiesInWave >= enemiesPerWave)//si se spawneo el max enemigos por oleada
            {
                actTimeBetweenWaves += Time.deltaTime;
                if (actTimeBetweenWaves >= timeBetweenWaves)
                {
                    ChooseSpawn();
                }
            }
            else
            {
                if (choosingSpawnPos)
                {
                    ChooseSpawnPos();
                }
                else
                {
                    actTimeBetweenSpawns += Time.deltaTime;
                    if (actTimeBetweenSpawns >= timeBetweenSpawns)
                    {
                        SpawnEnemy(EnemyType.kamikaze);
                    }
                }
            }
        }
        else
        {
            if (Player.instance.doingCombo)
            {
                if (destroyAllEnemiesAnim)
                {
                    DestroyAllEnemiesAnimProcess();
                }
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

        Player.instance.KonoUpdate();
        MechaAnimation.instance.KonoUpdate();
        UI_Controller.instance.KonoUpdate();
    }

    public void DestroyCity(GameObject aCity)
    {
        citiesDestroyed++;
        for (int i = 0; i < konoCities.Count; i++)
        {
            if (konoCities[i].city.gameObject == aCity)
            {
                konoCities.RemoveAt(i);
            }
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].targetCity.gameObject == aCity)
            {
                enemies[i].SelectTarget();
            }
        }
        if (citiesDestroyed >= maxCitiesDestroyed)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        Debug.Log("GAME OVER");
        playing = false;
        StopAllEnemies();
        Player.instance.stoppu = true;
        Weapons.instance.stoppu = true;
        Shizuka.instance.Play("GameOver");
    }

    public void StartGame()
    {
        Debug.Log("GAME START");
        playing = true;
        ChooseSpawn();
        SpawnEnemy(EnemyType.kamikaze);
    }

    void ChooseSpawn()
    {
        //print("NEW SPAWN");
        int r = Random.Range(0, spawns.Count);
        currentSpawn = spawns[r];
        currentEnemiesInWave = 0;
        actTimeBetweenSpawns = 0;//innecesario
        ChooseSpawnPos();
    }

    void ChooseSpawnPos()
    {
        //print("NEW SPAWN POS");
        int r = Random.Range(0, currentSpawn.spawnPositions.Count);
        currentSpawnPos = currentSpawn.spawnPositions[r];
        choosingSpawnPos = false;

    }


    public enum EnemyType
    {
        none,
        kamikaze
    }
    void SpawnEnemy(EnemyType enemy)
    {
        //print("SPAWN ENEMY");
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
        //print("Spawned enemy at " + currentSpawnPos.spawnPos);
        GameObject enemyAux = Instantiate(enemiesPrefabs[i], currentSpawnPos.spawnPos, Quaternion.Euler(0, 0, 0), enemyParent);
        enemyAux.GetComponent<Enemy>().mySpawnPos = currentSpawnPos;
        enemyAux.GetComponent<Enemy>().myCityWhiteList = currentSpawnPos.cityWhiteList;
        enemyAux.GetComponent<Enemy>().KonoStart();
  
        enemies.Add(enemyAux.GetComponent<Enemy>());

        currentEnemiesInWave++;
        if (currentEnemiesInWave >= enemiesPerWave)
        {
            actTimeBetweenWaves = 0;
        }
        actTimeBetweenSpawns = 0;
        choosingSpawnPos = true;
    }

    public void DestroyEnemy(Enemy enem)
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            if (enem.gameObject==enemies[i].gameObject)
            {
                enemies.RemoveAt(i);
            }
        }

    }

    float maxTimeBetweenDestroyEnemy = 0.15f;
    float timeBetweenDestroyEnemy = 0;
    [HideInInspector]
    public bool destroyAllEnemiesAnim = false;
    public void DestroyAllEnemiesAnimStart()
    {
        timeBetweenDestroyEnemy = 0;
        destroyAllEnemiesAnim = true;
    }
    public void DestroyAllEnemiesAnimProcess()
    {
        if (enemies.Count > 0)
        {
            timeBetweenDestroyEnemy += Time.deltaTime;
            if (timeBetweenDestroyEnemy >= maxTimeBetweenDestroyEnemy)
            {
                timeBetweenDestroyEnemy = 0;
                enemies[0].DestroySelf();//esto llama a enemies.Remove(enemy);
            }
        }
        else//termina animacion
        {
            timeBetweenDestroyEnemy = 0;
            Player.instance.StopComboSkill();
            destroyAllEnemiesAnim = false;
        }

    }

    public void StopAllEnemies()
    {
        foreach (Enemy e in enemies)
        {
            e.stoppu = true;
        }
    }

    public void ResumeAllEnemies()
    {
        foreach (Enemy e in enemies)
        {
            e.stoppu = false;
        }
    }
}
