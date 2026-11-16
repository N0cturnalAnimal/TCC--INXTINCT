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
            
            // ====================================================
            // CORREÇÃO: Esconde a UI antiga antes de colocar a nova
            // ====================================================
            if (notificationUICanvasGroup != null)
                notificationUICanvasGroup.alpha = 0;

            // Configura o Texto
            if (notificationTextUI != null)
                notificationTextUI.text = data.Message;
            
            // Lógica segura para a Imagem com Debug
            if (notificationImageUI != null)
            {
                if (data.Icon != null)
                {
                    notificationImageUI.sprite = data.Icon;
                    notificationImageUI.gameObject.SetActive(true);
                    Debug.Log("SUCESSO: A imagem " + data.Icon.name + " foi colocada no ícone!"); 
                }
                else
                {
                    notificationImageUI.gameObject.SetActive(false); 
                    Debug.Log("AVISO: O NotificationSO '" + data.name + "' não tem nenhum ícone salvo no campo 'Icon'!"); 
                }
            }
            else
            {
                Debug.LogError("ERRO GRAVE: O campo 'Notification Image UI' está vazio no Inspector do NotificationManager!"); 
            }

            // Executa o Fade In
            yield return StartCoroutine(FadeCanvasGroup(notificationUICanvasGroup, true, data.FadeDuration));
            
            // Espera o tempo na tela
            yield return new WaitForSecondsRealtime(data.DisplayDuration); 
            
            // Executa o Fade Out
            yield return StartCoroutine(FadeCanvasGroup(notificationUICanvasGroup, false, data.FadeDuration));
        }
        isDisplayingNotification = false;
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, bool fadeIn, float duration)
    {
        if (canvasGroup == null) yield break; 

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

    public void ForceHideNotification()
    {
        StopAllCoroutines(); 
        notificationQueue.Clear(); 
        isDisplayingNotification = false;

        if (notificationUICanvasGroup != null)
            notificationUICanvasGroup.alpha = 0; 
    }
}