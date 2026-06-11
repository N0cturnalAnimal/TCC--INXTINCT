using UnityEngine;

public class AguaMorte : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entrou na água!");
        if(other.CompareTag("Player"))
        {
            PlayerRespawn player =
                other.GetComponent<PlayerRespawn>();
            player.Respawn();
        }
    }
    
}
