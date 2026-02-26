using UnityEngine;

public class Checkpointsetter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Checkpoint[] Checkpoint = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);
        for (int i = 0; i < Checkpoint.Length; ++i)
        {
            Checkpoint[i].CheckpointReached = true;
        }
    }
}
