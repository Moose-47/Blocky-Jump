using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int Score;
    public int _score
    {
        get => Score;
        set => Score = value;
    }

    private int highScore;
    public int Highscore => highScore;

    [Header("UI References")]
    public GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        highScore = PlayerPrefs.GetInt("Highscore", 0);
    }

    public void PlayerDeath()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
    
