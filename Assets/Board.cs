using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private float leftBoundX;
    [SerializeField] private float rightBoundX;
    [SerializeField] private float sizeY;


    public float LeftBoundX => leftBoundX;

    public float RightBoundX => rightBoundX;

    public float SizeY => sizeY;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(rightBoundX - leftBoundX, sizeY, 0f));
    }
}