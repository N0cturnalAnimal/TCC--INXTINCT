using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider slider;
    [SerializeField] private string parameterName = "MasterVolume";

    private void Start()
    {
        // Carrega o valor salvo (padrão 0.75)
        float saved = PlayerPrefs.GetFloat(parameterName, 0.75f);

        slider.SetValueWithoutNotify(saved);
        ApplyVolume(saved);

        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    private void OnSliderChanged(float value)
    {
        ApplyVolume(value);
        PlayerPrefs.SetFloat(parameterName, value);
    }

    private void ApplyVolume(float value)
    {
        // Converte linear (0 a 1) para decibéis (-80 dB a 0 dB)
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        mixer.SetFloat(parameterName, dB);
    }
}