
using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isRespawning = false;

    public static System.Action OnPlayerRespawned;

    public int life;
    private int oLife;
    public AudioSource deathAudio;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        oLife = life;
    }

    public void Respawn(bool playDeathAudio = true)
    {
        if (isRespawning)
            return;

        StartCoroutine(RespawnRoutine(playDeathAudio));
    }

    public void Damage(int d)
    {
        life -= d;

        if (life <= 0)
            Respawn();
    }

    IEnumerator RespawnRoutine(bool playDeathAudio)
    {
        isRespawning = true;

        if (playDeathAudio && deathAudio != null)
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

        OnPlayerRespawned?.Invoke();
    }
}
