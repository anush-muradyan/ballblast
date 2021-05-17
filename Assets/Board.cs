using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private float leftBoundX;
    [SerializeField] private float rightBoundY;
    [SerializeField] private float sizeY;


    public float LeftBoundX => leftBoundX;

    public float RightBoundY => rightBoundY;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(rightBoundY - leftBoundX, sizeY, 0f));
    }
}