
using UnityEngine;

public class OnPlayerTriggerActive : MonoBehaviour
{
    public GameObject Active;
    public string tagToCompare;
    public FinishPoint finishPoint;

    private bool activated = false;

    void Start()
    {
        Active.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tagToCompare))
        {
            Active.SetActive(true);
            activated = true;

            if (finishPoint != null)
            {
                finishPoint.EnableFinish();
            }
        }
    }
}
