using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Unit;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float amount = 0.1f;
    public float deltaAngle = 5f;
    public bool leftA=true;
    public bool rightA = true;
    public LayerMask mask;
    [SerializeField] private Spawner spawner;
    [SerializeField] private Player player;

    public void Update()
    {
        spawner.units?.ForEach(unit =>
        {
            if (unit == null)
            {
                return;
            }
            updateStatus(unit);
        });
    }

    public void updateStatus(AbstractUnit unit)
    {
        var flag = "";
        CheckCollision(out var left, out var right,unit);
       // Debug.LogError($"{left.collider}, {right.collider}");
        var canMove = left.collider == null && right.collider == null;
        
        if (left.collider != null && leftA)
        {
           unit.transform.Rotate(Vector3.forward * deltaAngle * -1 * Time.deltaTime);
           Debug.Log("left");
        }

        else if (right.collider != null && rightA)
        {
            unit.transform.Rotate(Vector3.forward * deltaAngle * Time.deltaTime);
            Debug.Log("right");
           
        }
        
        else if (left.collider && right.collider)   
        {
            Debug.Log(("both"));
            unit.transform.Rotate(Vector3.up * player.transform.position.y * deltaAngle * Time.deltaTime);
        }

        if (canMove)
        {
            unit.transform.Translate(unit.transform.up * Time.deltaTime * 1f, Space.World);
        }
    }

    private void CheckCollision(out RaycastHit2D leftRayHit, out RaycastHit2D rightRayHit,AbstractUnit unit)
    {
        var leftRayOrigin = unit.transform.position + unit.transform.up * unit.transform.localScale.y * 0.5f -
                            unit.transform.right * unit.transform.localScale.x * 0.5f;
        var rightRayOrigin = unit.transform.position + unit.transform.up *  unit.transform.localScale.y * 0.5f
                                                     + unit.transform.right * unit.transform.localScale.x * 0.5f;
        var dir = unit.transform.up;

        Debug.DrawRay(leftRayOrigin, 2*dir * amount, Color.green);
        Debug.DrawRay(rightRayOrigin, 2*dir * amount, Color.green);

        var leftRay = Physics2D.Raycast(leftRayOrigin, dir, amount, mask);
        var rightRay = Physics2D.Raycast(rightRayOrigin, dir, amount, mask);
        leftRayHit = leftRay;
        rightRayHit = rightRay;
    }
}