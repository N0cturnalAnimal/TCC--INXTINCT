using UnityEngine;
using System.Collections;

public class AguaMorte : MonoBehaviour
{
    private bool isRespawning = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isRespawning)
            return;

        if (other.CompareTag("Player"))
        {
            StartCoroutine(Morrer(other));
        }
    }

    IEnumerator Morrer(Collider2D other)
    {
        isRespawning = true;

        PlayerRespawn playerRespawn = other.GetComponent<PlayerRespawn>();
        PlayerController playerController = other.GetComponent<PlayerController>();
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

        // Impede movimento durante a morte
        if (playerController != null)
            playerController.enabled = false;

        // Para o movimento atual
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        // Fade para preto
        yield return StartCoroutine(
            FadeManager.Instance.FadeOut()
        );

        // Pequena pausa dramática
        yield return new WaitForSeconds(0.3f);

        // Respawn
        playerRespawn.Respawn();

        // Pequena pausa antes de voltar
        yield return new WaitForSeconds(0.1f);

        // Fade de volta
        yield return StartCoroutine(
            FadeManager.Instance.FadeIn()
        );

        // Reativa o controle
        if (playerController != null)
            playerController.enabled = true;

        isRespawning = false;
    }
}
