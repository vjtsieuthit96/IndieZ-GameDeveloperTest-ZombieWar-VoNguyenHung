using TMPro;
using UnityEngine;
using System.Collections;

public class SurvivalTimerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;

    void OnEnable()
    {
        GameEventManager.Instance.OnSurvivalTimeChanged += UpdateTimerUI;
        GameEventManager.Instance.OnSurvivalMilestone += PlayMilestoneAnim;
    }

    void OnDisable()
    {
        GameEventManager.Instance.OnSurvivalTimeChanged -= UpdateTimerUI;
        GameEventManager.Instance.OnSurvivalMilestone -= PlayMilestoneAnim;
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
}