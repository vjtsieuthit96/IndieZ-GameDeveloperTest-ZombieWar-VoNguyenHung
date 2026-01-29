using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private int poolSize = 15;
    private AudioSource[] sources;
    private int index = 0;

    void Awake()
    {        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            InitPool();
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void InitPool()
    {
        sources = new AudioSource[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = new GameObject("AudioSource_" + i);
            go.transform.parent = transform;
            AudioSource src = go.AddComponent<AudioSource>();
            src.spatialBlend = 1f; 
            sources[i] = src;
        }
    }

    public void PlaySound(AudioClip clip, Vector3 position)
    {
        if (clip == null) return;
        AudioSource src = sources[index];
        src.transform.position = position;
        src.PlayOneShot(clip);
        index = (index + 1) % sources.Length;
    }

    public void PlayRandomSound(AudioClip[] clips, Vector3 position)
    {
        if (clips == null || clips.Length == 0) return;
        int rand = Random.Range(0, clips.Length);
        PlaySound(clips[rand], position);
    }
}