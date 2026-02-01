using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

    [Header("Music Settings")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip[] musicClips;

    private Coroutine musicCoroutine;
    private bool isRunning = false;
    private int lastIndex = -1;

    [Header("SFX Pool Settings")]
    [SerializeField] private int poolSize = 15;
    private AudioSource[] sfxSources;
    [SerializeField] private float sfxMaxDistance = 20f;
    private int sfxIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitSFXPool();
            
            float musicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
            float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f);          
            SetMusicVolume(musicVol);
            SetSFXVolume(sfxVol);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (!isRunning)
        {
            musicCoroutine = StartCoroutine(MusicRoutine());
            isRunning = true;
        }
    }

    // ---------------- MUSIC ----------------
    private IEnumerator MusicRoutine()
    {
        PlayRandomMusic();
        yield return new WaitForSeconds(musicSource.clip.length);

        while (true)
        {
            float waitBeforePlay = Random.Range(60f, 180f);
            yield return new WaitForSeconds(waitBeforePlay);

            PlayRandomMusic();
            yield return new WaitForSeconds(musicSource.clip.length);

            float waitAfterPlay = Random.Range(60f, 180f);
            yield return new WaitForSeconds(waitAfterPlay);
        }
    }

    private void PlayRandomMusic()
    {
        if (musicClips.Length == 0) return;

        int index;
        do
        {
            index = Random.Range(0, musicClips.Length);
        } while (index == lastIndex && musicClips.Length > 1);

        lastIndex = index;
        musicSource.clip = musicClips[index];
        musicSource.Play();
    }

    // ---------------- SFX ----------------
    private void InitSFXPool()
    {
        sfxSources = new AudioSource[poolSize];        
        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = new GameObject("SFXSource_" + i);
            go.transform.parent = transform;
            AudioSource src = go.AddComponent<AudioSource>();
            src.spatialBlend = 1f;
            src.minDistance = 1f;
            src.maxDistance = sfxMaxDistance;
            src.outputAudioMixerGroup = sfxMixerGroup;
            sfxSources[i] = src;
        }
    }

    public void PlaySound(AudioClip clip, Vector3 position)
    {
        if (clip == null) return;
        float distance = Vector3.Distance(Camera.main.transform.position, position);
        if (distance > sfxMaxDistance) return;
        AudioSource src = sfxSources[sfxIndex];
        src.transform.position = position;
        src.PlayOneShot(clip);
        sfxIndex = (sfxIndex + 1) % sfxSources.Length;
    }

    public void PlayRandomSound(AudioClip[] clips, Vector3 position)
    {
        if (clips == null || clips.Length == 0) return;
        int rand = Random.Range(0, clips.Length);
        PlaySound(clips[rand], position);
    }

    // ---------------- VOLUME CONTROL ----------------
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}