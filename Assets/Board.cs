using System;
using DefaultNamespace;
using DefaultNamespace.Unit;
using UnityEngine;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    [SerializeField] private float leftBoundX;
    [SerializeField] private float rightBoundX;
    [SerializeField] private float sizeY;
    [SerializeField] private Spawner fallingUnit;
    private void Update()
    {

       /* fallingUnit.units?.ForEach(unit =>
        {
            if (unit == null)
            {
                return;
            }

            if (Mathf.Abs(unit.transform.position.x) > rightBoundX)
            {
                var dir = unit.transform.up * -1f;
                unit.transform.Rotate(dir*90f);
            }

            if (Mathf.Abs(unit.transform.position.y) > sizeY / 2)
            {
                // velocity.y *= -1;
                // unit.transform.position += velocity*0.4f;
            }
            Debug.DrawLine(unit.transform.position, unit.transform.position + unit.transform.up * 2f, Color.red);
        });*/
    }

    public float LeftBoundX => leftBoundX;

    public float RightBoundX => rightBoundX;

    public float SizeY => sizeY;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(rightBoundX - leftBoundX, sizeY, 0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("jgdsgasgbdjsg");
       //other.transform.position += -1 * velocity * 0.8f;
    }
}