using UnityEngine;

public class FallingTree : MonoBehaviour
{
    public Transform tractor;
    private Quaternion OriginalRot;
    public Vector3 NextRot;
    public float distanceToMove;
    public float distanceToReturn;
    public float speed = 2;
    private bool falling;

    void Start()
    {
        falling = false;
        OriginalRot = transform.rotation;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, tractor.position) < distanceToMove)
            falling = true;

        if (falling)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(NextRot), speed * Time.deltaTime);

        if (Vector3.Distance(transform.position,tractor.position) > distanceToReturn && transform.rotation != OriginalRot)
        {
            falling = false;
            transform.rotation = Quaternion.Slerp(transform.rotation, OriginalRot, speed * Time.deltaTime);
        }
    }
}
