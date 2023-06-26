using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int EnemyCount
    {
        get
        {
            return enemyCount;
        }
        set
        {
            enemyCount = value;
            EnemyCounter.text = "" + enemyCount;
        }
    }

    private int enemyCount = 0;

    [SerializeField]
    public GameObject Player;
    [SerializeField]
    public TextMeshPro EnemyCounter;
    [SerializeField]
    public float GameOverForce = 5f;
    private void Awake()
    {
        EnemyCounter.text = enemyCount.ToString();
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void GameOver()
    {
        Rigidbody rigidbody = Player.GetComponent<Rigidbody>();

        rigidbody.isKinematic = false;
        rigidbody.freezeRotation = false;
        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.useGravity = true;
        rigidbody.AddForce(Vector3.up * GameOverForce, ForceMode.Impulse);
        rigidbody.AddTorque(Vector3.left * GameOverForce, ForceMode.Impulse);
        // Time.timeScale = .1f;
        Player.GetComponent<FirstPersonController>().enabled = false;
        Debug.Log("GameOver");
    }
    public void QuitGame()
    {
        Debug.Log("quitting");
        Application.Quit();
    }
}
