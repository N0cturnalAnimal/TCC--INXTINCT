using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float delay;

    private float timer;
    public bool destruct = false;

    void Start()
    {
        if (destruct)
            StartSequence();
    }

    private void OnEnable()
    {
        timer = Time.time;
    }

    void Update()
    {
        if (destruct && timer + delay < Time.time)
            gameObject.SetActive(false);
    }

    public void StartSequence()
    {
        timer = Time.time;
        destruct = true;
    }
}
