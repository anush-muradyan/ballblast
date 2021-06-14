using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.GameStates;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace {
	public enum GameState {
		None,
		Started,
		Pause,
		Resume,
		Restart,
		Win,
		Loose
	}

	public struct GameInit {
		public float Life { get; }
		public float BulletCount { get; }

		public GameInit(float life, float bulletCount) {
			Life = life;
			BulletCount = bulletCount;
		}
	}

	public class GameManager : MonoBehaviour {
		[SerializeField] private Player player;
		[SerializeField] private GameObject winPanel;
		[SerializeField] private Spawner spawner;

		public GameState gameState = GameState.None;
		private Dictionary<Type, List<IGameState>> gameStates;
		public UnityEvent<GameInit> OnStartGame { get; } = new UnityEvent<GameInit>();
		public UnityEvent<int> OnShoot { get; } = new UnityEvent<int>();
		public UnityEvent<float> OnPlayerLifeChanged { get; } = new UnityEvent<float>();

		private void Awake() {
			Prepare();
		}

		private void OnEnable() {
			player.OnShoot.AddListener(OnShoot.Invoke);
			player.OnLifeChanged.AddListener(OnPlayerLifeChanged.Invoke);
			DynamicObjectsController.Instance.OnNotify.AddListener(handleDynamicObjects);
		}

		private void OnDisable() {
			player.OnShoot.RemoveListener(OnShoot.Invoke);
			player.OnLifeChanged.RemoveListener(OnPlayerLifeChanged.Invoke);
			DynamicObjectsController.Instance.OnNotify.RemoveListener(handleDynamicObjects);
		}

		public void Prepare() {
			gameStates = new Dictionary<Type, List<IGameState>>();
			AddType<IGameStart>();
			AddType<IGamePause>();
			AddType<IGameRestart>();
			AddType<IGameResume>();
		}

		private void AddType<T>() where T : class {
			gameStates.Add(typeof(T), Utils.GetInterfaces<T, IGameState>());
		}

		private List<T> ResolveType<T>() where T : class {
			var type = typeof(T);
			if (!gameStates.ContainsKey(type)) {
				Debug.LogError("Can not find that type: " + type);
				return null;
			}

			return gameStates[type].Select(state => state as T).ToList();
		}

		private void handleDynamicObjects(IDynamicObject dynamicObject) {
			addInterface<IGameStart>(dynamicObject);
			addInterface<IGamePause>(dynamicObject);
			addInterface<IGameRestart>(dynamicObject);
			addInterface<IGameResume>(dynamicObject);
		}

		private void addInterface<T>(IDynamicObject dynamicObject) {
			var type = typeof(T);
			if (!gameStates.ContainsKey(type)) {
				Debug.LogError("Can not find that type: " + type);
				return;
			}

			var inter = dynamicObject.GetInterface<T>();
			if (inter == null) {
				return;
			}

			gameStates[type].Add(inter as IGameState);
		}

		private void Update() {
			if (Input.GetKeyDown(KeyCode.Q)) {
				StartGame();
			}

			if (Input.GetKeyDown(KeyCode.W)) {
				pauseGame();
			}

			if (Input.GetKeyDown(KeyCode.E)) {
				resumeGame();
			}

			if (Input.GetKeyDown(KeyCode.R)) {
				restartGame();
			}

			if (Input.GetKeyDown(KeyCode.T)) {
				gameWin();
			}

			if (Input.GetKeyDown(KeyCode.Y)) {
				gameLoose();
			}

			if (Input.GetKeyDown(KeyCode.U)) {
				Debug.Log("enemy created");
			}
		}


		public void StartGame() {
			gameState = GameState.Started;
			OnStartGame?.Invoke(new GameInit(1f,8));
			ResolveType<IGameStart>()
				.ForEach(start => start.StartGame());
		}

		public void pauseGame() {
			gameState = GameState.Pause;
			ResolveType<IGamePause>()
				.ForEach(pause => pause.PauseGame());
		}

		public void resumeGame() {
			gameState = GameState.Resume;
			spawner.Init();
			ResolveType<IGameResume>().ForEach(resume => resume.ResumeGame());
		}

		public void restartGame() {
			winPanel.SetActive(false);
			gameState = GameState.Restart;
			ResolveType<IGameRestart>().ForEach(restart => restart.RestartGame());
		}

		public void gameWin() {
			gameState = GameState.Win;
			ResolveType<IGameEnd>().ForEach(win => win.EndGame(GameEnd.Win));
			winPanel.SetActive(true);
		}

		private void gameLoose() {
			gameState = GameState.Loose;
			resumeGame();
			if (Input.GetKeyDown(KeyCode.M)) {
				resumeGame();
			}
		}
	}
}