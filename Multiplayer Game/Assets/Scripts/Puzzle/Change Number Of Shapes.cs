using UnityEngine;

public class ChangeNumberOfShapes : MonoBehaviour
{
    [SerializeField]
    bool IncreasesNumberOfShapes;
    [SerializeField]
    ShapePuzzle shape;
    [SerializeField]
    int ShapeID;
    private void OnCollisionEnter(Collision collision)
    {
        shape.ScrollToNextObjectRPC(IncreasesNumberOfShapes, ShapeID);
    }
    private void OnTriggerEnter(Collider other)
    {
        shape.ScrollToNextObjectRPC(IncreasesNumberOfShapes, ShapeID);
    }
}
