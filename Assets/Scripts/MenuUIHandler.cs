using System.Collections;
using System.Collections.Generic;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;      //lncluded only in Editor
#endif

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;

public class MenuUIHandler : MonoBehaviour
{
    
    public TMP_InputField inputName;
    public TMP_Text bestScoreText;

    private void Start()
    {
        bestScoreText.text = "Best Score: " + GameManager.Instance.bestPlayer + " : " + GameManager.Instance.highestScore;
    }

    public void StartGame()
    {
        if (inputName.text != string.Empty ) 
        {
            bool exists = GameManager.Instance.playerInfo.Exists(p => p.playerName == inputName.text);
            if (!exists) 
            {
                GameManager.Instance.currentPlayerName = inputName.text;
                SceneManager.LoadScene(1);
            }
            
        }
    }

    public void ExitGame()
    {
        GameManager.Instance.SaveData();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif 
    }
    public void RankingMenu()
    {
        SceneManager.LoadScene(2);
    }
}
