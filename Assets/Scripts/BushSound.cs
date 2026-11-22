
using UnityEngine;

public class BushSound : MonoBehaviour
{
    public AudioSource bushAudio;

    private bool playerInside;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag("Player") && !playerInside)
        {
            playerInside = true;

            if (bushAudio != null)
                bushAudio.Play();
        }
    }

    void OnTriggerExit2D(Collider2D other)
{
    if (other.transform.root.CompareTag("Player"))
    {
        playerInside = false;

        if (bushAudio != null)
            bushAudio.Stop();
    }
}
}
