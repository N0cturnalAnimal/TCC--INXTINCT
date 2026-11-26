
using UnityEngine;

public class FallingTree : MonoBehaviour
{
    public Transform tractor;
    private Quaternion OriginalRot;
    public Vector3 NextRot;
    public float distanceToMove;
    public float distanceToReturn;
    public float speed = 2;

    [Header("Som da árvore")]
    public AudioSource fallingSound;

    private bool falling;
    private Quaternion FallRot;

    void Start()
    {
        falling = false;
        OriginalRot = transform.rotation;

        FallRot = Quaternion.Euler(
            OriginalRot.eulerAngles + new Vector3(
                NextRot.x,
                -NextRot.y,
                -NextRot.z
            )
        );
    }

    void Update()
    {
        if (tractor == null)
            return;

        if (Vector3.Distance(transform.position, tractor.position) < distanceToMove
            && !falling)
        {
            falling = true;

            if (fallingSound != null)
                fallingSound.Play();
        }

        if (falling)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                FallRot,
                speed * Time.deltaTime
            );
        }

        if (Vector3.Distance(transform.position, tractor.position) > distanceToReturn
            && Quaternion.Angle(transform.rotation, OriginalRot) > 0.1f)
        {
            falling = false;

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                OriginalRot,
                speed * Time.deltaTime
            );
        }
    }
}
