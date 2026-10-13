using UnityEngine;

public class ChaseEndTrigger : MonoBehaviour
{
    public ChaseManager chaseManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        chaseManager.EndChase();
    }
}