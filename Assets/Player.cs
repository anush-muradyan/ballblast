using System;
using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.GameStates;
using DefaultNamespace.Unit;
using UnityEngine;

public class Player : MonoBehaviour, IGameStart,IGameWin,IGameLoose,IGamePause,IGameResume
{
    [SerializeField] private Camera camera;
    [SerializeField] private Board board;
    [SerializeField] private DeathZone deathZone;
    [SerializeField] private Transform pivot;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float viewAngle;
    [SerializeField] private float moveSpeed;

    [SerializeField] private float rotationSpeed;
    
    [SerializeField] private Bullet shootingItem;

    private List<Bullet> activeBullets = new List<Bullet>();
   
    private bool _canShoot;

    
    private bool isStarted;
    private bool isWon;
    private bool isloosed;
    private bool isPaused;
    private void Start()
    {
        _canShoot = true;
        
    }

    private void Update()
    {
        if (!isStarted)
        {
            return;
        }

        if (isPaused)
        {
            return;
        }

        if (!isWon)
        {
            CreatingGameObjects();
            Shoot();
            checkBounds();
        }

        if (isWon)
        {
            Debug.Log("Player Win");
            isWon = false;
        }
        if (isloosed)
        {
            Debug.Log("Player Loose");
            isloosed = false;
        }
        if (isPaused)
        {
            Debug.Log("Game Paused");
            isPaused = false;
        }
        
    }

    public void StartGame()
    {
        isStarted = true;
    }

    public void WinGame()
    {
        isWon = true;
    }
    public void LooseGame()
    {
        isloosed = true;
    }

    public void PauseGame()
    {
        isPaused = true;
        
    }
    public void ResumeGame()
    {
        isPaused = false;

    }
    
    private void CreatingGameObjects()
    {
        var mouse = camera.ScreenToWorldPoint(Input.mousePosition);
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
    }

    private void Shoot()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        if (_canShoot)
        {
            StartCoroutine(wait());
            _canShoot = false;
            var currentPos = camera.ScreenToWorldPoint(Input.mousePosition);
            var dir = currentPos - transform.position;

            var bullet = Instantiate(shootingItem, shootPoint.position, Quaternion.identity);

            bullet.OnHit += onHit;

            bullet.Shoot(dir);
            activeBullets.Add(bullet);
        }
    }

    private void onHit()
    {
        Debug.LogError("Yeeeeeee");
    }

    private void checkBounds()
    {
        activeBullets?.ForEach(bullet =>
        {
            if (bullet == null)
            {
                return;
            }

            if (bullet.transform.position.y > deathZone.sizeY)
            {
                bullet.OnHit -= onHit;
                Destroy(bullet.gameObject);
            }
        });

        activeBullets?.RemoveAll(bullet => bullet == null);
    }

    private IEnumerator wait()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        _canShoot = true;
    }
}