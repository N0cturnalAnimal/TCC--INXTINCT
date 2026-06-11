using UnityEngine;
using System.Collections;
public class PlayerRespawn : MonoBehaviour
{
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Respawn()
    {
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
    yield return StartCoroutine(
        FadeManager.Instance.FadeOut()
    );

    transform.position =
        RespawnManager.Instance.currentCheckpoint.position;

    yield return new WaitForSeconds(0.2f);

    yield return StartCoroutine(
        FadeManager.Instance.FadeIn()
    );
}
}
