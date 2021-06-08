using System;
using DefaultNamespace.UI.View;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
	public class GamePlayHandler : IDisposable
	{
		private GameView gameView;
		private GameManager gameManager;
		private WinView winView;

		public GamePlayHandler(GameView gameView, GameManager gameManager,WinView winView)
		{
			this.gameView = gameView;
			this.gameManager = gameManager;
			this.winView = winView;
			initEvents();
		}

		private void initEvents()
		{
			gameManager.OnShoot.AddListener(() => gameView.SetAmmoText(Random.value.ToString()));
			
		}

		private void resetEvents()
		{
			gameManager.OnShoot.RemoveAllListeners();
		}

		public void StartGame()
		{
			gameManager.StartGame();
		}

		public void PauseGame()
		{
			gameManager.pauseGame();
		}

		public void ResumeGame()
		{
			gameManager.resumeGame();
		}

		public void Dispose()
		{
			resetEvents();
		}
	}
}