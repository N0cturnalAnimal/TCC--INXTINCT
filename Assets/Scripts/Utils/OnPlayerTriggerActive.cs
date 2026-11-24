
using UnityEngine;
using UnityEngine.Playables;

public class OnPlayerTriggerActive : MonoBehaviour
{
    [Header("Objetos")]
    public GameObject Active;
    public GameObject CutsceneTractor;

    [Header("Referências")]
    public string tagToCompare;
    public FinishPoint finishPoint;
    public ChaseMusicController musicController;
    public PlayableDirector cutscene;

    private bool activated = false;
    private bool isPlayingCutscene = false;

    private PlayerController playerController;
    private Rigidbody2D playerRB;

    private RigidbodyConstraints2D originalConstraints;
    private bool originalControllerEnabled;
    private Transform cutsceneOffset;
    private Vector3 originalOffsetPosition;
    

    void OnEnable()
    {
        PlayerRespawn.OnPlayerRespawned += ResetSequence;

        if (cutscene != null)
            cutscene.stopped += OnCutsceneFinished;
    }

    void Start()
    {
        if (Active != null)
            Active.SetActive(false);

        if (CutsceneTractor != null)
            CutsceneTractor.SetActive(false);
    }

    void OnDisable()
    {
        PlayerRespawn.OnPlayerRespawned -= ResetSequence;

        if (cutscene != null)
            cutscene.stopped -= OnCutsceneFinished;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (activated || !other.CompareTag(tagToCompare))
            return;

        playerController = other.GetComponentInParent<PlayerController>();
        cutsceneOffset = playerController.transform.Find("CutsceneOffset");

if (cutsceneOffset != null)
{
    originalOffsetPosition = cutsceneOffset.localPosition;
}
        playerRB = other.GetComponentInParent<Rigidbody2D>();

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

        activated = true;
        isPlayingCutscene = true;

        if (CutsceneTractor != null)
            CutsceneTractor.SetActive(true);

        if (cutscene != null)
            cutscene.Play();
        else
            StartChase();
    }

    void OnCutsceneFinished(PlayableDirector director)
    {
        if (!isPlayingCutscene)
            return;

        isPlayingCutscene = false;

        if (CutsceneTractor != null)
            CutsceneTractor.SetActive(false);
        if (cutsceneOffset != null)
{
    cutsceneOffset.localPosition = originalOffsetPosition;
}
        StartChase();
    }

    void StartChase()
    {
        if (Active != null)
            Active.SetActive(true);

        if (finishPoint != null)
            finishPoint.EnableFinish();

        if (musicController != null)
            musicController.StartChase();

        UnlockPlayer();
    }

    void UnlockPlayer()
    {
        if (playerRB != null)
        {
            playerRB.constraints = originalConstraints;
            playerRB.linearVelocity = Vector2.zero;
        }

        if (playerController != null)
            playerController.enabled = originalControllerEnabled;
    }

    void ResetSequence()
    {
        if (musicController != null)
        {
            musicController.StopChase();
        }
        
        isPlayingCutscene = false;
        
        if (cutscene != null && cutscene.state == PlayState.Playing)
            cutscene.Stop();

        if (CutsceneTractor != null)
            CutsceneTractor.SetActive(false);

        if (Active != null)
            Active.SetActive(false);

        UnlockPlayer();

        activated = false;
    }
}
