using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DefaultNamespace.UI.View {
	public class GameViewResult : IViewResult {
		public UnityEvent OnBack { get; }
		
		public GameViewResult() {
			OnBack = new UnityEvent();
		}

		public void Dispose() {
			OnBack?.RemoveAllListeners();
			
		}
	}

	public class GameView : AbstractView<GameViewResult> {
		[SerializeField] public  Slider lifeSlider;
		[SerializeField] private TextMeshProUGUI ammoText;
		[SerializeField] private Button backButton;
		[SerializeField] private GameManager gameManager;
		protected override void OnEnable() {
			base.OnEnable();
			backButton.onClick.AddListener(Result.OnBack.Invoke);
		}

		protected override void OnDisableInternal() {
			base.OnDisableInternal();
			backButton.onClick.RemoveListener(Result.OnBack.Invoke);
			gameManager.restartGame();
		}

		public void UpdateLifeSlider(float value) {
			lifeSlider.value = value;
		}

		public void SetAmmoText(string text) {
			ammoText.text = text;
		}
	}
}