using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.GameStates;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : MonoBehaviour, IGameStart, IGameEnd, IGameRestart {
	[SerializeField] private Camera camera;
	[SerializeField] private Board board;
	[SerializeField] private DeathZone deathZone;
	[SerializeField] private Transform pivot;
	[SerializeField] private Transform shootPoint;
	[SerializeField] private float viewAngle;
	[SerializeField] private float moveSpeed;
	[SerializeField] private int maxBullets;
	[SerializeField] private float rotationSpeed;
	[SerializeField] private float shootDelay = 0.2f;
	[SerializeField] private Bullet shootingItem;

	private float life=1f;

	private List<Bullet> activeBullets = new List<Bullet>();

	public UnityEvent<int> OnShoot { get; } = new UnityEvent<int>();
	public UnityEvent<float> OnLifeChanged { get; } = new UnityEvent<float>();

	private bool isStarted;
	private bool endGame;

	private float lastShootTime;
	private int shootBullets;

	private void Update() {
		if (!isStarted || endGame) {
			return;
		}

		PivotHolderRotation();
		Shoot();
		checkBounds();
	}

	public void StartGame() {
		isStarted = true;
	}

	public void RestartGame() {
		isStarted = false;
		endGame = false;
		shootBullets = 0;
		lastShootTime = 0;

		activeBullets?.ForEach(bullet => {
			if (bullet != null) {
				Destroy(bullet.gameObject);
			}
		});

		activeBullets?.RemoveAll(bullet => bullet == null);
	}

	private void PivotHolderRotation() {
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

	private void Shoot() {
		if (!Input.GetMouseButtonDown(0)) {
			return;
		}

		shoot();
	}

	private bool canShoot() {
		if (shootBullets >= maxBullets) {
			return false;
		}

		var currentTime = Time.time;
		if (currentTime > lastShootTime) {
			lastShootTime = currentTime + shootDelay;
			return true;
		}

		return false;
	}

	private void shoot() {
		if (!canShoot()) {	
			return;
		}

		var currentPos = camera.ScreenToWorldPoint(Input.mousePosition);
		var dir = currentPos - transform.position;
		var bullet = Instantiate(shootingItem, shootPoint.position, Quaternion.identity);
		bullet.Shoot(dir);
		activeBullets.Add(bullet);
		shootBullets++;
		OnShoot?.Invoke(maxBullets - shootBullets);
	}

	private void checkBounds() {
		activeBullets?.ForEach(bullet => {
			if (bullet == null) {
				return;
			}

			if (bullet.transform.position.y > deathZone.sizeY) {
				Destroy(bullet.gameObject);
			}
		});

		activeBullets?.RemoveAll(bullet => bullet == null);
	}

	public void EndGame(GameEnd gameEnd) {
		endGame = true;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		Destroy(other.gameObject);
		life -= 0.2f;
		OnLifeChanged?.Invoke(life);
		if (!(life <= 0)) return;
		EndGame(GameEnd.Loose);
		Debug.Log("GameEnd");
	}
}