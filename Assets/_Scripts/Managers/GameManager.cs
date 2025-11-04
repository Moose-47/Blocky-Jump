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
        if (isDead) return;

        isDead = true;

        //Add current score to persistent points
        int totalPoints = PlayerPrefs.GetInt("Points", 0);
        totalPoints += _score;
        PlayerPrefs.SetInt("Points", totalPoints);
        PlayerPrefs.Save();

        Time.timeScale = 0f;
    }
}
    
