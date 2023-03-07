using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReadInput : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TextMeshProUGUI playerMessage;

    public string playerName;

    public void SaveName()
    {
        Debug.Log("Here is the input field: " + nameInput.text);
        playerName = nameInput.text;
        if (playerName != "")
        {
            Debug.Log("Player name is set. Here it is: " + playerName);
            StatsManager.Instance.playerName = playerName;
            StatsManager.Instance.gameReady = true;
        } else
        {
            Debug.Log(" Player name is not set...");
            StatsManager.Instance.gameReady = false;
        }
    }
}
