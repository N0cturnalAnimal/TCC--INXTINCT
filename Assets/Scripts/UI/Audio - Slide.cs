using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ControleAudio : MonoBehaviour
{
    public static ControleAudio Instance;

    [Header("Audio Mixer")]
    public AudioMixer mixer;

    [Header("Sliders")]
    public Slider sliderMusica;
    public Slider sliderSFX;

    private const string CHAVE_MUSICA = "VolumeMusica";
    private const string CHAVE_SFX = "VolumeSFX";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (mixer == null)
        {
            Debug.LogError("AudioMixer não configurado!");
            return;
        }

        float musica = PlayerPrefs.GetFloat(CHAVE_MUSICA, 1f);
        float sfx = PlayerPrefs.GetFloat(CHAVE_SFX, 1f);

        AtualizarMusica(musica);
        AtualizarSFX(sfx);

        if (sliderMusica != null)
        {
            sliderMusica.value = musica;
            sliderMusica.onValueChanged.AddListener(AtualizarMusica);
        }

        if (sliderSFX != null)
        {
            sliderSFX.value = sfx;
            sliderSFX.onValueChanged.AddListener(AtualizarSFX);
        }
    }

    public void AtualizarMusica(float volume)
    {
        float db = volume <= 0.0001f ? -80f : Mathf.Log10(volume) * 20f;

        if (mixer.SetFloat("VolumeMusica", db))
        {
            PlayerPrefs.SetFloat(CHAVE_MUSICA, volume);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogError("Parâmetro VolumeMusica não encontrado!");
        }
    }

    public void AtualizarSFX(float volume)
    {
        float db = volume <= 0.0001f ? -80f : Mathf.Log10(volume) * 20f;

        if (mixer.SetFloat("VolumeSFX", db))
        {
            PlayerPrefs.SetFloat(CHAVE_SFX, volume);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogError("Parâmetro VolumeSFX não encontrado!");
        }
    }
}