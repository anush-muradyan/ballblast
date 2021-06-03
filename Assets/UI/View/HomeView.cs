using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DefaultNamespace.UI.View {
	public class HomeViewResult : IViewResult {
		public UnityEvent OnPlay { get; }

		public HomeViewResult() {
			OnPlay = new UnityEvent();
		}

		public void Dispose() {
			OnPlay?.RemoveAllListeners();
		}
	}

	public class HomeView : AbstractView<HomeViewResult> {
		[SerializeField] private Button playGame;

		protected override void OnEnable() {
			playGame.onClick.AddListener(Result.OnPlay.Invoke);
		}

		protected override void OnDisableInternal() {
			playGame.onClick.RemoveListener(Result.OnPlay.Invoke);
		}
	}
}