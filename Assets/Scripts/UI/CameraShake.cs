using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    private CinemachineBasicMultiChannelPerlin noise;
    private Coroutine shakeRoutine;

    void Awake()
    {
        noise = GetComponent<CinemachineBasicMultiChannelPerlin>();

        if (noise != null)
            noise.AmplitudeGain = 0f;
    }

    public void Shake(float intensity = 2f, float duration = 0.4f)
    {
        if (noise == null)
            return;

        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(ShakeRoutine(intensity, duration));
    }

    IEnumerator ShakeRoutine(float intensity, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            noise.AmplitudeGain = Mathf.Lerp(
                intensity, 0f, elapsed / duration
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

        noise.AmplitudeGain = 0f;
        shakeRoutine = null;
    }

    public void StartContinuousShake(float intensity = 1f)
{
    if (noise == null)
        return;

    if (shakeRoutine != null)
    {
        StopCoroutine(shakeRoutine);
        shakeRoutine = null;
    }

    noise.AmplitudeGain = intensity;
}

public void StopShake()
{
    if (noise == null)
        return;

    if (shakeRoutine != null)
    {
        StopCoroutine(shakeRoutine);
        shakeRoutine = null;
    }

    noise.AmplitudeGain = 0f;
}
}
