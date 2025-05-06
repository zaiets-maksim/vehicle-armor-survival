using System;

namespace Connect4.Scripts.Infrastructure
{
    public interface ILoadingCurtain
    {
        public void SetDelay(float delay);
        event Action OnComplete;
        void Show();
        void Hide();
    }
}