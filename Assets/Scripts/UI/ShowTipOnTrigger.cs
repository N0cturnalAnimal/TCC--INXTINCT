using UnityEngine;

public class ShowTipOnTrigger : MonoBehaviour
{
    [Header("Configuração do Tip")]
    [Tooltip("Arraste o arquivo NotificationSO (o cubo azul) aqui")]
    public NotificationSO notificationData;

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. TESTE: Algo entrou no gatilho? (Mostra o nome do objeto)
        Debug.Log("Algo entrou no gatilho: " + other.name);

        if (other.CompareTag("Player") && notificationData != null)
        {
            // 2. TESTE: É o player e o Asset está preenchido!
            Debug.Log("É o Player! Tentando mostrar o Tip: " + notificationData.name);
            
            // Chama o NotificationManager para mostrar
            if (NotificationManager.Instance != null)
            {
                // 3. TESTE: O Manager existe e foi chamado!
                Debug.Log("NotificationManager encontrado. Mandando exibir.");
                NotificationManager.Instance.ShowNotification(notificationData);
            }
            else
            {
                // ERRO: O Manager não está na cena ou foi destruído
                Debug.LogError("ERRO: NotificationManager.Instance está NULL! O Manager está na cena?");
            }
        }
        else
        {
            // ERRO: A Tag está errada ou você esqueceu de arrastar o Asset
            if (!other.CompareTag("Player")) Debug.Log("Mas não é o Player (Tag errada).");
            if (notificationData == null) Debug.LogError("ERRO: O campo 'Notification Data' está vazio no Inspector!");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player saiu do gatilho.");
            
            // ADICIONAL: Força o Tip a sumir na hora que o jogador sai do gatilho
            if (NotificationManager.Instance != null)
            {
                NotificationManager.Instance.ForceHideNotification();
            }
        }
    }
}