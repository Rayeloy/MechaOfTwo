using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{

    public Rigidbody2D myRb;

    public float hp = 10;
    public float damage = 10;
    public float speed = 5;

    [HideInInspector]
    public City targetCity;
    protected Vector2 targetPos;

    [HideInInspector]
    public SpawnPos mySpawnPos;
    [HideInInspector]
    public List<City> myCityWhiteList;
    [HideInInspector]
    public bool stoppu = false;

    protected bool started = false;
    private void Awake()
    {
        started = false;
    }
    public virtual void KonoStart()
    {
        switch (mySpawnPos.mySpawnDir)
        {
            case SpawnPos.SpawnDir.right:
                transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
            case SpawnPos.SpawnDir.down:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case SpawnPos.SpawnDir.left:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case SpawnPos.SpawnDir.up:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;

        }
        SelectTarget();

        started = true;
    }

    protected virtual void Update()
    {
        if (started && !stoppu)
        {
            Move();
        }
    }

    public virtual void SelectTarget()
    {
        /*string ss = gameObject.name + " WhiteList: ";
        foreach (City c in myCityWhiteList)
        {
            ss += c.gameObject.name + ", ";
        }
        print(ss);*/
        //WHITELIST
        List<City> finalCityList = new List<City>();
        for (int i = 0; i < GameController.instance.konoCities.Count; i++)
        {
            bool valid = true;
            for (int j = 0; j < myCityWhiteList.Count; j++)
            {
                if (myCityWhiteList[j] == GameController.instance.konoCities[i].city)
                {
                    valid = false;
                    break;
                }
            }
            if (valid)
            {
                finalCityList.Add(GameController.instance.konoCities[i].city);
            }
        }
        string s = gameObject.name + " finalCityList: ";
        foreach(City c in finalCityList)
        {
            s += c.gameObject.name+", ";
        }
        print(s);
        //RANDOM CITY
        int r = Random.Range(0, finalCityList.Count);
        targetCity = finalCityList[r];
        targetPos = targetCity.pos;
    }

    /*public virtual void ChangeTarget()
    {
        for(int i=0; i < GameController.instance.konoCities.Count; i++)
        {
            if (!GameController.instance.konoCities[i].Contains(targetCity))
        }

    }*/

    protected virtual void Move()
    {

    }

    protected virtual void UpdateMyRotation()
    {

    }

    void TakeDamage(float damage)
    {
        hp -= damage;
        print("Enemy " + this.gameObject.name + " taking " + damage + " damage, current hp= " + hp);
        if (hp <= 0)
        {
            DestroySelf();
        }
    }

    public void DestroySelf()
    {
        GameController.instance.DestroyEnemy(this);
        print("Enemy " + this.gameObject.name + " destroyed");
        //animacion particulas explosion
        Destroy(this.gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        print("COLLISION WITH: " + col.name + "; tag= " + col.tag);
        switch (col.tag)
        {
            case "City":
                if (col.GetComponent<City>().gameObject == targetCity.gameObject)
                {
                    GameController.instance.DestroyEnemy(this);//lo pongo antes en este caso(aunque se repita, no pasa nada) para evitar que destroyCity llame al enemigo que no existe
                    col.GetComponent<City>().DamageCity(damage);
                    DestroySelf();
                }
                break;
            case "Bullet":
                TakeDamage(col.GetComponent<Bullet>().damage);
                col.GetComponent<Bullet>().SelfDestroy();
                break;
        }
    }
}
