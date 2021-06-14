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
			gameManager.OnShoot.AddListener(() => gameView.SetAmmoText($"{Player.bulletCount}"));
		
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