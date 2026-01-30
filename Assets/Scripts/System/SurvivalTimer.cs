using UnityEngine;
using System.Collections.Generic;

public class SurvivalTimer : MonoBehaviour
{
    [SerializeField] private float elapsedTime;
    private float nextMilestone = 300f;
  
    private HashSet<int> helicopterTriggered = new HashSet<int>();

    public float ElapsedTime => elapsedTime;

    public void SetElapsedTime(float time, List<int> triggered)
    {
        elapsedTime = time;
        nextMilestone = ((int)(elapsedTime / 300f) + 1) * 300f;      
        helicopterTriggered = new HashSet<int>(triggered);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        GameEventManager.Instance.InvokeSurvivalTimeChanged(elapsedTime);
       
        if (elapsedTime >= nextMilestone)
        {
            GameEventManager.Instance.InvokeSurvivalMilestone(nextMilestone);
            nextMilestone += 300f;
        }
       
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        if ((minutes == 15 || minutes == 20 || minutes == 30 || minutes == 35)
            && !helicopterTriggered.Contains(minutes))
        {
            helicopterTriggered.Add(minutes);
            GameEventManager.Instance.InvokeHelicopterTakeOff();
        }
    }

    private void OnEnable()
    {
        GameEventManager.Instance.OnPlayerDied += HandlePlayerDied;
    }

    private void OnDisable()
    {
        GameEventManager.Instance.OnPlayerDied -= HandlePlayerDied;
    }

    private void HandlePlayerDied()
    {
        SurvivalLeaderboard.Instance.AddRecord(elapsedTime);
    }

  
    public List<int> GetTriggeredHelicopters()
    {
        return new List<int>(helicopterTriggered);
    }
}