using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighScoreUIHandler : MonoBehaviour
{
    public Button backButton;
    public TMP_Text playerScoreList;

    private void Start()
    {
        playerScoreList.text = string.Empty;
        if (GameManager.Instance.playerInfo != null)
        foreach (var player in GameManager.Instance.playerInfo)
            playerScoreList.text += player.playerName + " : " + player.playerScore + "\n";
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
