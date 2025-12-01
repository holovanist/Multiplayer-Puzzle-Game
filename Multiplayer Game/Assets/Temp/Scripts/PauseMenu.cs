using NewMovment;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : NetworkBehaviour
{
    public GameObject Cam;
    InputAction PauseButton;
    // Start is called before the first frame update

    void Start()
    {
        if(!IsOwner)
            GetComponent<Canvas>().enabled = false;
        if(IsOwner)
        { 
            GetComponent<Canvas>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            PauseButton ??= InputSystem.actions.FindAction("Pause");
            if (PauseButton.WasPressedThisFrame() && Time.timeScale == 1)
            {
                Pause();
            }
            else if (PauseButton.WasPressedThisFrame() && Time.timeScale == 0)
            {
                Resume();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    public void Pause()
    {
        if (IsOwner)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            GetComponent<Canvas>().enabled = true;
            Cam.GetComponent<PlayerCam>().enabled = false;
        }
        Time.timeScale = 0;
    }

    public void Resume()
    {
        if (IsOwner)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            GetComponent<Canvas>().enabled = false;
            Cam.GetComponent<PlayerCam>().enabled = true;
        }
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}