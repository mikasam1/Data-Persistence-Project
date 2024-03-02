using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;      
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using static GameManager;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text bestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        if (GameManager.Instance != null)   //方便测试时直接从main scene进入
            bestScoreText.text = "Best Score: " + GameManager.Instance.bestPlayer + " : " + GameManager.Instance.highestScore;
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)        //esc回主菜单 space重新开始
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        if (GameManager.Instance != null)
        {
            int length = GameManager.Instance.playerInfo.Count;

            if (length < 3)
            {
                bool exist = GameManager.Instance.playerInfo.Exists(p => p.playerName == GameManager.Instance.currentPlayerName);
                if (!exist)
                {
                    PlayerInfo player = new();
                    player.playerName = GameManager.Instance.currentPlayerName;
                    player.playerScore = m_Points;
                    GameManager.Instance.playerInfo.Add(player);
                    GameManager.Instance.SortPlayerScore();
                }
                else 
                {
                    var player = GameManager.Instance.playerInfo.FirstOrDefault(p => p.playerName == GameManager.Instance.currentPlayerName);
                    if (m_Points > player.playerScore)
                    {
                        GameManager.Instance.playerInfo.Remove(player);
                        var updatePlayer = new PlayerInfo()
                        {
                            playerName = GameManager.Instance.currentPlayerName,
                            playerScore = m_Points
                        };
                        GameManager.Instance.playerInfo.Add(updatePlayer);
                        GameManager.Instance.SortPlayerScore();
                    }
                }
            }
            else if (m_Points > GameManager.Instance.playerInfo[length - 1].playerScore)
            {
                GameManager.Instance.playerInfo.RemoveAt(length - 1);
                PlayerInfo player = new();
                player.playerName = GameManager.Instance.currentPlayerName;
                player.playerScore = m_Points;
                GameManager.Instance.playerInfo.Add(player);
                GameManager.Instance.SortPlayerScore();
            }
            GameManager.Instance.highestScore = GameManager.Instance.playerInfo[0].playerScore;
            GameManager.Instance.bestPlayer = GameManager.Instance.playerInfo[0].playerName;            
        }
        m_GameOver = true;
        GameOverText.SetActive(true);
        if(GameManager.Instance != null)
            bestScoreText.text = "Best Score: " + GameManager.Instance.bestPlayer + " : " + GameManager.Instance.highestScore;
    }
    
}
