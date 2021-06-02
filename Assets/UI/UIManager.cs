using System;
using DefaultNamespace.UI.View;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace DefaultNamespace.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private HomeAbstractView homeAbstractView;
        [SerializeField] private GameAbstractView gameAbstractView;
        [SerializeField] private WinAbstractView winAbstractView;
        [SerializeField] private LooseAbstractView looseAbstractView;
        private AbstractView view;


        private void Start()
        {
            ShowHomeView();
        }

        private void closeActive()
        {
            if (view == null)
            {
                return;
            }

            view.Close();
        }

        public void ShowHomeView()
        {
            closeActive();
            homeAbstractView.OnPlay.AddListener(ShowGameView);
            homeAbstractView.Show();
            view = homeAbstractView;
        }

        public void ShowGameView()
        {
            closeActive();
            gameAbstractView.Show();

            view = gameAbstractView;
        }

        public void ShowWinView()
        {
            closeActive();
            winAbstractView.Show();
            view = winAbstractView;
        }

        public void ShowLooseView()
        {
            closeActive();
            looseAbstractView.Show();
            view = looseAbstractView;
        }
    }
}