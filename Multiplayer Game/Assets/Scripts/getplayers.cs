using UnityEngine;

public class getplayers : MonoBehaviour
{
    public Canvas Ui;
    private void OnTriggerEnter(Collider other)
    {
        Ui.gameObject.SetActive(true);
        GetComponentInParent<SpaceWhaleTargetMannager>().GetPlayers();
    }
}
