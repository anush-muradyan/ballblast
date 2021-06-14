using System;
using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.GameStates;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : MonoBehaviour, IGameStart, IGameEnd, IGamePause, IGameResume, IGameRestart
{
	[SerializeField] private Camera camera;
	[SerializeField] private Board board;
	[SerializeField] private DeathZone deathZone;
	[SerializeField] private Transform pivot;
	[SerializeField] private Transform shootPoint;
	[SerializeField] private Slider lifeSlider;
	[SerializeField] private float viewAngle;
	[SerializeField] private float moveSpeed;
	
	[SerializeField] private float rotationSpeed;

	[SerializeField] private Bullet shootingItem;
	public static int bulletCount = 10;
	private List<Bullet> activeBullets = new List<Bullet>();


	public UnityEvent OnShoot { get; } = new UnityEvent();

	private int score = 0;
	private bool _canShoot;

	private bool isStarted;

	private bool isPaused;
	private bool gameOver;
	private bool endGame;
	private Vector2 velocity;
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

		if (endGame)
		{
			return;
		}

		PivotHolderRotation();
		Shoot();
		checkBounds();
	}

	public void StartGame()
	{
		isStarted = true;
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
		isPaused = false;
		gameOver = false;
		endGame = false;
		_canShoot = true;
		score = 0;
		lifeSlider.value = lifeSlider.maxValue;
		bulletCount = 10;
		
		activeBullets?.ForEach(bullet =>
		{
			if (bullet != null)
			{
				Destroy(bullet.gameObject);
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

		if (bulletCount <= 0)
		{
			_canShoot = false;
			return;
		}
		

		if (_canShoot)
		{
			bulletCount--;
			StartCoroutine(wait());
			_canShoot = false;
			var currentPos = camera.ScreenToWorldPoint(Input.mousePosition);
			var dir = currentPos - transform.position;

			var bullet = Instantiate(shootingItem, shootPoint.position, Quaternion.identity);

			bullet.Shoot(dir);
			activeBullets.Add(bullet);
			OnShoot?.Invoke();
			Debug.Log(bulletCount);
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

	public void EndGame(GameEnd gameEnd)
	{
		endGame = true;
		
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		
		 lifeSlider.value -= 0.5f;
		 Destroy(other.gameObject);
		 Debug.Log(lifeSlider.value);
		 if (lifeSlider.value <= 0)
		 {
			 EndGame(GameEnd.Loose);
			 Debug.Log("GameEnd");
			 var a=FindObjectOfType<GameManager>().gameState=GameState.Loose;
			
		 } 
	}
}