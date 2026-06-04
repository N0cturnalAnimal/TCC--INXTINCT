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
    public float speed = 2;

    void Start()
    {
        OriginalPos = transform.position;
        NextPos = OriginalPos + NewPositionOffset;
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) > distanceToReturn && transform.position != OriginalPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, OriginalPos,speed * Time.deltaTime);
            sprite.flipX = false;
        }

        if (Vector3.Distance(transform.position, player.position) < distanceToMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, NextPos,speed * Time.deltaTime * 2);
            sprite.flipX = true;
        }

        if (transform.position == NextPos)
            sprite.enabled = false;
        else
            sprite.enabled = true;
    }
}
