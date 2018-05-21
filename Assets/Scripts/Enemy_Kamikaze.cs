using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Kamikaze : Enemy {

    [Tooltip("Time they are just entering the play zone and moving straight to avoid colision with entrances")]
    public float maxTimeSpawning = 3;
    protected float timeSpawning = 0;
    public float smoothRotTime = 0.2f;

    Vector2 currentDir;
    Vector2 finalDir;

    public override void KonoStart()
    {
        base.KonoStart();

    }

    protected override void Update()
    {
        base.Update();
        //UpdateMyRotation();
    }

    float smoothRotSpeedX;
    float smoothRotSpeedY;
    protected override void Move()
    {
        if (timeSpawning < maxTimeSpawning)//Spawning, move straight
        {
            timeSpawning += Time.deltaTime;
            switch (mySpawnPos.mySpawnDir)
            {
                case SpawnPos.SpawnDir.right:
                    myRb.velocity = Vector2.right * speed;
                    currentDir = Vector2.right;
                    break;
                case SpawnPos.SpawnDir.down:
                    myRb.velocity = Vector2.down * speed;
                    currentDir = Vector2.down;
                    break;
                case SpawnPos.SpawnDir.left:
                    myRb.velocity = Vector2.left * speed;
                    currentDir = Vector2.left;
                    break;
                case SpawnPos.SpawnDir.up:
                    myRb.velocity = Vector2.up * speed;
                    currentDir = Vector2.up;
                    break;
            }
        }
        else//movimiento normal
        {
            //print("TARGET POSITION: "+currentTargetPos);
            finalDir = targetPos - (Vector2)transform.position;
            currentDir.x = Mathf.SmoothDamp(currentDir.x, finalDir.x, ref smoothRotSpeedX, smoothRotTime);
            currentDir.y = Mathf.SmoothDamp(currentDir.y, finalDir.y, ref smoothRotSpeedY, smoothRotTime);
            myRb.velocity = currentDir.normalized * speed;
            UpdateMyRotation();
            //añadir oscilaciones (movimiento aleatorio a los lados)
        }
    }
    public override void SelectTarget()
    {
        base.SelectTarget();
    }
    protected override void UpdateMyRotation()
    {
        float dotProduct = Vector2.Dot(currentDir.normalized, Vector2.right);
        float magnitudesProduct = currentDir.normalized.magnitude * Vector2.right.magnitude;
        float cos = dotProduct / magnitudesProduct;
        float angleDif = Mathf.Acos(cos) * Mathf.Rad2Deg;
        if (currentDir.y <= 0)
        {
            angleDif = -angleDif;
        }
        //print("ANGLE_DIF= " + angleDif);
        angleDif -= 90;
        //print("ANGLE_DIF2= " + angleDif);
        transform.rotation = Quaternion.Euler(0, 0, angleDif);
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);

    }
}
