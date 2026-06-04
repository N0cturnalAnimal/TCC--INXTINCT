using UnityEngine;

public class OnPlayerTriggerActive : MonoBehaviour
{
    public GameObject Active;
    public string tagToCompare;

    void Start()
    {
        Active.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tagToCompare))
        {
            Active.SetActive(true);
        }
    }
}
