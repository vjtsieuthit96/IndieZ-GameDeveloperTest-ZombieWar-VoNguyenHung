using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject loadingIcon;

    public void OnPlayButton()
    {
        if (loadingIcon != null) loadingIcon.SetActive(true);
       
        if (PlayerPrefs.HasKey("SceneIndex"))
        {
            int savedSceneIndex = PlayerPrefs.GetInt("SceneIndex");
            StartCoroutine(LoadSceneAsync(savedSceneIndex));
        }
        else
        {            
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            StartCoroutine(LoadSceneAsync(nextSceneIndex));
        }
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                if (loadingIcon != null) loadingIcon.SetActive(false);
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}