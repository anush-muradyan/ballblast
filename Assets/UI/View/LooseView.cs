using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DefaultNamespace.UI.View {
	
		public class LooseViewResult : IViewResult
		{
			public UnityEvent onRestart { get; }
			public UnityEvent onHome { get; }

			public LooseViewResult()
			{
				onRestart = new UnityEvent();
				onHome = new UnityEvent();
			}

			public void Dispose()
			{
				onRestart?.RemoveAllListeners();
				onHome.RemoveAllListeners();
			}
		}


		public class LooseView : AbstractView<LooseViewResult>
		{
			[SerializeField] private Button restartButton;
			[SerializeField] private Button homeButton;

			protected override void OnEnable()
			{
				base.OnEnable();
				restartButton.onClick.AddListener(Result.onRestart.Invoke);
				homeButton.onClick.AddListener(Result.onHome.Invoke);
			}

			protected override void OnDisableInternal()
			{
				base.OnDisableInternal();
				restartButton.onClick.RemoveListener(Result.onRestart.Invoke);
				restartButton.onClick.RemoveListener(Result.onHome.Invoke);
			}
		}
}
