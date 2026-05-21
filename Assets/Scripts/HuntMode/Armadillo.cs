using UnityEngine;
using UnityEngine.UI;

public class Armadillo : MonoBehaviour
{
    public float attentintionLimit = 5000;

    private bool failed = false;
    private bool completed = false;

    public Sprite failedS;
    public Sprite working;
    public SpriteRenderer armadillo;
    private Vector3 originalPos;

    public GameObject ArmadilloObj;

    public float minDelay;
    public float maxDelay;
    public float minDuration;
    public float maxDuration;
    public bool looking;
    private float timer;

    public float newAttentionModifier;

    public Image bar;
    public GameObject barItself;

    void Start()
    {
        failed = false;
        completed = false;
    }

    void Update()
    {
        if (!failed && !completed && Time.time > timer)
            GetAttention();

        if (AttentionLevel.Instance.Player.state == PlayerEnum.Carrying && !failed && !completed)
        {
            completed = true;
            barItself.SetActive(false);
        }

        if (failed || completed)
            return;

        bar.fillAmount = AttentionLevel.Instance.attentionLevel / attentintionLimit;

        if (AttentionLevel.Instance.attentionLevel > attentintionLimit)
        {
            failed = true;
            armadillo.sprite = failedS;
            ArmadilloObj.tag  = "Untagged";
        }
    }

    void GetAttention()
    {
        if (looking)
        {
            timer = Time.time + Random.Range(minDuration, maxDuration);
            looking = false;
            armadillo.flipX = false;
            AttentionLevel.Instance.attentionModifier = 1;
        }
        else
        {
            timer = Time.time + Random.Range(minDelay, maxDelay);
            looking = true;
            armadillo.flipX = true;
            AttentionLevel.Instance.attentionModifier = newAttentionModifier;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (completed)
            return;

        if (other.CompareTag("Player"))
        {
            AttentionLevel.Instance.StartChecking();
            barItself.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (completed)
            return;

        if (other.CompareTag("Player"))
        {
            AttentionLevel.Instance.ResetAttention();
            failed = false;
            armadillo.sprite = working;
            ArmadilloObj.tag = "Carriable";
            barItself.SetActive(false);
        }
    }
}
