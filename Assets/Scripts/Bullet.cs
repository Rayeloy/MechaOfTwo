using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject impactAnim;
    bool traveling = false;
    [HideInInspector]
    public Vector2 dir;
    float speed;
    [HideInInspector]
    public float damage;
    Vector2 velocity;

    float maxTime = 5;
    float currentTime = 0;

    public void konoStart(Vector2 direction, float _speed, float _damage)
    {
        traveling = true;
        dir = direction;
        speed = _speed;
        velocity = dir * speed;
        damage = _damage;
        currentTime = 0;

    }

    private void Update()
    {
        if (traveling)
        {
            currentTime += Time.deltaTime;
            transform.parent.Translate(velocity * Time.deltaTime);
            if (currentTime >= maxTime)
            {
                SelfDestroy();
            }
        }
    }

    public void SelfDestroy()
    {
        //impact anim
        GameObject impact=Instantiate(impactAnim, transform.position, Quaternion.Euler(0, 0, 0), Weapons.instance.bulletsParent);
        impact.GetComponent<Impact>().konoStart();
        Destroy(transform.parent.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        print("Collision with "+col.gameObject.name);
        switch (col.tag)
        {
            case "Enemy":
                break;
            case "Obstacle":
                SelfDestroy();
                break;
        }
    }
}
