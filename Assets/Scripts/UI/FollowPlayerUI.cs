using UnityEngine;

public class FollowPlayerUI : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 1.5f, 0);

    void LateUpdate()
    {
        transform.position = player.position + offset;
    }
}
