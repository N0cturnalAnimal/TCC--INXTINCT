
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour
{
    [Header("Configuração")]
    public bool startEnabled = false;
    public ChaseMusicController musicController;

    [Header("Encerramento")]
    public bool playEndingVideo = false;
    public VideoPlayer endingVideo;
    public GameObject videoScreen;
    public string creditsSceneName = "Creditos";

    private bool canFinish = false;
    private bool finished = false;

    private PlayerController playerController;
    private Rigidbody2D playerRB;
    private RigidbodyConstraints2D originalConstraints;
    private bool originalControllerEnabled;

    private void Start()
    {
        canFinish = startEnabled;

        if (videoScreen != null)
            videoScreen.SetActive(false);
    }

    private void OnEnable()
    {
        if (endingVideo != null)
            endingVideo.loopPointReached += OnVideoFinished;
    }

    private void OnDisable()
    {
        if (endingVideo != null)
            endingVideo.loopPointReached -= OnVideoFinished;
    }

    public void EnableFinish()
    {
        canFinish = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canFinish || finished)
            return;

        if (!collision.CompareTag("Player"))
            return;

        finished = true;

        if (musicController != null)
            musicController.StopChase();

        if (playEndingVideo)
        {
            StartEnding(collision);
        }
        else
        {
            SceneController.instance.NextLevel();
        }
    }

    private void StartEnding(Collider2D collision)
    {
        playerController = collision.GetComponentInParent<PlayerController>();
        playerRB = collision.GetComponentInParent<Rigidbody2D>();

        if (playerController != null)
        {
            originalControllerEnabled = playerController.enabled;
            playerController.enabled = false;
        }

        if (playerRB != null)
        {
            originalConstraints = playerRB.constraints;
            playerRB.linearVelocity = Vector2.zero;
            playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        if (endingVideo == null)
        {
            Debug.LogError("Vídeo de encerramento não foi atribuído!", this);
            return;
        }

        if (videoScreen != null)
            videoScreen.SetActive(true);

        endingVideo.Play();
    }

    private void OnVideoFinished(VideoPlayer videoPlayer)
    {
        SceneManager.LoadScene(creditsSceneName);
    }
}
