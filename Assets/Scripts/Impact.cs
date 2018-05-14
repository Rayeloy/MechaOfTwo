using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{
    public float maxTime = 2;
    float time = 0;
    bool counting = false;

    public void konoStart()
    {
        counting = true;
    }
    private void Update()
    {
        if (counting)
        {
            time += Time.deltaTime;
            if (time >= maxTime)
            {
                Destroy(gameObject);
            }
        }

    }
}

