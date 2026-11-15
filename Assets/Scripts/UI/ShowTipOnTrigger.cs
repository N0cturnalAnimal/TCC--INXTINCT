using UnityEngine;

public class ShowTipOnTrigger : MonoBehaviour
{
    [Header("Configuração do Tip")]
    [Tooltip("Arraste o arquivo NotificationSO (o cubo azul) aqui")]
    public NotificationSO notificationData;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && notificationData != null)
        {
            // Chama o NotificationManager para mostrar
            if (NotificationManager.Instance != null)
            {
                NotificationManager.Instance.ShowNotification(notificationData);
            }
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