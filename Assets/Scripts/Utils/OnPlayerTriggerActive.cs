
using UnityEngine;

public class OnPlayerTriggerActive : MonoBehaviour
{
    public GameObject Active;
    public string tagToCompare;
    public FinishPoint finishPoint;
    public ChaseMusicController musicController;

    private bool activated = false;

    void Start()
    {
        Active.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (activated || !other.CompareTag(tagToCompare))
            return;

        Active.SetActive(true);
        activated = true;

        if (finishPoint != null)
        {
            finishPoint.EnableFinish();
        }

        if (musicController != null)
        {
            musicController.StartChase();
        }
    }
}
