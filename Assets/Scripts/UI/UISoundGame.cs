using UnityEngine;
using UnityEngine.UI;

public class UISoundGame : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    void Awake()
    {
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f);

        musicSlider.SetValueWithoutNotify(musicVol);
        sfxSlider.SetValueWithoutNotify(sfxVol);
    }

    public void SetSFXVolume(float volume)
    {
        AudioManager.Instance.SetSFXVolume(volume);        
    }
    public void SetMusicVolume(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
       
    }


}
