using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isDead = false;

    private int Score;
    public int _score
    {
        get => Score;
        set => Score = value;
    }

    private int highScore;
    public int Highscore => highScore;

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
        isDead = true;
        Time.timeScale = 0f;
    }
}
    
