using UnityEngine;

[CreateAssetMenu(fileName = "Notification", menuName = "Scriptable Objects/Notification")]
public class NotificationSO : ScriptableObject
{
    [SerializeField, TextArea] private string _message; // texto
    [SerializeField] private Sprite _icon; // <-- NOVO: espaço para a imagem
    [SerializeField] private float _displayDuration = 2f; 
    [SerializeField] private float _fadeDuration = 1f; 

    public string Message => _message;
    public Sprite Icon => _icon; // <-- NOVO
    public float DisplayDuration => _displayDuration;
    public float FadeDuration => _fadeDuration;
}