using UnityEngine;
using UnityEngine.UI;

public class PowerUpUI : MonoBehaviour
{
    [Header("References")]
    public PlayerController player;

    public Button doubleJumpButton;
    public Text doubleJumpUsesText;

    public Button slowTimeButton;
    public Text slowTimeUsesText;

    public Button unstuckButton;
    public Text unstuckUsesText;

    [Header("Slow Time Settings")]
    public float slowTimeDuration = 3f;

    [Header("Unstuck Settings")]
    public Transform unstuckOrigin;
    public float unstuckRayLength = 20f;
    public float unstuckCheckInterval = 0.1f;

    private float unstuckTimer = 0f;

    private void Start()
    {
        doubleJumpButton?.onClick.AddListener(OnDoubleJumpPressed);
        slowTimeButton?.onClick.AddListener(OnSlowTimePressed);
        unstuckButton?.onClick.AddListener(OnUnstuckPressed);
    }

    private void Update()
    {
        UpdateUI();

        if (unstuckButton != null && unstuckOrigin != null && player != null)
        {
            unstuckTimer += Time.unscaledDeltaTime;
            if (unstuckTimer >= unstuckCheckInterval)
            {
                unstuckButton.interactable = CheckUnstuckAvailable();
                unstuckTimer = 0f;
            }
        }

        if (doubleJumpButton != null && player != null)
        {
            //Only allow double jump while airborne
            doubleJumpButton.interactable = !player.IsGrounded;
        }
    }

    private void UpdateUI()
    {
        if (doubleJumpUsesText != null)
            doubleJumpUsesText.text = "x" + PlayerPrefs.GetInt("DoubleJump", 0);

        if (unstuckUsesText != null)
            unstuckUsesText.text = "x" + PlayerPrefs.GetInt("Unstuck", 0);

        if (slowTimeUsesText != null)
            slowTimeUsesText.text = "x" + PlayerPrefs.GetInt("SlowTime", 0);
    }

    private bool CheckUnstuckAvailable()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(unstuckOrigin.position, Vector2.down, unstuckRayLength);
        foreach (var hit in hits)
        {
            if (hit.collider == null) continue;
            if (hit.collider.CompareTag("LockedBlock")) return true;
            if (hit.collider.CompareTag("Player")) return false;
        }
        return false;
    }

    private void OnDoubleJumpPressed()
    {
        int uses = PlayerPrefs.GetInt("DoubleJump", 0);
        if (uses > 0)
        {
            player?.PerformDoubleJump();
            PlayerPrefs.SetInt("DoubleJump", uses - 1);
            PlayerPrefs.Save();
        }
    }

    private void OnSlowTimePressed()
    {
        int uses = PlayerPrefs.GetInt("SlowTime", 0);
        if (uses > 0)
        {
            player?.ActivateSlowTime(slowTimeDuration);
            PlayerPrefs.SetInt("SlowTime", uses - 1);
            PlayerPrefs.Save();
        }
    }

    private void OnUnstuckPressed()
    {
        int uses = PlayerPrefs.GetInt("Unstuck", 0);
        if (uses > 0)
        {
            player?.UseUnstuck();
            PlayerPrefs.SetInt("Unstuck", uses - 1);
            PlayerPrefs.Save();
        }
    }
}
