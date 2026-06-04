using UnityEngine;

public class Tractor : MonoBehaviour
{
    public Transform respawnPoint;
    private Vector3 originalPosition;

    public float movementLimit;
    public float maxSpeed = 10f;

    void Start()
    {
        originalPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    void Update()
    {
        if (transform.position.x > movementLimit)
        {
            transform.position = new Vector3(movementLimit, transform.position.y, transform.position.z);
        }
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
