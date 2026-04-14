using UnityEngine;

public class ShowTipOnTrigger : MonoBehaviour
{
    public string tip;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UITips.Instance.ShowTip(tip);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UITips.Instance.ShowTip();
        }
    }
}
