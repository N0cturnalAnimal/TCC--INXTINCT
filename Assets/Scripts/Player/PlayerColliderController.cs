
using UnityEngine;

public class PlayerColliderController : MonoBehaviour
{
    [Header("Referências")]
    public CapsuleCollider2D playerCollider;
    public Animator animator;

    [Header("Collider em pé")]
    public Vector2 standingSize = new Vector2(6.8f, 5.5f);
    public Vector2 standingOffset = new Vector2(-0.47f, -0.65f);

    [Header("Collider agachado")]
    public Vector2 crouchingSize = new Vector2(6.8f, 3.5f);
    public Vector2 crouchingOffset = new Vector2(-0.47f, -1.6f);

    void Update()
    {
        bool isCrouching = animator.GetBool("isCrouching");

        if (isCrouching)
        {
            playerCollider.size = crouchingSize;
            playerCollider.offset = crouchingOffset;
        }
        else
        {
            playerCollider.size = standingSize;
            playerCollider.offset = standingOffset;
        }
    }
}
