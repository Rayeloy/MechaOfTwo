using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour {

    float cityHP = 100;
    //-----HP BAR-----

    private void Update()
    {
        if (cityHP <= 0)
        {
            DestroySelf();
        }
    }

    public void DestroySelf()
    {
        GameController.instance.citiesDestroyed++;
        //animacion particulas
        Destroy(this.gameObject);
    }

    public void DamageCity(float damage)
    {
        cityHP -= damage;
    }
}
