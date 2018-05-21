using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour {

    public Rigidbody2D myRb;

    public float hp = 10;
    public float damage = 10;
    public float speed = 5;

    [HideInInspector]
    public City targetCity;
    protected Vector2 targetPos;

    [HideInInspector]
    public SpawnPos mySpawnPos;
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
        if (started)
        {
            Move();
        }
    }

    public virtual void SelectTarget()
    {
            int r = Random.Range(0, GameController.instance.konoCities.Count);
            targetCity = GameController.instance.konoCities[r].city;
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
        if (hp <= 0)
        {
            DestroySelf();
        }
        print("Enemy " + this.gameObject.name + " taking "+damage+" damage, current hp= "+hp);
    }

    void DestroySelf()
    {
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
