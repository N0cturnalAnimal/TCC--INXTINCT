using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static bool checkpointsLocked = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (checkpointsLocked)
            return;

        if (other.CompareTag("Player"))
        {
            RespawnManager.Instance.SetCheckpoint(transform);
        }
    }
}