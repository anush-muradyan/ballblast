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
				Debug.Log("Here");
				ShowWinView();
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
			var gameplay = new GamePlayHandler(gameView, gameManager, winView);
			gameView.Result.OnBack.AddListener(ShowHomeView);
			gameView.Result.OnBack.AddListener(gameplay.Dispose);
			gameplay.StartGame();
			view = gameView.Show();
		}

		public void ShowWinView()
		{
			var gameplay = new GamePlayHandler(gameView, gameManager, winView);
			winView.Result.onHome.AddListener(ShowHomeView);
			winView.Result.onHome.AddListener(gameplay.Dispose);
			winView.Result.onRestart.AddListener(ShowGameView);
			winView.Result.onRestart.AddListener(gameplay.Dispose);
			view = winView.Show();
		}

		public void ShowLooseView()
		{
			view = looseView.Show();
		}
	}
}