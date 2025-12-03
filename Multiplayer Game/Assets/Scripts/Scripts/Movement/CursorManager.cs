using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Main;
    private void Awake()
    {
        Main = this;
    }

}