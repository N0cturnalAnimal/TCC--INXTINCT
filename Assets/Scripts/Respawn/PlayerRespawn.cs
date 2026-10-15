
using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isRespawning = false;

    public int life;
    private int oLife;
    public AudioSource deathAudio;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        oLife = life;
    }

    public void Respawn()
    {
        if (isRespawning)
            return;

        StartCoroutine(RespawnRoutine());
    }

    public void Damage(int d)
    {
        life -= d;

        if (life <= 0)
            Respawn();
    }

    IEnumerator RespawnRoutine()
    {
        isRespawning = true;

        if (deathAudio != null)
            deathAudio.Play();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (RespawnManager.Instance == null ||
            RespawnManager.Instance.currentCheckpoint == null)
        {
            Debug.LogError("Nenhum checkpoint definido no RespawnManager!", this);
            isRespawning = false;
            yield break;
        }

        yield return StartCoroutine(FadeManager.Instance.FadeOut());

        transform.position =
            RespawnManager.Instance.currentCheckpoint.position;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        yield return new WaitForSeconds(0.2f);

        yield return StartCoroutine(FadeManager.Instance.FadeIn());

        life = oLife;
        isRespawning = false;
    }
}
