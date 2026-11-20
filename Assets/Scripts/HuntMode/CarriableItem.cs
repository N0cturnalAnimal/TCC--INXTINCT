using UnityEngine;

public class CarryableItem : MonoBehaviour
{
    public enum ItemEffect
    {
        RecoverStamina,
        DrainStamina
    }

    [Header("Effect")]
    public ItemEffect effect;
    public float staminaAmount = 25f;
}
