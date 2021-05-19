using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlocks : MonoBehaviour
{
    private Transform Square;

    private Vector2 s;
    private float nextSpawnTime;
    private float secondsBetweenSpawns = 1;

    private void Start()
    {
        s = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
    }

    private void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + secondsBetweenSpawns;
            //Vector2 spawnPosition = new Vector2(Random.Range(-screenHalfSize.x, screenHalfSize.x), screenHalfSize.y);
            //Instantiate(fallingBlock, spawnPosition, Quaternion.identity);
        }
    }
}