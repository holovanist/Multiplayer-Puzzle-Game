using Unity.Netcode;
using UnityEngine;

public class MovementTest3 : NetworkBehaviour
{
    [Header("Player Control Parameters")]
    [SerializeField] float walkSpeed = 3.0f;
    [SerializeField] float sprintMultiplier = 2.0f;
    [SerializeField] float jumpForce = 5.0f;
    [SerializeField] float gravityMultiplier = 1.0f;
    [SerializeField] float mouseSensitivity = 0.1f;
    [SerializeField] float upDownLookRange = 80f;

    [Header("References")]
    [SerializeField] CharacterController characterController;
    [SerializeField] Camera MainCamera;

    PlayerInputHandler PlayerInputHandler;
    Vector3 currentMovement;
    float verticalRotation;
    float CurrentSpeed => walkSpeed * (PlayerInputHandler.SprintTriggered ? sprintMultiplier : 1);
    void Start()
    {
        if (!IsOwner)
        {
            MainCamera.GetComponent<Camera>().enabled = false;
            MainCamera.gameObject.GetComponent<AudioListener>().enabled = false;
        }
        PlayerInputHandler = GetComponent<PlayerInputHandler>();
        PlayerInputHandler.LockCursor();
    }
    void Update()
    {
        HandleMovement();
        HandleRotation();
    }
    Vector3 CalculateworldDirection()
    {
        Vector3 inputDirection = new Vector3(PlayerInputHandler.MovementInput.x, 0f, PlayerInputHandler.MovementInput.y);
        Vector3 worldirection = transform.TransformDirection(inputDirection);
        return worldirection.normalized;
    }
    void HandleJumping()
    {
        if (characterController.isGrounded)
        {
            currentMovement.y = -0.5f;
            if (PlayerInputHandler.JumpTriggered)
            {
                currentMovement.y = jumpForce;
            }
        }
        else
        {
            currentMovement.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }
    }
    void HandleMovement()
    {
        Vector3 worldDirection = CalculateworldDirection();
        currentMovement.x = worldDirection.x * CurrentSpeed;
        currentMovement.z = worldDirection.z * CurrentSpeed;

        HandleJumping();
        characterController.Move(currentMovement * Time.deltaTime);
    }
    void HandleRotation()
    {
        float mouseXRotation = PlayerInputHandler.RotationInput.x * mouseSensitivity;
        float mouseYRotation = PlayerInputHandler.RotationInput.y * mouseSensitivity;

        //applies horizontal rotation
        transform.Rotate(0, mouseXRotation, 0);

        //applies vertical rotation to camera
        verticalRotation = Mathf.Clamp(verticalRotation - mouseYRotation, -upDownLookRange, upDownLookRange);
        MainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
}
