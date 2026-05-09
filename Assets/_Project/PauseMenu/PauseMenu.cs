using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace PauseMenu
{
    public class PauseMenu : MonoBehaviour
    {
        public bool IsPaused
        {
            get => CanvasPauseMenu.activeSelf;
        }
        private static PauseMenu instance;
        public static PauseMenu Instance
        {
            get
            {
                if (instance == null)
                    instance = FindAnyObjectByType<PauseMenu>();

                return instance;
            }
        }
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }

        [SerializeField] private GameObject CanvasPauseMenu;
        public event UnityAction OnGamePaused;
        public event UnityAction OnGameResumed;
        private float OriginalTimeScale = 1f;
        private CursorLockMode OriginalCursorLockMode = CursorLockMode.Locked;
        private void Start()
        {
            ResumeGameAndHideMenu();
        }
        [ContextMenu("Pause")]
        public void PauseGameAndShowMenu()
        {
            OriginalCursorLockMode = Cursor.lockState;
            Cursor.lockState = CursorLockMode.None;

            OriginalTimeScale = Time.timeScale;
            Time.timeScale = 0;

            CanvasPauseMenu.SetActive(true);

            OnGamePaused?.Invoke();
        }
        [ContextMenu("Resume")]
        public void ResumeGameAndHideMenu()
        {
            Cursor.lockState = OriginalCursorLockMode;

            Time.timeScale = OriginalTimeScale;

            CanvasPauseMenu.SetActive(false);
            OnGameResumed?.Invoke();
        }
    }
}