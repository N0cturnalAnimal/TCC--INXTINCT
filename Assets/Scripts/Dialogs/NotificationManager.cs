using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;
    
    [Header("Notification UI")]
    [SerializeField] private GameObject notificationParent; 
    [SerializeField] private TMP_Text notificationTextUI; 
    [SerializeField] private Image notificationImageUI; // Arraste o "Icone" aqui

    private CanvasGroup notificationUICanvasGroup;
    private Queue<NotificationSO> notificationQueue = new Queue<NotificationSO>();
    private bool isDisplayingNotification = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (notificationParent != null)
        {
            notificationUICanvasGroup = notificationParent.GetComponent<CanvasGroup>();
            if (notificationUICanvasGroup != null)
                notificationUICanvasGroup.alpha = 0;
        }
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
            
            if (notificationTextUI != null)
                notificationTextUI.text = data.Message;
            
            // Lógica segura para a Imagem
            if (notificationImageUI != null)
            {
                if (data.Icon != null)
                {
                    notificationImageUI.sprite = data.Icon;
                    notificationImageUI.gameObject.SetActive(true);
                }
                else
                {
                    // Se o SO não tiver imagem, esconde o ícone para não ficar quadrado branco
                    notificationImageUI.gameObject.SetActive(false); 
                }
            }

            yield return StartCoroutine(FadeCanvasGroup(notificationUICanvasGroup, true, data.FadeDuration));
            
            yield return new WaitForSecondsRealtime(data.DisplayDuration); 
            
            yield return StartCoroutine(FadeCanvasGroup(notificationUICanvasGroup, false, data.FadeDuration));
        }
        isDisplayingNotification = false;
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, bool fadeIn, float duration)
    {
        if (canvasGroup == null) yield break; // Evita erro se o CanvasGroup não existir

        float targetAlpha = fadeIn ? 1f : 0f;
        float initialAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime; 
            canvasGroup.alpha = Mathf.Lerp(initialAlpha, targetAlpha, elapsedTime / duration);
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
    }

    // ==========================================
    // ADICIONAL: Forçar o fechamento do Tip
    // ==========================================
    public void ForceHideNotification()
    {
        StopAllCoroutines(); // Para qualquer fade que esteja acontecendo
        notificationQueue.Clear(); // Limpa a fila de textos pendentes
        isDisplayingNotification = false;

        if (notificationUICanvasGroup != null)
            notificationUICanvasGroup.alpha = 0; // Esconde na hora
    }
}