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

            shape.ScrollToNextObject(IncreasesNumberOfShapes, ShapeID);
    }
    private void OnTriggerEnter(Collider other)
    {
        shape.ScrollToNextObject(IncreasesNumberOfShapes, ShapeID);
    }
}
