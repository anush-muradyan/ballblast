using UnityEngine;

namespace DefaultNamespace.UI.View
{
    public class LooseAbstractView:AbstractView
    {
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