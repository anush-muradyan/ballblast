using System;
using DefaultNamespace.UI.View;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace {
	public class GamePlayHandler : IDisposable {
		private GameView gameView;
		private GameManager gameManager;

		public GamePlayHandler(GameView gameView, GameManager gameManager) {
			this.gameView = gameView;
			this.gameManager = gameManager;
			initEvents();
		}

		private void initEvents() {
			gameManager.OnSoot.AddListener(() => gameView.SetAmmoText(Random.value.ToString()));
		}

		private void resetEvents() {
			gameManager.OnSoot.RemoveAllListeners();
		}

		public void StartGame() {
			gameManager.StartGame();
		}

		public void Dispose() {
			resetEvents();
		}
	}
}