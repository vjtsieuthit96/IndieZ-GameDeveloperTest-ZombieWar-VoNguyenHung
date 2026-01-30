using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject effectPanel;
    [SerializeField] private GameObject popupPanel;    
    [SerializeField] private GameObject popupPrefab;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("PlayerHP"))
        {
            PopupSpawn(popupPrefab,
                "In the ashes of humanity, only the resilient endure…\nFight to survive!");
        }
    }


    private void OnEnable()
    {
        GameEventManager.Instance.OnPlayerDied += ShowGameOver;
        GameEventManager.Instance.OnSurvivalMilestone += HandleMilestone;
        GameEventManager.Instance.OnHelicopterTakeOff += HandleHelicopter;
    }

    private void OnDisable()
    {
        GameEventManager.Instance.OnPlayerDied -= ShowGameOver;
        GameEventManager.Instance.OnSurvivalMilestone -= HandleMilestone;
        GameEventManager.Instance.OnHelicopterTakeOff -= HandleHelicopter;
    }


    private void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        if (effectPanel != null)
            effectPanel.SetActive(false);
    }

    public void OnRetryButton()
    {        
        SceneManager.LoadScene(0);
    }   
    public void OnExitButton()
    {
        Application.Quit();
    }
    public void OnReloadClick()
    {
        GameEventManager.Instance.TriggerReloadClicked();
    }
    public void OnGrenadeClick()
    {
        GameEventManager.Instance.TriggerGrenadeClicked();
    }

    public void PopupSpawn(GameObject popupSpawn, string message)
    {
        GameObject popup = ObjectPoolManager.SpawnObject(
            popupSpawn,
            popupPanel.transform,
            Quaternion.identity
        );

        popup.transform.localPosition = Vector3.zero;
        popup.transform.localScale = Vector3.one;        
        PopupUI popupUI = popup.GetComponent<PopupUI>();
        if (popupUI != null)
        {
            popupUI.SetMessage(message);
        }
    }
    #region PopUp Messages
    private bool tutorialShown = false;
   
    private List<string> encouragementMessages = new List<string>()
    {
        "Keep pushing forward!",
        "You're stronger than you think!",
        "Survival is in your blood!",
        "Every second counts — stay sharp!",
        "The resilient always endure!"
    };
    private float nextEncouragementTime = 0f;
    private void Update()
    {
        if (tutorialShown) 
        {
            if (Time.time >= nextEncouragementTime)
            {
                int randIndex = Random.Range(0, encouragementMessages.Count);
                PopupSpawn(popupPrefab, encouragementMessages[randIndex]);               
                nextEncouragementTime = Time.time + Random.Range(480f, 780f);
            }
        }
    }

    private void HandleMilestone(float milestone)
    {
        if (!tutorialShown)
        {
            PopupSpawn(popupPrefab,
                "Every 5 minutes the game auto‑saves." +
                "\nBut beware — if you die, all saves are wiped and you must start from the beginning." +
                "\nReinforcements from the army will arrive after 10 minutes…");
            tutorialShown = true;            
            nextEncouragementTime = Time.time + Random.Range(480f, 780f);
        }
    }

    private void HandleHelicopter()
    {
        PopupSpawn(popupPrefab, "Helicopter reinforcements incoming!"+ "\nThey will arrive in 30 seconds — prepare yourself!");
    }

    #endregion
}