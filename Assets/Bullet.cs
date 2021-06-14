using System;
using System.Collections;
using DefaultNamespace.Unit;
using UnityEngine;

namespace DefaultNamespace
{
    public class Bullet : MonoBehaviour
    {
    [SerializeField] private float speed;
    private Vector2 dir;
    

    public void Shoot(Vector2 dir)
    {
        this.dir = dir;
    }

    private void Update()
    {
        transform.Translate(dir * speed * 1.12f * Time.deltaTime);
    }
    }
}