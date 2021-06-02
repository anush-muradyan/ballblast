using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DefaultNamespace.UI.View
{
    public class HomeAbstractView : AbstractView
    {
        [SerializeField] private Button playGame;
        public UnityEvent OnPlay { get; } = new UnityEvent();

        protected override void OnEnable()
        {
            playGame.onClick.AddListener(onPlayGameButtonClick);
        }

        protected override void OnDisable()
        {
            playGame.onClick.RemoveListener(onPlayGameButtonClick);
            OnPlay?.RemoveAllListeners();
        }

        private void onPlayGameButtonClick()
        {
            OnPlay?.Invoke();
        }

        public override void Show()
        {
            gameObject.SetActive(true);
        }

        public override void Close()
        {
            gameObject.SetActive(false);
        }
    }
}