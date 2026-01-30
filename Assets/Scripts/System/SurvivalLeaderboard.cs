using System.Collections.Generic;
using UnityEngine;

public class SurvivalLeaderboard : MonoBehaviour
{
    public static SurvivalLeaderboard Instance;

    private List<float> records = new List<float>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadRecords();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddRecord(float time)
    {
        records.Add(time);
        records.Sort((a, b) => b.CompareTo(a));
        if (records.Count > 10) records.RemoveAt(10); 
        SaveRecords();
    }

    public List<float> GetTopRecords() => records;

    public void SaveRecords()
    {        
        PlayerPrefs.SetInt("LeaderboardCount", records.Count);
        
        for (int i = 0; i < records.Count; i++)
        {
            PlayerPrefs.SetFloat("Leaderboard_" + i, records[i]);
        }

        PlayerPrefs.Save();
    }

    public void LoadRecords()
    {
        records.Clear();
        int count = PlayerPrefs.GetInt("LeaderboardCount", 0);

        for (int i = 0; i < count; i++)
        {
            float time = PlayerPrefs.GetFloat("Leaderboard_" + i, 0f);
            if (time > 0f) records.Add(time);
        }
    }

    public void ResetRecords()
    {
        PlayerPrefs.DeleteKey("LeaderboardCount");
        for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.DeleteKey("Leaderboard_" + i);
        }
        records.Clear();
    }
}