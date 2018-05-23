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

    public SpriteRenderer[] spritesCiudad;
    public SpriteRenderer[] spritesCiudadDestruida;

    private void Awake()
    {
        pos = transform.position;
        spritesCiudad[0].enabled = true;
        spritesCiudad[1].enabled = true;
        spritesCiudadDestruida[0].enabled = false;
        spritesCiudadDestruida[1].enabled = false;
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
        spritesCiudad[0].enabled = false;
        spritesCiudad[1].enabled = false;
        spritesCiudadDestruida[0].enabled = true;
        spritesCiudadDestruida[1].enabled = true;
        //animacion particulas
        //Destroy(this.gameObject);
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
