using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float hp = 10;
    public float damage = 10;
    public float speed = 5;
    public float turningSpeed = 1;

    bool targetSelected = false;
    Vector2 targetPos;

    private void Awake()
    {
  
    }
    private void Start()
    {
        SelectTarget();
    }

    private void Update()
    {
        Move();
    }

    void SelectTarget()
    {
        if (!targetSelected)
        {
            int r = Random.Range(0, GameController.instance.cities.Length);
            targetPos = GameController.instance.cities[r].position;
        }
    }

    void Move()
    {

    }

    void DestroySelf()
    {
        //animacion particulas explosion
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.tag)
        {
            case "City":
                col.GetComponent<City>().DamageCity(damage);
                DestroySelf();
                break;
            case "Bullet":
                DestroySelf();
                break;
        }
    }
}
