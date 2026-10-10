using UnityEngine;
using System.Collections;
public class PlayerRespawn : MonoBehaviour
{
    Rigidbody2D rb;
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
        deathAudio.Play();
        yield return StartCoroutine(
            FadeManager.Instance.FadeOut()
        );

        transform.position =
            RespawnManager.Instance.currentCheckpoint.position;

        yield return new WaitForSeconds(0.2f);

        yield return StartCoroutine(
            FadeManager.Instance.FadeIn()
        );
        life = oLife;
    }
}
