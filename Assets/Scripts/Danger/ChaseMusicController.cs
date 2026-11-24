
using UnityEngine;

public class ChaseMusicController : MonoBehaviour
{
    public AudioSource normalMusic;
    public AudioSource chaseMusic;
    public AudioSource heartbeatAudio;

    private void Start()
    {
        PlayNormalMusic();
    }

    private void OnEnable()
    {
        PlayerRespawn.OnPlayerRespawned += StopChase;
    }

    private void OnDisable()
    {
        PlayerRespawn.OnPlayerRespawned -= StopChase;
    }

    public void StartChase()
    {
        if (normalMusic != null)
            normalMusic.Stop();

        if (chaseMusic != null && !chaseMusic.isPlaying)
            chaseMusic.Play();

        if (heartbeatAudio != null && !heartbeatAudio.isPlaying)
            heartbeatAudio.Play();
    }

    public void StopChase()
    {
        if (chaseMusic != null)
            chaseMusic.Stop();

        if (heartbeatAudio != null)
            heartbeatAudio.Stop();

        PlayNormalMusic();
    }

    private void PlayNormalMusic()
    {
        if (normalMusic != null && !normalMusic.isPlaying)
            normalMusic.Play();
    }
}
