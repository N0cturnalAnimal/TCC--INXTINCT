using UnityEngine;
using UnityEngine.UI;

public class OpossumArmadillo : MonoBehaviour
{
    public float attentintionLimit = 5000;

    private bool failed = false;
    private bool completed = false;

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

    public float speed = 2;
    public Vector3 NewPositionOffset;
    private Vector3 NextPos;

    void Start()
    {
        failed = false;
        completed = false;
        NextPos = ArmadilloObj.transform.position + NewPositionOffset;
    }

    void Update()
    {
        if (failed)
        {
            ArmadilloObj.transform.position = Vector3.MoveTowards(
                ArmadilloObj.transform.position,
                NextPos,
                speed * Time.deltaTime
            );

            if (Vector3.Distance(ArmadilloObj.transform.position, NextPos) < 0.01f)
            {
                armadillo.enabled = false;
            }
        }

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
            ArmadilloObj.tag  = "Untagged";
            armadillo.flipX = true;
            barItself.SetActive(false);
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
        if (completed || failed)
            return;

        if (other.CompareTag("Player"))
        {
            AttentionLevel.Instance.ResetAttention();
            failed = false;
            ArmadilloObj.tag = "Carriable";
            barItself.SetActive(false);
        }
    }
}
