
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    public bool startEnabled = false;

    private bool canFinish = false;
    private bool finished = false;

    private void Start()
    {
        canFinish = startEnabled;
    }

    public void EnableFinish()
    {
        canFinish = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canFinish || finished)
            return;

        if (collision.CompareTag("Player"))
        {
            finished = true;
            SceneController.instance.NextLevel();
        }
    }
}
