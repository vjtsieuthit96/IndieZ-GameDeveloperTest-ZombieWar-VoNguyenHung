using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject effectPanel;   

    private void OnEnable()
    {
        GameEventManager.Instance.OnPlayerDied += ShowGameOver;
    }

    private void OnDisable()
    {
        GameEventManager.Instance.OnPlayerDied -= ShowGameOver;
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
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
   
    public void OnExitButton()
    {
        Application.Quit();
    }
}