using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class DeathZone:MonoBehaviour
    {
        [SerializeField]  public float leftBoundX;
        [SerializeField] public float rightBoundX;
        [SerializeField] public float sizeY;
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(rightBoundX - leftBoundX, sizeY, 0f));
        }
    }
}