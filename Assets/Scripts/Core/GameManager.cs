using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
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

    public void IncreaseEnemyCount()
    {
        EnemyCount++;
    }
    [SerializeField]
    public GameObject Player;
    public void GameOver()
    {
        OnGameOver.Invoke();
        Debug.Log("GameOver");
    }
}
