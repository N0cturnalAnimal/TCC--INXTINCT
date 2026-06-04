using UnityEngine;

public class Tractor : MonoBehaviour
{
    public Transform respawnPoint;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = respawnPoint.position;
            transform.position = originalPosition;
            gameObject.SetActive(false);
        }
    }
}
