
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    private bool canFinish = false;
    private bool finished = false;

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
