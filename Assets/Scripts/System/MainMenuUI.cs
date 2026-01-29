using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuUI : MonoBehaviour
{    
    public void OnPlayButton()
    {        
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }   
    public void OnExitButton()
    {   
        Application.Quit();
    }
}