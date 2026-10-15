using UnityEngine;

public class TreeCollisionTest : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(
            "O tronco colidiu com: " + collision.gameObject.name,
            this
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(
            "O trigger do tronco detectou: " + other.gameObject.name,
            this
        );
    }
}
