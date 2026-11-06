
using UnityEngine;

public class Opossum : MonoBehaviour
{
    public Transform player;
    public Vector3 NewPositionOffset;

    private Vector3 OriginalPos;
    private Vector3 NextPos;

    public float distanceToMove;
    public float distanceToReturn;

    private SpriteRenderer sprite;
    private Animator animator;

    public float speed = 2;
    public bool moveOnce;

    private bool moving;
    private bool flying;
    public float minIdleSpeed = 0.8f;
    public float maxIdleSpeed = 1.2f;

    void Start()
    {
        OriginalPos = transform.position;
        NextPos = OriginalPos + NewPositionOffset;

        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        animator.SetBool("isFlying", false);
        animator.speed = Random.Range(minIdleSpeed, maxIdleSpeed);
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // Retorna à posição original quando o jogador se afasta
        if (distance > distanceToReturn &&
            transform.position != OriginalPos &&
            !moveOnce)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                OriginalPos,
                speed * Time.deltaTime
            );

            sprite.flipX = false;

            SetFlying(false);
        }

        // Inicia a fuga quando o jogador se aproxima
        if (distance < distanceToMove || moving)
        {
            if (moveOnce)
                moving = true;

            transform.position = Vector3.MoveTowards(
                transform.position,
                NextPos,
                speed * Time.deltaTime * 2
            );

            sprite.flipX = true;

            SetFlying(true);
        }

        // Verifica se chegou ao destino
        if (Vector3.Distance(transform.position, NextPos) < 0.01f)
        {
            SetFlying(false);
            sprite.enabled = false;
        }
        else
        {
            sprite.enabled = true;
        }
    }

    void SetFlying(bool value)
    {
        if (flying == value)
            return;

        flying = value;
        animator.SetBool("isFlying", flying);
    }
}
