using UnityEngine;

public class NotificationTrigger : MonoBehaviour
{
    [SerializeField] private NotificationSO notificationData;
    private bool hasTriggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (NotificationManager.Instance != null)
            {
                NotificationManager.Instance.ShowNotification(notificationData);
            }
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (hasTriggered) return;

        if (collision.CompareTag("Player"))
        {
            hasTriggered = true;
            NotificationManager.Instance.ShowNotification(notificationData);
        }
    }
}
