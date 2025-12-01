using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text pointsText;

    public Button buyDoubleJumpButton;
    public TMP_Text doubleJumpUsesText;

    public Button buyUnstuckButton;
    public TMP_Text unstuckUsesText;

    public Button buySlowTimeButton;
    public TMP_Text slowTimeUsesText;

    [Header("Power-Up Costs")]
    public int doubleJumpCost = 100;
    public int unstuckCost = 250;
    public int slowTimeCost = 500;

    private void Start()
    {
        buyDoubleJumpButton.onClick.AddListener(() => PurchasePowerUp("DoubleJump", doubleJumpCost));
        buyUnstuckButton.onClick.AddListener(() => PurchasePowerUp("Unstuck", unstuckCost));
        buySlowTimeButton.onClick.AddListener(() => PurchasePowerUp("SlowTime", slowTimeCost));

        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    ///<summary>
    ///Attempts to purchase a power-up.
    ///</summary>
    ///<param name="powerUpKey">Key to store in PlayerPrefs (DoubleJump, Unstuck, SlowTime)</param>
    ///<param name="cost">Cost of this power-up</param>
    private void PurchasePowerUp(string powerUpKey, int cost)
    {
        int currentPoints = PlayerPrefs.GetInt("Points", 0);

        if (currentPoints >= cost)
        {
            //Subtract points
            currentPoints -= cost;
            PlayerPrefs.SetInt("Points", currentPoints);

            //Add 1 use to power-up
            int currentUses = PlayerPrefs.GetInt(powerUpKey, 0);
            currentUses++;
            PlayerPrefs.SetInt(powerUpKey, currentUses);

            PlayerPrefs.Save();
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough points to purchase " + powerUpKey);
        }
    }

    private void UpdateUI()
    {
        int currentPoints = PlayerPrefs.GetInt("Points", 0);
        if (pointsText != null)
            pointsText.text = "" + currentPoints;

        if (doubleJumpUsesText != null)
            doubleJumpUsesText.text = "x" + PlayerPrefs.GetInt("DoubleJump", 0);

        if (unstuckUsesText != null)
            unstuckUsesText.text = "x" + PlayerPrefs.GetInt("Unstuck", 0);

        if (slowTimeUsesText != null)
            slowTimeUsesText.text = "x" + PlayerPrefs.GetInt("SlowTime", 0);
    }
}
