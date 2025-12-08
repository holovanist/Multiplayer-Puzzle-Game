using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace NewMovment
{
    public class PlayerCam : NetworkBehaviour
    {
        public float sensX;
        public float sensY;
        public float MaxXRotationUp = 90f;
        public float MaxXRotationDown = -90f;

        public Transform orientatiion;

        PlayerInputHandler PlayerInputHandler;

        float xRotation;
        float yRotation;
        private void Start()
        {
            PlayerInputHandler = GetComponent<PlayerInputHandler>();
            PlayerInputHandler.LockCursor();
        }
        private void Update()
        {
            if (!IsOwner) return;
            float mouseX = PlayerInputHandler.RotationInput.x * sensX * Time.deltaTime;
            float mouseY = -PlayerInputHandler.RotationInput.y * sensY * Time.deltaTime;

            yRotation += mouseX;

            xRotation += mouseY;
            xRotation = Mathf.Clamp(xRotation, MaxXRotationDown, MaxXRotationUp);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientatiion.rotation = Quaternion.Euler(0, yRotation, 0);

            if(PlayerInputHandler.PauseTriggered)
            {
                PlayerInputHandler.UnlockCursor();
            }    
            if(PlayerInputHandler.LeftClickTriggered)
            {
                PlayerInputHandler.LockCursor();
            }
        }
        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            PlayerInputHandler.LockCursor();

        }
    }
}