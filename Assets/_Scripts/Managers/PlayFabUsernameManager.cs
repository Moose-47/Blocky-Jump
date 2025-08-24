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
    private bool isLoggedIn = false;

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);    
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Logged into PlayFab");

        isLoggedIn = true;

        if (PlayerPrefs.HasKey(PlayerPrefsKey))
        {
            SceneManager.LoadScene("Main Menu");
            return;
        }

        usernamePanel.SetActive(true);
        feedbackText.text = "";
        submitButton.onClick.AddListener(OnSubmitUsername);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Login Failed: " + error.GenerateErrorReport());
        feedbackText.text = "Failed to connect to server. Please restart.";
    }    

    private void OnSubmitUsername()
    {
        if (!isLoggedIn)
        {
            feedbackText.text = "Not logged in yet. Please wait.";
            return;
        }

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

        if (ProfanityFilter.ContainsProfanity(username))
        {
            feedbackText.text = "Username cannot contain inappropriate language.";
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
                    feedbackText.text = "Username cannot contain invalid characters.";
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
