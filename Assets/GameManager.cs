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

	public class GameManager : MonoBehaviour {
		[SerializeField] private Player player;
		[SerializeField] private GameObject winPanel;
		private GameState gameState = GameState.None;
		private Dictionary<Type, List<IGameState>> gameStates;
		public UnityEvent OnSoot => player.OnShoot;
		private void Start() {
			Prepare();
		}

		private void OnEnable() {
			DynamicObjectsController.Instance.OnNotify.AddListener(handleDynamicObjects);
		}

		private void OnDisable() {
			DynamicObjectsController.Instance.OnNotify.RemoveListener(handleDynamicObjects);
		}

		public void Prepare() {
			gameStates = new Dictionary<Type, List<IGameState>>();
			AddType<IGameStart>();
			AddType<IGamePause>();
			AddType<IGameLoose>();
			AddType<IGameRestart>();
			AddType<IGameResume>();
			AddType<IGameWin>();
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
			addInterface<IGameLoose>(dynamicObject);
			addInterface<IGameRestart>(dynamicObject);
			addInterface<IGameResume>(dynamicObject);
			addInterface<IGameWin>(dynamicObject);
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
				restartGame(); //TODO: implement ME!
			}

			if (Input.GetKeyDown(KeyCode.T)) {
				gameWin(); //TODO: Need to be fixed
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
			ResolveType<IGameStart>()
				.ForEach(start => start.StartGame());
		}

		private void pauseGame() {
			gameState = GameState.Pause;
			ResolveType<IGamePause>().ForEach(pause => pause.PauseGame());
		}

		private void resumeGame() {
			gameState = GameState.Resume;
			ResolveType<IGameResume>().ForEach(resume => resume.ResumeGame());
		}

		private void restartGame() {
			winPanel.SetActive(false);
			gameState = GameState.Restart;
			ResolveType<IGameRestart>().ForEach(restart => restart.RestartGame());
		}

		public void gameWin() {
			gameState = GameState.Win;
			ResolveType<IGameWin>().ForEach(win => win.WinGame());
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