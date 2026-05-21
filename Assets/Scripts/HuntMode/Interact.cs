using UnityEngine;

public class Interact : MonoBehaviour
{
    public GameObject toActive;
    bool canInteract = false;

    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
            toActive.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
        }
    }
}
