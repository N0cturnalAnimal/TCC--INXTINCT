using UnityEngine;

public class TractorController : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 5f;

    [Header("Estado")]
    public bool chasing = false;

    private void Update()
    {
        if (!chasing)
            return;

        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    public void StartChasing()
    {
        chasing = true;
    }

    public void StopChasing()
    {
        chasing = false;
    }
}
