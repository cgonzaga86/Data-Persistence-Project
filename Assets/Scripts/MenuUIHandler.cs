using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro;

public class MenuUIHandler : MonoBehaviour
{
    public TextMeshProUGUI playerMessage;
    // Start the main game
    public void StartGame()
    {
        if (StatsManager.Instance.gameReady)
        {
            SceneManager.LoadScene(1);
        } else
        {
            SetPlayerMessage();
        }
    }

    // Quit the game
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    // Set Player Message
    private void SetPlayerMessage()
    {
        playerMessage.text = "Please set a player name before starting!";
    }
}
