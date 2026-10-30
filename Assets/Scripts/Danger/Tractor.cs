using UnityEngine;

public class Tractor : MonoBehaviour
{
    public Transform respawnPoint;
    private Vector3 originalPosition;

    public float movementLimit;
    public float maxSpeed = 10f;
    private Rigidbody2D rb;
    public float chaseSpeed = 10f;

    public AudioSource deathAudio;

    void Start()
    {
        originalPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
{
    if (rb == null)
        return;

    rb.linearVelocity = new Vector2(-chaseSpeed, rb.linearVelocity.y);

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
            deathAudio.Play();
        }
    }

    
public void ResetForPlayerRespawn()
{
    // Restaura a posição inicial do trator
    transform.position = originalPosition;

    // Para o movimento físico
    if (rb != null)
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    // Desativa o trator até o trigger ativá-lo novamente
    gameObject.SetActive(false);
}

}
