using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField] GameObject leftWall;
    [SerializeField] GameObject rightWall;
    [SerializeField] GameObject frontWall;
    [SerializeField] GameObject backWall;
    public bool visited {  get; private set; }

    public void Visit()
    {
        visited = true;
    }
    public void ClearLeftWall()
    {
        Destroy(leftWall);
    }
    public void ClearRightWall()
    {
        Destroy(rightWall);
    }
    public void ClearFrontWall()
    {
        Destroy(frontWall);
    }
    public void ClearBackWall()
    {
        Destroy(backWall);
    }
}
