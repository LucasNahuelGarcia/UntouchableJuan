using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField][Range(0f, 1f)] private float TimeStopSpeed = 1f;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = (GameManager)FindObjectOfType(typeof(GameManager));

            return instance;
        }
    }
    private static GameManager instance;
    public UnityEvent<int> OnEnemyCountChanged = new UnityEvent<int>();
    public UnityEvent OnGameOver = new UnityEvent();
    public int EnemyCount
    {
        get
        {
            return enemyCount;
        }
        private set
        {
            enemyCount = value;
            OnEnemyCountChanged.Invoke(value);
        }
    }
    private int enemyCount = 0;
    private void Start()
    {
        enemyCount = 0;
    }
    public void IncreaseEnemyCount()
    {
        EnemyCount++;
    }
    [SerializeField]
    public GameObject Player;

    [ContextMenu("End Game")]
    public void GameOver()
    {
        OnGameOver.Invoke();
        Debug.Log("GameOver");
        StartCoroutine(StopTimeGradually(TimeStopSpeed));
    }

    [ContextMenu("Restart Game")]
    public void RestartGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator StopTimeGradually(float timeStopSpeed)
    {
        while (Time.timeScale > 0)
        {
            float newTimeScale = Time.timeScale;
            newTimeScale -= timeStopSpeed * Time.unscaledDeltaTime;
            if (newTimeScale < 0) newTimeScale = 0;
            Time.timeScale = newTimeScale;
            yield return new WaitForEndOfFrame();
        }
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
