using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private Transform pivot;
    [SerializeField] private float viewAngle;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0f;
        var pos = transform.position;
        var playerSizeX = transform.localScale.x;
        var clampValue = Mathf.Clamp(mouse.x, board.LeftBoundX + playerSizeX * 0.5f,
            board.RightBoundY - playerSizeX * 0.5f);
        pos.x = clampValue;
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * moveSpeed);
        var dir = mouse - pivot.position;
        var angle = -90f + Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, -viewAngle * 0.5f, viewAngle * 0.5f);
        pivot.rotation = Quaternion.Lerp(pivot.rotation, Quaternion.Euler(Vector3.forward * angle),
            Time.deltaTime * rotationSpeed);
    }

    public float AngleTo(Vector2 this_, Vector2 to)
    {
        Vector2 direction = to - this_;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0f) angle += 360f;
        return angle;
    }
}