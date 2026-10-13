using UnityEngine;

public class TractorDestroyable : MonoBehaviour
{
    public enum DestroyType
    {
        Destroy,
        Fall,
        Break
    }

    [Header("Tipo de reação")]
    public DestroyType destroyType;

    [Header("Objetos que serão desativados")]
    public Collider2D[] collidersToDisable;

    [Header("Objetos visuais")]
    public GameObject objectToDisable;

    public void DestroyByTractor()
    {
        switch (destroyType)
        {
            case DestroyType.Destroy:
                DestroyObject();
                break;

            case DestroyType.Fall:
                FallObject();
                break;

            case DestroyType.Break:
                BreakObject();
                break;
        }
    }

    void DestroyObject()
    {
        DisableColliders();

        if (objectToDisable != null)
            objectToDisable.SetActive(false);
        else
            gameObject.SetActive(false);
    }

    void FallObject()
    {
        DisableColliders();

        // Por enquanto apenas desativa.
        // Depois colocaremos a animação de queda aqui.
        if (objectToDisable != null)
            objectToDisable.SetActive(false);
        else
            gameObject.SetActive(false);
    }

    void BreakObject()
    {
        DisableColliders();

        if (objectToDisable != null)
            objectToDisable.SetActive(false);
        else
            gameObject.SetActive(false);
    }

    void DisableColliders()
    {
        foreach (Collider2D col in collidersToDisable)
        {
            if (col != null)
                col.enabled = false;
        }
    }
}
