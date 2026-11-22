
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource footstepAudio;
    public AudioSource climbAudio;

    private PlayerController player;
    private Rigidbody2D rb;

    void Start()
    {
        player = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        bool isWalking =
            player.state != PlayerEnum.Climbing &&
            player.state != PlayerEnum.Carrying &&
            player.canJump &&
            Mathf.Abs(rb.linearVelocity.x) > 0.1f;

        if (isWalking)
        {
            if (!footstepAudio.isPlaying)
                footstepAudio.Play();
        }
        else
        {
            if (footstepAudio.isPlaying)
                footstepAudio.Stop();
        }
    }

    public void PlayClimbSound()
    {
        if (climbAudio != null)
            climbAudio.Play();
    }
}
