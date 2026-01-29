using UnityEngine;

public class SurvivalTimer : MonoBehaviour
{
    private float elapsedTime;
    private float nextMilestone = 300f;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        GameEventManager.Instance.InvokeSurvivalTimeChanged(elapsedTime);
        
        if (elapsedTime >= nextMilestone)
        {
            GameEventManager.Instance.InvokeSurvivalMilestone(nextMilestone);      
            nextMilestone += 300f;
        }
    }
    public float ElapsedTime => elapsedTime;

    public void SetElapsedTime(float time)
    {
        elapsedTime = time;
        nextMilestone = ((int)(elapsedTime / 300f) + 1) * 300f;
    }
}