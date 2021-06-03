using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.GameStates;
using UnityEngine;

public class Player : MonoBehaviour, IGameStart, IGameWin, IGameLoose, IGamePause, IGameResume, IGameRestart
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
    [SerializeField] private Spawner spawner;
    [SerializeField] private GameManager gameManager;
    private List<Bullet> activeBullets = new List<Bullet>();
    
    private int score = 0;
    private bool _canShoot;

    private bool isStarted;
    private bool isWon;
    private bool isloosed;
    private bool isPaused;
    private bool gameOver;
    private int activeBulletCount;

    private void Start()
    {
        Debug.Log("Player ");
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

        if (!isWon && !gameOver)
        {
            PivotHolderRotation();
            Shoot();
            checkBounds();
        }
       
        if (isWon && gameOver)
        {
            Debug.Log("Player Win");
            gameOver = false;
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
        activeBulletCount = spawner.units.Count;
    }
    public void WinGame()
    {
        isWon = true;
        gameOver = true;
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
    public void RestartGame()
    {
        isStarted = false;
        isWon = false;
        isloosed = false;
        isPaused = false;
        gameOver = false;
        score = 0;
        activeBullets?.ForEach(bullet =>
        {
            if (bullet != null)
            {
                Destroy(bullet.gameObject);
                Debug.Log("Destroying bullet");
            }
        });

        activeBullets?.RemoveAll(bullet => bullet == null);
       
    }
    private void PivotHolderRotation()
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
        if (++score == activeBulletCount) 
        {
            gameManager.gameWin();
        }
        Debug.Log("Score " + score + "  Activebullet " + activeBulletCount);
        Debug.Log(spawner.units.Count);
    }

    private void winGame()
    {
        if (score == activeBulletCount) 
        {
            gameManager.gameWin();
        }
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