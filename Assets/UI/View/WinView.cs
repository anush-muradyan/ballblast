using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DefaultNamespace.UI.View
{
    public class WinViewResult : IViewResult
    {
        public UnityEvent OnWin { get; }

        public WinViewResult()
        {
            OnWin = new UnityEvent();
        }

        public void Dispose()
        {
            OnWin?.RemoveAllListeners();
        }
    }

    public class WinView : AbstractView<WinViewResult>
    {
        [SerializeField] private Button winView;

        protected override void OnEnable()
        {
            base.OnEnable();
            winView.onClick.AddListener(Result.OnWin.Invoke);
        }

        protected override void OnDisableInternal()
        {
            base.OnDisableInternal();
            winView.onClick.RemoveListener(Result.OnWin.Invoke);
        }
    }
}