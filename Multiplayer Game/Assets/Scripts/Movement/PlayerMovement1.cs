using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement1 : NetworkBehaviour
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
    [SerializeField] Transform MainCamera;

    PlayerInputHandler PlayerInputHandler;
    Vector3 currentMovement;
    float verticalRotation;
    bool MouseLocked;
    float CurrentSpeed => walkSpeed * (PlayerInputHandler.SprintTriggered ? sprintMultiplier : 1);
    void Start()
    {
        PlayerInputHandler = GetComponent<PlayerInputHandler>();
        PlayerInputHandler.LockCursor();
        MouseLocked = true;
    }
    void Update()
    {
        if (!IsOwner)
        {
            MainCamera.GetComponent<Camera>().enabled = false;
            MainCamera.gameObject.GetComponent<AudioListener>().enabled = false;
        }
        if(IsLocalPlayer && IsServer)
        {
        HandleMovement(PlayerInputHandler.MovementInput);
        HandleRotation(PlayerInputHandler.RotationInput);
        HandleJumping(PlayerInputHandler.JumpTriggered);
        }
        else if (IsLocalPlayer)
        {
            MoveServerRpc(PlayerInputHandler.MovementInput, PlayerInputHandler.RotationInput, PlayerInputHandler.JumpTriggered);
        }

        if (PlayerInputHandler.PauseTriggered && IsOwner)
        {
            PlayerInputHandler.UnlockCursor();
            MouseLocked =false;
        }
        if (!MouseLocked && PlayerInputHandler.LeftClickTriggered && IsOwner)
        {
            PlayerInputHandler.LockCursor();
            MouseLocked =true;
        }
    }
    Vector3 CalculateworldDirection(Vector2 Input)
    {
        Vector3 inputDirection = new(Input.x, 0f, Input.y);
        Vector3 worldirection = transform.TransformDirection(inputDirection);
        return worldirection.normalized;
    }
    void HandleJumping(bool JumpTriggered)
    {
        if (characterController.isGrounded)
        {
            currentMovement.y = -0.5f;
            if (JumpTriggered)
            {
                currentMovement.y = jumpForce;
            }
        }
        else
        {
            currentMovement.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }
    }
    void HandleMovement(Vector2 Input)
    {
        Vector3 worldDirection = CalculateworldDirection(Input);
        currentMovement.x = worldDirection.x * CurrentSpeed;
        currentMovement.z = worldDirection.z * CurrentSpeed;

        characterController.Move(currentMovement * Time.deltaTime);
    }
    void HandleRotation(Vector2 RotationInput)
    {
        float mouseXRotation = RotationInput.x * mouseSensitivity;
        float mouseYRotation = RotationInput.y * mouseSensitivity;

        //applies horizontal rotation
        transform.Rotate(0, mouseXRotation, 0);

        //applies vertical rotation to camera
        verticalRotation = Mathf.Clamp(verticalRotation - mouseYRotation, -upDownLookRange, upDownLookRange);
        MainCamera.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
    [Rpc(SendTo.Server)]
    private void MoveServerRpc(Vector2 MovementInput, Vector2 LookInput, bool JumpInput)
    {
        HandleRotation(LookInput);
        HandleMovement(MovementInput);
        HandleJumping(JumpInput);
    }
}
