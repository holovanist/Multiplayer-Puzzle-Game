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

        float xRotation;
        float yRotation;
        public StarterAssetsInputs _input;
        InputAction MousePos;

        private void Awake()
        {
            MousePos = InputSystem.actions.FindAction("Look");
            Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
        }
        private void Update()
        {
            if (!IsOwner) return;
            float mouseX = -MousePos.ReadValue<Vector2>().x * sensX;
            float mouseY = MousePos.ReadValue<Vector2>().y * sensY;

            yRotation += mouseX;

            xRotation += mouseY;
            xRotation = Mathf.Clamp(xRotation, MaxXRotationDown, MaxXRotationUp);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientatiion.rotation = Quaternion.Euler(0, yRotation, 0);
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
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false; 

        }
    }
}