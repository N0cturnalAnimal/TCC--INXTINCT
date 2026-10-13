
using UnityEngine;

public class TractorChaseTrigger : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private GameObject tractor;

    private bool chaseStarted = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (chaseStarted)
            return;

        if (!other.CompareTag("Player"))
            return;

        chaseStarted = true;

        if (tractor != null)
        {
            tractor.SetActive(true);
        }
        else
        {
            Debug.LogError("O trator não foi atribuído ao TractorChaseTrigger!", this);
        }
    }
}

