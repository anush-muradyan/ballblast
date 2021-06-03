using DefaultNamespace.UI.View;
using UnityEngine;

namespace DefaultNamespace.UI {
	public class UIManager : MonoBehaviour {
		[SerializeField] private HomeView homeView;
		[SerializeField] private GameView gameView;
		[SerializeField] private WinAbstractView winAbstractView;
		[SerializeField] private LooseAbstractView looseAbstractView;

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
			var gameplay = new GamePlayHandler(gameView, gameManager);
			gameView.Result.OnBack.AddListener(ShowHomeView);
			gameView.Result.OnBack.AddListener(gameplay.Dispose);
			view = gameView.Show();
			gameplay.StartGame();
		}

		public void ShowWinView() {
			view = winAbstractView.Show();
		}

		public void ShowLooseView() {
			view = looseAbstractView.Show();
		}
	}
}