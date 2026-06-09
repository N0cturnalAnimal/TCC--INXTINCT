using UnityEngine;

[CreateAssetMenu(fileName = "Notification", menuName = "Scriptable Objects/Notification")]
public class NotificationSO : ScriptableObject
{
    [SerializeField, TextArea] private string _message; //texto
    [SerializeField] private float _displayDuration = 2f; //quanto tempo vai ficar na tela
    [SerializeField] private float _fadeDuration = 1f; //duração de fade in/out

    public string Message => _message;
    public float DisplayDuration => _displayDuration;
    public float FadeDuration => _fadeDuration;

}
