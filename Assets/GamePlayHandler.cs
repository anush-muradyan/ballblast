using System;
using DefaultNamespace.UI.View;

namespace DefaultNamespace
{
	public class GamePlayHandler : IDisposable
	{
		private GameView gameView;
		private GameManager gameManager;
		

		public GamePlayHandler(GameView gameView, GameManager gameManager)
		{
			this.gameView = gameView;
			this.gameManager = gameManager;
			initEvents();
		}

		private void initEvents()
		{
			gameManager.OnStartGame.AddListener(gameView.Init);
			gameManager.OnShoot.AddListener((amount) => gameView.SetAmmoText($"{amount}"));
			gameManager.OnPlayerLifeChanged.AddListener(life => gameView.UpdateLifeSlider(life));
		}

		private void resetEvents()
		{
			gameManager.OnShoot.RemoveAllListeners();
			gameManager.OnStartGame.RemoveAllListeners();
			gameManager.OnPlayerLifeChanged.RemoveAllListeners();
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