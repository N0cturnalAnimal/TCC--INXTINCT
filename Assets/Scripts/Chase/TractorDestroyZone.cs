using UnityEngine;

public class TractorDestroyZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        TractorDestroyable destroyable =
            other.GetComponentInParent<TractorDestroyable>();

        if (destroyable != null)
        {
            destroyable.DestroyByTractor();
        }
    }
}