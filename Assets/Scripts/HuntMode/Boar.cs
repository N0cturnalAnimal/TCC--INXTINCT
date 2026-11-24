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
    public AudioSource idleAudio;
    public AudioSource runningAudio;
    public bool isTemplate = false;

    void Start()
    {
        originalScale = transform.localScale;
        OriginalPos = transform.position;
        following = false;
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (isTemplate)
        return;
        
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
        if (following)
{
    if (idleAudio != null && idleAudio.isPlaying)
        idleAudio.Stop();

    if (runningAudio != null && !runningAudio.isPlaying)
        runningAudio.Play();
}
else
{
    if (runningAudio != null && runningAudio.isPlaying)
        runningAudio.Stop();

    if (idleAudio != null && !idleAudio.isPlaying)
        idleAudio.Play();
}
    }

    void Flip()
{
    if (player == null)
        return;

    float direction = player.position.x - transform.position.x;

    if (direction > 0.01f)
        transform.localScale = new Vector3(
            Mathf.Abs(originalScale.x),
            originalScale.y,
            originalScale.z
        );

    else if (direction < -0.01f)
        transform.localScale = new Vector3(
            -Mathf.Abs(originalScale.x),
            originalScale.y,
            originalScale.z
        );
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
