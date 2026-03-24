using UnityEngine;

public class SpaceWhaleRespawnBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Checkpoint[] Checkpoint = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);
        for (int i = 0; i < Checkpoint.Length; ++i)
        {
            Checkpoint[i].Reset = true;
        }
        TellWhaleItCommitedMurder();
    }
    void TellWhaleItCommitedMurder()
    {
        SpaceWhaleTargetMannager.ExitAttackState();
    }
}
