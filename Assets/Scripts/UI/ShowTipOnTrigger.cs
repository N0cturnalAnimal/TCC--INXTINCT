using UnityEngine;

public class ShowTipOnTrigger : MonoBehaviour
{
    [Header("Configuração do Tip")]
    [Tooltip("Arraste o arquivo NotificationSO (o cubo azul) aqui")]
    public NotificationSO notificationData;

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. TESTE: Algo entrou no gatilho? (Mostra o nome do objeto)

        if (other.CompareTag("Player") && notificationData != null)
        {
            // 2. TESTE: É o player e o Asset está preenchido!
            
            // Chama o NotificationManager para mostrar
            if (NotificationManager.Instance != null)
            {
                // 3. TESTE: O Manager existe e foi chamado!
                NotificationManager.Instance.ShowNotification(notificationData);
            }
            else
            {
                // ERRO: O Manager não está na cena ou foi destruído
            }
        }
        else
        {
            // ERRO: A Tag está errada ou você esqueceu de arrastar o Asset
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            // ADICIONAL: Força o Tip a sumir na hora que o jogador sai do gatilho
            if (NotificationManager.Instance != null)
            {
                NotificationManager.Instance.ForceHideNotification();
            }
        }
    }
}