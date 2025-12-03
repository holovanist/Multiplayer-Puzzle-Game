using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Main;
    private void Awake()
    {
        Main = this;
    }
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void UnlockCursor()
    {
        Cursor.lockState=CursorLockMode.None;
        Cursor.visible = true;
    }
}