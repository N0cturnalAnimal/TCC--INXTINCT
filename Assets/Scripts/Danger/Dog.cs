
using UnityEngine;

public class Dog : MonoBehaviour
{
    [Header("Movimentação")]
    [Tooltip("-1 = esquerda | 1 = direita")]
    public float movementDirection = 1f;

    public float movementLimit;
    public float maxSpeed = 10f;
    public float chaseSpeed = 10f;

    [Header("Referências")]
    public AudioSource deathAudio;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movementDirection = Mathf.Sign(movementDirection);
    }

    void FixedUpdate()
    {
        if (rb == null)
            return;

        rb.linearVelocity = new Vector2(
            movementDirection * chaseSpeed,
            rb.linearVelocity.y
        );

        if (Mathf.Abs(rb.linearVelocity.x) > maxSpeed)
        {
            rb.linearVelocity = new Vector2(
                movementDirection * maxSpeed,
                rb.linearVelocity.y
            );
        }
    }

    void Update()
    {
        if (rb == null)
            return;

        if (movementDirection > 0 && transform.position.x >= movementLimit)
        {
            transform.position = new Vector3(
                movementLimit,
                transform.position.y,
                transform.position.z
            );

            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        else if (movementDirection < 0 && transform.position.x <= movementLimit)
        {
            transform.position = new Vector3(
                movementLimit,
                transform.position.y,
                transform.position.z
            );

            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerRespawn playerRespawn =
            other.GetComponent<PlayerRespawn>();

        if (playerRespawn == null)
            playerRespawn = other.GetComponentInParent<PlayerRespawn>();

        if (playerRespawn != null)
        {
            playerRespawn.Respawn();
        }
        else
        {
            Debug.LogError("PlayerRespawn não encontrado no Player!", other);
        }
    }
}
