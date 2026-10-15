
using UnityEngine;

public class TractorFallingTree : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private GameObject tractor;

    [Header("Queda")]
    [SerializeField] private float fallTorque = 8f;

    private Rigidbody2D rb;
    private bool hasFallen;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError(
                "Tree_Fall_Test precisa de um Rigidbody2D.", this);
            return;
        }

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 1f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasFallen || tractor == null || rb == null)
            return;

        Transform hitTransform = collision.transform;

        bool hitTractor =
            hitTransform == tractor.transform ||
            hitTransform.IsChildOf(tractor.transform) ||
            tractor.transform.IsChildOf(hitTransform);

        if (!hitTractor)
            return;

        hasFallen = true;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;

        float direction =
            tractor.transform.position.x < transform.position.x
            ? -1f
            : 1f;

        rb.AddTorque(direction * fallTorque, ForceMode2D.Impulse);

        Debug.Log("A árvore começou a cair!", this);
    }
}
