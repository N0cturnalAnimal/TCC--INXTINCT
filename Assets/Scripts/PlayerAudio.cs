using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource footstepAudio;
    public AudioSource carryAudio;

    private PlayerController player;

    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    void Update()
    {
        bool isWalking =
            player != null &&
            player.canJump &&
            player.state != PlayerEnum.Climbing &&
            player.state != PlayerEnum.Carrying &&
            Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.01f;

        if (footstepAudio == null)
            return;

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

    public void PlayCarrySound()
    {
        if (carryAudio != null)
            carryAudio.Play();
    }
}
