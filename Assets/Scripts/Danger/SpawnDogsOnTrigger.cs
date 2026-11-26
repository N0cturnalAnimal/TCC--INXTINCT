
using UnityEngine;
using UnityEngine.Playables;
using System.Collections.Generic;

public class SpawnDogsOnTrigger : MonoBehaviour
{
    [Header("Configuração")]
    public GameObject dogPrefab;
    public Transform[] spawnPoints;
    public string tagToCompare = "Player";

    [Header("Cães da Cutscene")]
    public GameObject[] cutsceneDogs;

    [Header("Referências")]
    public FinishPoint finishPoint;
    public ChaseMusicController musicController;
    public PlayableDirector cutscene;
    public Transform visual;

    [Header("Câmera da Cutscene")]
    public GameObject cutsceneCamera;

    private bool activated = false;
    private bool isPlayingCutscene = false;

    private List<GameObject> spawnedDogs = new List<GameObject>();

    private PlayerController playerController;
    private Rigidbody2D playerRB;

    private RigidbodyConstraints2D originalConstraints;
    private bool originalControllerEnabled;

    private Vector3 originalVisualLocalPosition;

    private void Start()
    {
        if (visual != null)
            originalVisualLocalPosition = visual.localPosition;
        else
            Debug.LogError("Visual da onça não foi atribuído!", this);
    }

    private void OnEnable()
    {
        PlayerRespawn.OnPlayerRespawned += ResetPack;

        if (cutscene != null)
            cutscene.stopped += OnCutsceneFinished;
    }

    private void OnDisable()
    {
        PlayerRespawn.OnPlayerRespawned -= ResetPack;

        if (cutscene != null)
            cutscene.stopped -= OnCutsceneFinished;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated || !other.CompareTag(tagToCompare))
            return;

        if (dogPrefab == null)
        {
            Debug.LogError("Dog Prefab não foi atribuído!", this);
            return;
        }

        activated = true;
        isPlayingCutscene = true;

        playerController = other.GetComponentInParent<PlayerController>();
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

        if (cutscene != null)
            cutscene.Play();
        else
            OnCutsceneFinished(null);
    }

    private void OnCutsceneFinished(PlayableDirector director)
    {
        if (!isPlayingCutscene)
            return;

        isPlayingCutscene = false;

        if (cutsceneCamera != null)
            cutsceneCamera.SetActive(false);

        foreach (GameObject dog in cutsceneDogs)
        {
            if (dog != null)
                dog.SetActive(false);
        }

        if (visual != null)
            visual.localPosition = originalVisualLocalPosition;

        UnlockPlayer();
        SpawnPack();
    }

    private void SpawnPack()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint == null)
            {
                Debug.LogError("Existe um Spawn Point vazio!", this);
                continue;
            }

            GameObject dog = Instantiate(
                dogPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );

            Boar boar = dog.GetComponent<Boar>();

            if (boar != null)
            {
                boar.isTemplate = false;
                boar.player = playerController != null
                    ? playerController.transform
                    : null;
            }

            spawnedDogs.Add(dog);
        }

        if (finishPoint != null)
            finishPoint.EnableFinish();

        if (musicController != null)
            musicController.StartChase();

        Debug.Log("Matilha criada: " + spawnedDogs.Count + " cachorros.");
    }

    private void UnlockPlayer()
    {
        if (playerRB != null)
        {
            playerRB.constraints = originalConstraints;
            playerRB.linearVelocity = Vector2.zero;
        }

        if (playerController != null)
            playerController.enabled = originalControllerEnabled;
    }

    private void ResetPack()
    {
        isPlayingCutscene = false;

        if (cutscene != null && cutscene.state == PlayState.Playing)
            cutscene.Stop();

        if (cutsceneCamera != null)
            cutsceneCamera.SetActive(false);

        if (visual != null)
            visual.localPosition = originalVisualLocalPosition;

        UnlockPlayer();

        foreach (GameObject dog in spawnedDogs)
        {
            if (dog != null)
                Destroy(dog);
        }

        spawnedDogs.Clear();
        activated = false;

        if (musicController != null)
            musicController.StopChase();

        Debug.Log("Matilha reiniciada. DogTrigger liberado novamente.");
    }
}
