using UnityEngine;

public class ResetCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Checkpoint[] Checkpoint = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);
        foreach (Checkpoint checkpoint in Checkpoint)
        {
            checkpoint.Reset = true;
        }
    }
}
