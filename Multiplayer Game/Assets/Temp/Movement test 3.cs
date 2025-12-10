using Unity.Netcode;
using UnityEngine;
using UnityEngine.Windows;

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
    PlayerInputHandler Input;
    Vector3 currentMovement;
    float verticalRotation;
    float CurrentSpeed => walkSpeed * (Input.SprintTriggered ? sprintMultiplier : 1) / (Input.CrouchTriggered ? crouchDivider : 1); 
    
    [Header("Crouching")]
    public float crouchDivider;
    public float crouchYScale;
    private float startYScale;
    bool crouching; 
    
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    //bool grounded;

    void Start()
    {
        if (!IsOwner)
        {
            MainCamera.GetComponent<Camera>().enabled = false;
            MainCamera.gameObject.GetComponent<AudioListener>().enabled = false;
        }
        startYScale = transform.localScale.y;
        Input = GetComponent<PlayerInputHandler>();
        Input.LockCursor();
    }
    void Update()
    {
        //grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        HandleMovement();
        HandleRotation();
    }
    Vector3 CalculateworldDirection()
    {
        Vector3 inputDirection = new(Input.MovementInput.x, 0f, Input.MovementInput.y);
        Vector3 worldirection = transform.TransformDirection(inputDirection);
        return worldirection.normalized;
    }
    void HandleJumping()
    {
        if (characterController.isGrounded)
        {
            currentMovement.y = -0.5f;
            if (Input.JumpTriggered)
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
        if (Input.CrouchTriggered && !crouching)
        {
            crouching = true;
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        }
        else if (!Input.CrouchTriggered)
        {
            crouching = false;
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }
    void HandleRotation()
    {
        float mouseXRotation = Input.RotationInput.x * mouseSensitivity;
        float mouseYRotation = Input.RotationInput.y * mouseSensitivity;

        //applies horizontal rotation
        transform.Rotate(0, mouseXRotation, 0);

        //applies vertical rotation to camera
        verticalRotation = Mathf.Clamp(verticalRotation - mouseYRotation, -upDownLookRange, upDownLookRange);
        MainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
}
