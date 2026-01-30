using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    [Header("Cinemachine Virtual Camera")]
    [SerializeField] private CinemachineBasicMultiChannelPerlin noise;

    private void Awake()
    {
        Instance = this;
    }
       
    public void Shake(float intensity, float frequency, float duration)
    {
        if (noise == null) return;
        StopAllCoroutines();
        StartCoroutine(ShakeRoutine(intensity, frequency, duration));
    }

    private IEnumerator ShakeRoutine(float intensity, float frequency, float duration)
    {
        noise.AmplitudeGain = intensity;
        noise.FrequencyGain = frequency;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;
       
    }
}