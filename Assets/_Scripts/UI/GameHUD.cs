using TMPro;
using UnityEngine;

public class GameHUD : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text timerText;
    public TMP_Text scoreText;
    public GameObject gameOverPanel;

    private float elapsedTime = 0f;

    private void Start()
    {
        gameOverPanel.SetActive(false);
        ResetHUD();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        UpdateTimerUI();
        UpdateScoreUI();

        if (GameManager.Instance.isDead)
        {
            OpenGameOverPanel();
        }
    }

    public void ResetHUD()
    {
        elapsedTime = 0f;
        UpdateScoreUI();
        UpdateTimerUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null && GameManager.Instance != null)
            scoreText.text = $"{GameManager.Instance._score}";
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    private void OpenGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }
}
