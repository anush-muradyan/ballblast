using DefaultNamespace.UI.View;
using UnityEngine;

namespace DefaultNamespace.UI
{
	public class UIManager : MonoBehaviour
	{
		[SerializeField] private HomeView homeView;
		[SerializeField] private GameView gameView;
		[SerializeField] private WinView winView;
		[SerializeField] private LooseView looseView;
		[SerializeField] private GameManager gameManager;

		private AbstractView _view;

		private AbstractView view
		{
			get => _view;
			set
			{
				if (_view != null)
				{
					_view.Hide();
				}

				_view = value;
			}
		}

		private void Update()
		{
			if (gameManager.gameState == GameState.Win)
			{
				ShowWinView();
			}
			
			if (gameManager.gameState == GameState.Loose)
			{
				ShowLooseView();
			}
		}

		private void Start()
		{
			ShowHomeView();
		}

		public void ShowHomeView()
		{
			homeView.Result.OnPlay.AddListener(ShowGameView);
			view = homeView.Show();
		}

		public void ShowGameView()
		{
			var gameplay = new GamePlayHandler(gameView, gameManager);
			gameView.Result.OnBack.AddListener(ShowHomeView);
			gameView.Result.OnBack.AddListener(gameplay.Dispose);
			view = gameView.Show();
			gameplay.StartGame();
		}

		public void ShowWinView()
		{
			winView.Result.onHome.AddListener(ShowHomeView);
			winView.Result.onRestart.AddListener(ShowGameView);
			view = winView.Show();
		}

		public void ShowLooseView()
		{
			looseView.Result.onHome.AddListener(ShowHomeView);
			looseView.Result.onRestart.AddListener(ShowGameView);
			view = looseView.Show();
		}
	}
}