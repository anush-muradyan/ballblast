using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.Unit;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float amount = 0.1f;
    public float deltaAngle = 5f;
    public bool leftA=true;
    public bool rightA = true;
    public LayerMask mask;

    

    public void Update()
    {
        updateStatus();
    }

    public void updateStatus()
    {
        var flag = "";
        CheckCollision(out var left, out var right);
        Debug.LogError($"{left.collider}, {right.collider}");
        var canMove = left.collider == null && right.collider == null;
        
        if (left.collider != null && leftA)
        {
            transform.Rotate(Vector3.forward * deltaAngle * -1 * Time.deltaTime);
            flag = "left";
        }

        else if (right.collider != null && rightA)
        {
            transform.Rotate(Vector3.forward * deltaAngle * Time.deltaTime);
            flag = "right";
           
        }
        
        else if (left.collider && right.collider)   
        {
            if (flag=="left")
            {
                rightA = false;
            }
            if (flag=="right")
            {
                leftA = false;
            }
        }

        Debug.LogError(flag);
        if (canMove)
        {
            transform.Translate(transform.up * Time.deltaTime * 1f, Space.World);
        }
    }

    private void CheckCollision(out RaycastHit2D leftRayHit, out RaycastHit2D rightRayHit)
    {
        var leftRayOrigin = transform.position + transform.up * transform.localScale.y * 0.5f -
                            transform.right * transform.localScale.x * 0.5f;
        var rightRayOrigin = transform.position + transform.up *  transform.localScale.y *0.5f + transform.right * transform.localScale.x * 0.5f;
        var dir = transform.up;

        Debug.DrawRay(leftRayOrigin, dir * amount, Color.green);
        Debug.DrawRay(rightRayOrigin, dir * amount, Color.green);

        var leftRay = Physics2D.Raycast(leftRayOrigin, dir, amount, mask);
        var rightRay = Physics2D.Raycast(rightRayOrigin, dir, amount, mask);
        leftRayHit = leftRay;
        rightRayHit = rightRay;
    }
}