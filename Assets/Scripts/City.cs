using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour {
    [HideInInspector]
    public float maxCityHP = 10;
    [HideInInspector]
    public float cityHP;
    [HideInInspector]
    public Vector2 pos;
    //-----HP BAR-----



    private void Awake()
    {
        pos = transform.position;
        //print(gameObject.name + " pos= " + pos);
    }
    private void Start()
    {
        cityHP = maxCityHP;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            DamageCity(5);
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
        if (cityHP <= 0)
        {
            DestroySelf();
        }
        //print(gameObject.name + " hp= " + cityHP);
    }
}
