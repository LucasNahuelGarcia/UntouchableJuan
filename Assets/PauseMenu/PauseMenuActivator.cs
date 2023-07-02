using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace PauseMenu
{
    public class PauseMenuActivator : MonoBehaviour
    {
        public UnityEvent OnGamePaused = new UnityEvent();
        public UnityEvent OnGameResumed = new UnityEvent();

        private void OnEnable()
        {
            PauseMenu.Instance.OnGamePaused += OnGamePaused.Invoke;
            PauseMenu.Instance.OnGameResumed += OnGameResumed.Invoke;
        }
        private void OnDisable()
        {
            if (PauseMenu.Instance == null)
                return;
            PauseMenu.Instance.OnGamePaused -= OnGamePaused.Invoke;
            PauseMenu.Instance.OnGameResumed -= OnGameResumed.Invoke;
        }
        public void OnPause(InputValue value)
        {
            if (PauseMenu.Instance.IsPaused)
                PauseMenu.Instance.ResumeGameAndHideMenu();
            else
                PauseMenu.Instance.PauseGameAndShowMenu();
        }
    }
}