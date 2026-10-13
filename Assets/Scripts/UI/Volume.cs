using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsLoader : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string[] parameters = { "MusicVolume", "SFXVolume" };

    private void Start()
    {
        foreach (string p in parameters)
        {
            float value = PlayerPrefs.GetFloat(p, 0.75f);
            float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
            mixer.SetFloat(p, dB);
        }
    }
}