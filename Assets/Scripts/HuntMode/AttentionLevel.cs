using UnityEngine;
using System;

public class AttentionLevel : MonoBehaviour
{
    public static AttentionLevel Instance;
    public float attentionLevel;

    public PlayerController Player;

    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        Player.AttentionBringing += AddAttention;
    }

    public void AddAttention(float numToAdd)
    {
        attentionLevel += numToAdd;
    }

    public void ResetAttention()
    {
        attentionLevel = 0;
    }
}
