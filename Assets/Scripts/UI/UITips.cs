using UnityEngine;
using TMPro;

public class UITips : MonoBehaviour
{
    public static UITips Instance;
    public TMP_Text tipText;

    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        ShowTip();
    }

    public void ShowTip(string tip = "")
    {
        tipText.text = tip;
    }
}
