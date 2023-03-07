using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text hiScoreText;
    public int hiScoreLoaded;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    private string savePath;
    
    // Start is called before the first frame update
    void Start()
    {
        // Initialize the Current Score
        string playerName = StatsManager.Instance.playerName;
        ScoreText.text = $"{playerName}'s Score : {m_Points}";

        // Initialize the High Score
        InitializeHighScore();

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
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        string playerName = StatsManager.Instance.playerName;
        ScoreText.text = $"{playerName}'s Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        SaveScore(StatsManager.Instance.playerName, m_Points);
    }

    private void InitializeHighScore()
    {
        savePath = Application.persistentDataPath + "/savedata.json";
        Debug.Log($"File Save Path: {savePath}");
        if (File.Exists(savePath))
        {
            Debug.Log("Save file exists.");
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            hiScoreText.text = $"Best Score : {data.highScoreName} : {data.highScore}";
            Debug.Log($"High score found! {data.highScoreName}, {data.highScore}");
            hiScoreLoaded = data.highScore;
        } else
        {
            hiScoreText.text = "No High Score Recorded!";
            Debug.Log("No High Score is available.");
        }
    }

    private void SaveScore(string name, int score)
    {
        if (m_Points > hiScoreLoaded)
        {
            savePath = Application.persistentDataPath + "/savedata.json";
            SaveData saveData = new SaveData();
            saveData.highScoreName = name;
            saveData.highScore = score;

            string json = JsonUtility.ToJson(saveData);
            File.WriteAllText(savePath, json);
            Debug.Log($"File saved: {savePath}");
        } else
        {
            Debug.Log("Current game not eligible for high score.");
        }
    }

    [System.Serializable]
    class SaveData
    {
        public string highScoreName;
        public int highScore;
    }
}
