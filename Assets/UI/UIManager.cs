using DefaultNamespace.UI.View;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI {
	public class UIManager : MonoBehaviour {
		[SerializeField] private HomeView homeView;
		[SerializeField] private GameView gameView;
		[SerializeField] private WinView winView;
		[SerializeField] private LooseView looseView;
		[SerializeField] private GameManager gameManager;
		private AbstractView _view;

		private AbstractView view {
			get => _view;
			set {
				if (_view != null) {
					_view.Hide();
				}

				_view = value;
			}
		}


		private void Start() {
			ShowHomeView();
		}

		public void ShowHomeView() {
			homeView.Result.OnPlay.AddListener(ShowGameView);
			view = homeView.Show();
		
		}
		
		
		public void ShowGameView() {
			var gameplay = new GamePlayHandler(gameView, gameManager,winView);
			gameView.Result.OnBack.AddListener(ShowHomeView);
			gameView.Result.OnBack.AddListener(gameplay.Dispose);
			view = gameView.Show();
			gameplay.StartGame();

			if (gameManager.gameState == GameState.Win)
			{
				Debug.Log("Here");
				ShowWinView();
			}

		}

		public void ShowWinView() {
			var winPlay = new GamePlayHandler(gameView, gameManager,winView);
			view = winView.Show();
			winView.Result.OnWin.AddListener(ShowHomeView);
			gameView.Result.OnBack.AddListener(winPlay.Dispose);
		}

		public void ShowLooseView() {
			view = looseView.Show();
		}
	}
}