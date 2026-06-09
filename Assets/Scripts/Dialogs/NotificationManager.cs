using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;
    [Header("Notification UI")]
    [SerializeField] private GameObject notificationParent; //parent GameObject com o CanvasGroup
    [SerializeField] private TMP_Text notificationTextUI; //componente do texto
    
    private CanvasGroup notificationUICanvasGroup;
    private Queue<NotificationSO> notificationQueue = new Queue<NotificationSO>();
    private bool isDisplayingNotification = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        notificationUICanvasGroup = notificationParent.GetComponent<CanvasGroup>();
        notificationUICanvasGroup.alpha = 0;
    }

    public void ShowNotification(NotificationSO notificationData)
    {
        notificationQueue.Enqueue(notificationData);
        if (!isDisplayingNotification)
        {
            StartCoroutine(DisplayNotification());
        }
    }

        private IEnumerator DisplayNotification()
    {
        isDisplayingNotification = true;
        while (notificationQueue.Count > 0)
        {
            NotificationSO data = notificationQueue.Dequeue();
            notificationTextUI.text = data.Message;
            yield return StartCoroutine(FadeCanvasGroup(notificationUICanvasGroup, true, data.FadeDuration)); //Fade in
            yield return new WaitForSeconds(data.DisplayDuration); //display
            yield return StartCoroutine(FadeCanvasGroup(notificationUICanvasGroup, false, data.FadeDuration)); //Fade out
        }
        isDisplayingNotification = false;
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, bool fadeIn, float duration)
    {
        float targetAlpha = fadeIn ? 1f : 0f;
        float initialAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(initialAlpha, targetAlpha, elapsedTime / duration);
            yield return null;
        }
        canvasGroup.alpha =  targetAlpha;
    }
}
