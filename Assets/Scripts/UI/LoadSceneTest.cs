using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneTest : MonoBehaviour
{    
    public void LoadNextScene()
    {        
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
      
        int nextIndex = currentIndex + 1;
      
        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextIndex = 0;
        }
      
        SceneManager.LoadScene(nextIndex);
    }
}