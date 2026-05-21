using UnityEngine;
using System;

public class AttentionLevel : MonoBehaviour
{
    public static AttentionLevel Instance;
    public float attentionLevel;
    public bool counting = false;

    public PlayerController Player;

    public float attentionModifier = 1;

    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        Player.AttentionBringing += AddAttention;
    }

    public void StartChecking()
    {
        counting = true;
    }

    public void AddAttention(float numToAdd)
    {
        if (!counting)
            return;

        attentionLevel += numToAdd * attentionModifier;
    }

    public void ResetAttention()
    {
        counting = false;
        attentionLevel = 0;
    }
}
