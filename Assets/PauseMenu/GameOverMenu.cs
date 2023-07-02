using UnityEngine;
using UnityEngine.InputSystem;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject CanvasGameOverMenu;
    void Start()
    {
        CanvasGameOverMenu.SetActive(false);
    }

    public void ShowGameOverMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        CanvasGameOverMenu.SetActive(true);
    }
}
