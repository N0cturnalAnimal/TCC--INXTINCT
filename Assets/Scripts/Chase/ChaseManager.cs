using UnityEngine;

public class ChaseManager : MonoBehaviour
{
    [Header("Trator")]
    public TractorController tractor;

    [Header("Player")]
    public PlayerController player;

    [Header("Estado")]
    public bool chaseActive = false;

    public void StartChase()
    {
        if (chaseActive)
            return;

        chaseActive = true;

        tractor.StartChasing();
    }

    public void EndChase()
    {
        if (!chaseActive)
            return;

        chaseActive = false;

        tractor.StopChasing();
    }
}