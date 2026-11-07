
using UnityEngine;
using UnityEngine.UI;

public class StaminaWheel : MonoBehaviour
{
    [Header("Stamina")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float drainRate = 30f;
    [SerializeField] private float recoveryRate = 30f;
    [SerializeField] private float recoveryThreshold = 25f;

    [Header("UI")]
    [SerializeField] private Image greenWheel;
    [SerializeField] private Image redWheel;
    [SerializeField] private GameObject staminaUI;
    private bool isVisible;

    private float stamina;
    private bool staminaExhausted;

    public bool CanRun => !staminaExhausted;
    public float CurrentStamina => stamina;

    void Start()
    {
        stamina = maxStamina;
        UpdateWheel();
    }

    void SetVisibility(bool visible)
{
    if (isVisible == visible)
        return;

    isVisible = visible;
    staminaUI.SetActive(visible);
}
    public void UpdateStamina(bool isRunning)
{
    if (isRunning && !staminaExhausted)
    {
        stamina -= drainRate * Time.deltaTime;

        if (stamina <= 0f)
        {
            stamina = 0f;
            staminaExhausted = true;
        }
    }
    else
    {
        stamina += recoveryRate * Time.deltaTime;
        stamina = Mathf.Min(stamina, maxStamina);

        if (stamina >= maxStamina * (recoveryThreshold / 100f))
        {
            staminaExhausted = false;
        }
    }

    UpdateWheel();

    // Mostra enquanto corre ou enquanto recupera stamina
    bool shouldBeVisible = isRunning || stamina < maxStamina;

    SetVisibility(shouldBeVisible);
}

    void UpdateWheel()
    {
        float fill = stamina / maxStamina;

        greenWheel.fillAmount = fill;
        redWheel.fillAmount = fill;

        greenWheel.enabled = !staminaExhausted;
    }
}
