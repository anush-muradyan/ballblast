using DefaultNamespace;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private Board board;
    [SerializeField] private Transform pivot;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float viewAngle;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private Bullet shootingItem;

    private void Update()
    {
        Vector3 mouse = camera.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0f;
        var pos = transform.position;
        var playerSizeX = transform.localScale.x;
        var clampValue = Mathf.Clamp(mouse.x, board.LeftBoundX + playerSizeX * 0.5f,
            board.RightBoundX - playerSizeX * 0.5f);
        pos.x = clampValue;
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * moveSpeed);
        var dir = mouse - pivot.position;

        var angle = -90f + Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, -viewAngle * 0.5f, viewAngle * 0.5f);
        pivot.rotation = Quaternion.Lerp(pivot.rotation, Quaternion.Euler(Vector3.forward * angle),
            Time.deltaTime * rotationSpeed);


        Shoot();
    }

    private void Shoot()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        var currentPos = camera.ScreenToWorldPoint(Input.mousePosition);
        var dir = currentPos - transform.position;

        var bullet = Instantiate(shootingItem, shootPoint.position, Quaternion.identity);
        bullet.Shoot(dir);
        //
        //
        // shootingItem.position = transform.position;
        // //Debug.Log(shootingItem.position);
        // var input = Input.GetKeyDown(KeyCode.Mouse0);
        //
        // if (!input)
        //     return;
        //
        //
        // Vector3 mouse = camera.ScreenToWorldPoint(Input.mousePosition);
        // shootingItem.position = mouse;
        // Debug.Log(shootingItem.position +  "      " + mouse);

        //if (Input.GetKey(KeyCode.Mouse0))
        //{
        //    Debug.Log("HHH");
        //}
    }
}