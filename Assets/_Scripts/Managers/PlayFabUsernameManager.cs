using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayFabUsernameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject usernamePanel;
    public TMP_InputField usernameInput;
    public Button submitButton;
    public TMP_Text feedbackText;

    private const string PlayerPrefsKey = "PlayerUsername";

    private void Start()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKey))
        {
            SceneManager.LoadScene("Main Menu");
            return;
        }

        usernamePanel.SetActive(true);
        feedbackText.text = "";
        submitButton.onClick.AddListener(OnSubmitUsername);
        
    }

    private void OnSubmitUsername()
    {
        string username = usernameInput.text.Trim();

        if (string.IsNullOrEmpty(username))
        {
            feedbackText.text = "Username cannot be empty.";
            return;
        }

        if (username.Length > 16)
        {
            feedbackText.text = "Username cannot exceed 16 character.";
            return;
        }

        submitButton.interactable = false;

        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = username
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request,
            result =>
            {
                PlayerPrefs.SetString(PlayerPrefsKey, result.DisplayName);
                PlayerPrefs.Save();
                SceneManager.LoadScene("Main Menu");
            },
            error =>
            {
                submitButton.interactable = true;

                if (error.Error == PlayFabErrorCode.InvalidParams)
                    feedbackText.text = "Username contains inappropriate language or invalid characters.";
                else if (error.Error == PlayFabErrorCode.NameNotAvailable)
                    feedbackText.text = "Username is unavailable, try another username.";
                else
                    feedbackText.text = "Error setting username. Please try again.";

                Debug.LogError("PlayFab username error: " + error.GenerateErrorReport());
            });
    }

    private void OnDestroy()
    {
        submitButton.onClick.RemoveAllListeners();
    }
}
