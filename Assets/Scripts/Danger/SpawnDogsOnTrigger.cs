
using UnityEngine;
using System.Collections.Generic;

public class SpawnDogsOnTrigger : MonoBehaviour
{
    [Header("Configuração")]
    public GameObject dogPrefab;
    public Transform[] spawnPoints;
    public string tagToCompare = "Player";

    [Header("Referências")]
    public FinishPoint finishPoint;

    private bool activated = false;
    private List<GameObject> spawnedDogs = new List<GameObject>();

    private void OnEnable()
    {
        PlayerRespawn.OnPlayerRespawned += ResetPack;
    }

    private void OnDisable()
    {
        PlayerRespawn.OnPlayerRespawned -= ResetPack;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (activated || !other.CompareTag(tagToCompare))
            return;

        if (dogPrefab == null)
        {
            Debug.LogError("Dog Prefab não foi atribuído!", this);
            return;
        }

        activated = true;

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

            spawnedDogs.Add(dog);
        }

        if (finishPoint != null)
            finishPoint.EnableFinish();

        Debug.Log("Matilha criada: " + spawnedDogs.Count + " cachorros.");
    }

    void ResetPack()
    {
        foreach (GameObject dog in spawnedDogs)
        {
            if (dog != null)
                Destroy(dog);
        }

        spawnedDogs.Clear();
        activated = false;

        Debug.Log("Matilha reiniciada. DogTrigger liberado novamente.");
    }
}
