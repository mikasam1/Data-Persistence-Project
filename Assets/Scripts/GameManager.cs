using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;      //lncluded only in Editor
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using static UnityEditor.Experimental.GraphView.GraphView;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int highestScore;
    public string bestPlayer;
    public List<PlayerInfo> playerInfo;

    [Serializable]
    public struct PlayerInfo
    {
        public string playerName; 
        public int playerScore;
    }

    public string currentPlayerName;

    private bool isLoaded = false;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadData();
    }
    private void Start()
    {
        if (!isLoaded)
        {
            highestScore = 0;
            bestPlayer = string.Empty;
            playerInfo = new List<PlayerInfo>();
        }
    }
    [Serializable]
    class SaveDataClass
    {
        public int highestScore;
        public string BestPlayer;
        public List<PlayerInfo> playerInfo;
    }
    public void SaveData()
    {
        SaveDataClass data = new SaveDataClass();
        data.highestScore = highestScore;
        data.BestPlayer = bestPlayer;
        data.playerInfo = playerInfo;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savedata.json", json);
    }
    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savedata.json";
        if (File.Exists(path)) 
        {
            string json = File.ReadAllText(path);
            SaveDataClass data = JsonUtility.FromJson<SaveDataClass>(json);
            highestScore = data.highestScore;
            bestPlayer = data.BestPlayer;
            playerInfo = data.playerInfo;
            isLoaded = true;
        }       
    }
    public void SortPlayerScore()
    {
        playerInfo = playerInfo.OrderByDescending(p => p.playerScore).ToList();
    }
}
      