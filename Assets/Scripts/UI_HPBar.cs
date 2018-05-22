using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HPBar : MonoBehaviour {

    public City myCity;
    public float xWhenEmpty;
    public float xWhenFull;
    float maxDistance;
    public Transform hpBar;
    float hpValue;

    private void Awake()
    {
        hpBar.transform.localPosition = new Vector3(xWhenFull, hpBar.transform.localPosition.y, 0);
        maxDistance = xWhenEmpty - xWhenFull;
    }
    private void Update()
    {
        hpValue = myCity.cityHP / myCity.maxCityHP;
        float currentX = xWhenFull + (hpValue * maxDistance);
        hpBar.localPosition = new Vector2(currentX,hpBar.localPosition.y);
    }

}
