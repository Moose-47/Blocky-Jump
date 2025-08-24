using System.Collections.Generic;
using UnityEngine;

public static class ProfanityFilter
{
    //Hashset to store banned words for fast lookup
    private static HashSet<string> bannedWords;

    //Flag to ensure we only load the words once
    private static bool initialized = false;

    private static void Initialize()
    {
        if (initialized) return;

        bannedWords = new HashSet<string>(); //Create the HashSet

        //Load the text file from Resources folder
        TextAsset profanityList = Resources.Load<TextAsset>("profanity");
        if (profanityList == null)
        {
            Debug.LogError("Profanity file not found in Resources/profanity.txt");
            initialized = true;
            return;
        }

        //Split the file into lines, removing empty entries
        string[] words = profanityList.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string word in words)
        {
            //Trim whitespace and convert to lowercase, then add to HashSet
            bannedWords.Add(word.Trim().ToLower());
        }

        initialized = true;
        Debug.Log($"Loaded {bannedWords.Count} banned words");
    }

    public static bool ContainsProfanity(string input)
    {
        Initialize(); //Ensure banned words are loaded

        if (string.IsNullOrEmpty(input) || bannedWords == null)
            return false;

        //Normalize input: lowercase and remove spaces
        string lowerInput = input.ToLower().Replace(" ", "");

        //Check if any banned words exist in the input
        foreach (string word in bannedWords)
        {
            if (lowerInput.Contains(word))
                return true; 
        }

        return false;
    }
}