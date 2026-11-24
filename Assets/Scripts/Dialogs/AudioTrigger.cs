
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    public AudioSource audioSource;
    private bool alreadyPlayed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (alreadyPlayed || !other.CompareTag("Player"))
            return;

        audioSource.Play();
        alreadyPlayed = true;
    }
}
