using UnityEngine;

public class ChaseTrigger1 : MonoBehaviour
{
    public ChaseManager chaseManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        chaseManager.StartChase();
    }
}