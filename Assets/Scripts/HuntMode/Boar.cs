using UnityEngine;

public class Boar : MonoBehaviour
{
    public Transform player;
    public float distanceToMove;
    public float distanceToReturn;
    public bool following;
    public float speed = 2;
    private Vector3 OriginalPos;

    public int damage;
    private Animator animator;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
        OriginalPos = transform.position;
        following = false;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) >= distanceToReturn)
        {
            transform.position = Vector3.MoveTowards(transform.position, OriginalPos, speed * Time.deltaTime);
            following = false;
        }

        if (Vector3.Distance(transform.position, player.position) < distanceToMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            following = true;
        }
        Flip();

        animator.SetBool("isRunning", following);
    }

    void Flip()
{
    float direction = player.position.x - transform.position.x;

    if (Mathf.Abs(direction) > 0.01f)
    {
        Vector3 scale = originalScale;
        scale.x = originalScale.x * -Mathf.Sign(direction);
        transform.localScale = scale;
    }
}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRespawn p = other.gameObject.GetComponent<PlayerRespawn>();
            p.Damage(damage);
        }
    }
}
