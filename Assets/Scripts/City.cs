using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour {
    [HideInInspector]
    public float cityHP = 10;
    [HideInInspector]
    public Vector2 pos;
    //-----HP BAR-----



    private void Awake()
    {
        pos = transform.position;
        //print(gameObject.name + " pos= " + pos);
    }

    private void Update()
    {
        if (cityHP <= 0)
        {
            DestroySelf();
        }
    }

    public void DestroySelf()
    {
        GameController.instance.DestroyCity(this.gameObject);
        //animacion particulas
        Destroy(this.gameObject);
    }

    public void DamageCity(float damage)
    {
        cityHP -= damage;
        //print(gameObject.name + " hp= " + cityHP);
    }
}
