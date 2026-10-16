using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoControl : MonoBehaviour
{
    [Header("Overall control")]
    public bool playingVideo;
    public float waitTime = 0;
    private float videoStart;

    [Header("Cutscenes audios and clips")]
    public VideoClip[] cutscenesVideos;
    public AudioClip[] cutsceneAudioList;
    public VideoPlayer cutscenesPlayer;
    public AudioSource cutsceneAudio;

    private AudioClip lastOverride;
    public SceneController sceneControl;

    [Header("OverrideDestination")]
    public bool overRide;
    public string sceneDestination;

    public Action VideoEnd;

    void Update()
    {
        if (!playingVideo)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
            EndVideo();

        if (Time.time > videoStart + cutscenesPlayer.clip.length)
        {
            EndVideo();
        }
    }

    public void PlayVideo(int videoNum = 0)
    {
        playingVideo = true;
        cutscenesPlayer.clip = cutscenesVideos[videoNum];
        videoStart = Time.time;
        cutscenesPlayer.Play();
        if (cutsceneAudioList.Length != 0)
            StartCoroutine(playAudioDelay(videoNum));
    }

    public void EndVideo()
    {
        VideoEnd?.Invoke();
        playingVideo = false;
        if (cutsceneAudio != null)
        {
            cutsceneAudio.Stop();
            cutsceneAudio.clip = lastOverride;

            if (lastOverride != null)
                cutsceneAudio.Play();
        }

        sceneControl.NextLevel();
    }

    public IEnumerator playAudioDelay(int videoNum)
    {
        lastOverride = cutsceneAudio.clip;
        yield return new WaitForSeconds(waitTime);

        if (cutsceneAudio != null)
        {
            cutsceneAudio.clip = cutsceneAudioList[videoNum];
            cutsceneAudio.Play();
        }
    }

    public void StartPlayVideoDelay(float wait) => StartCoroutine(playVideoDelay(wait));

    public IEnumerator playVideoDelay(float wait)
    {
        yield return new WaitForSeconds(wait);
        PlayVideo();
    }

}
