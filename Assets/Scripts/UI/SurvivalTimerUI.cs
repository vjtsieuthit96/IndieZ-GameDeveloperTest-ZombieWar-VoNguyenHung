using TMPro;
using UnityEngine;
using System.Collections;

public class SurvivalTimerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text leaderboardText;
    [SerializeField] private GameObject leadboardPanel;
    private bool isVisible = false;

    private void Start()
    {
        RefreshLeaderboard();
    }

    void OnEnable()
    {
        GameEventManager.Instance.OnSurvivalTimeChanged += UpdateTimerUI;
        GameEventManager.Instance.OnSurvivalMilestone += PlayMilestoneAnim;
        GameEventManager.Instance.OnPlayerDied += ShowLeaderboard;
    }

    void OnDisable()
    {
        GameEventManager.Instance.OnSurvivalTimeChanged -= UpdateTimerUI;
        GameEventManager.Instance.OnSurvivalMilestone -= PlayMilestoneAnim;
        GameEventManager.Instance.OnPlayerDied -= ShowLeaderboard;
    }
    public void TogglePanel()
    {
        RefreshLeaderboard();
        isVisible = !isVisible;
        leadboardPanel.SetActive(isVisible);      
    }

    private void UpdateTimerUI(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void PlayMilestoneAnim(float milestone)
    {
        StartCoroutine(ScaleAnim());
    }

    private IEnumerator ScaleAnim()
    {
        Vector3 original = timerText.transform.localScale;
        Vector3 target = original * 2.5f;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 4;
            timerText.transform.localScale = Vector3.Lerp(original, target, t);
            yield return null;
        }

        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 4;
            timerText.transform.localScale = Vector3.Lerp(target, original, t);
            yield return null;
        }
    }
    private void ShowLeaderboard()
    {
        var topRecords = SurvivalLeaderboard.Instance.GetTopRecords();
        leaderboardText.text = "";

        int count = Mathf.Min(topRecords.Count, 5);
        for (int i = 0; i < count; i++)
        {
            float time = topRecords[i];
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            leaderboardText.text += $"{i + 1}. {minutes:00}:{seconds:00}\n";
        }

        if (!isVisible)
        {
            isVisible = true;
            leadboardPanel.SetActive(true);
        }
    }
    public void RefreshLeaderboard()
    {
        var topRecords = SurvivalLeaderboard.Instance.GetTopRecords();
        leaderboardText.text = "";

        int count = Mathf.Min(topRecords.Count, 5); 
        for (int i = 0; i < count; i++)
        {
            float time = topRecords[i];
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            leaderboardText.text += $"{i + 1}. {minutes:00}:{seconds:00}\n";
        }
    }

}