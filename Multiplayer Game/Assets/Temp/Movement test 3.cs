using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] GameObject PlayerModel;
    PlayerInputHandler Input;
    Vector3 currentMovement;
    float verticalRotation;
    float CurrentSpeed => walkSpeed * (Input.SprintTriggered ? sprintMultiplier : 1) / (Input.CrouchTriggered ? crouchDivider : 1); 
    
    [Header("Crouching")]
    public float crouchDivider;
    float camPosition;
    public Vector3 CrouchCamPosition;
    bool crouching; 
    
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool WasGrounded;
    Animator anim;


    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        if (!IsOwner)
        {
            MainCamera.GetComponent<Camera>().enabled = false;
            MainCamera.gameObject.GetComponent<AudioListener>().enabled = false;
        }
        if(IsLocalPlayer)
        {
            GetComponent<PlayerInput>().enabled = true;
            GetComponent<PlayerInputHandler>().enabled = true;
        }
        camPosition = MainCamera.transform.position.y;
        Input = GetComponent<PlayerInputHandler>();
        Input.LockCursor();
    }
    private void Update()
    {
        if (Input.SprintTriggered && Input.MovementInput.y == 1)
        {
            anim.speed = 2;
            anim.SetBool("BackwordsWalk", false);
        }
        else if (!Input.SprintTriggered && Input.MovementInput.y == 1)
        {
            anim.speed = 1;
            anim.SetBool("BackwordsWalk", false);
        }
        else if (Input.SprintTriggered && Input.MovementInput.y == -1)
        {
            anim.SetBool("BackwordsWalk", true);
            anim.speed = 2;
        }
        else if (!Input.SprintTriggered && Input.MovementInput.y == -1)
        {
            anim.SetBool("BackwordsWalk", true);
            anim.speed = 1;
        }
        else if (Input.MovementInput.y == 0)
            anim.SetBool("BackwordsWalk", false);
        if(characterController.isGrounded && !WasGrounded)
        {
            anim.SetBool("Grounded", true);
            WasGrounded = true;
        }        
        if(!characterController.isGrounded && WasGrounded)
        {
                anim.SetTrigger("Jump");
            anim.SetBool("Grounded", false);
            WasGrounded = false;
        }
        if(IsServer)
            HandleRotation();
        else
            HandleRotationRPC(Input.RotationInput.x * mouseSensitivity, Input.RotationInput.y * mouseSensitivity);
    }
    void FixedUpdate()
    {
        //grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        HandleMovement();
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
    float time;
    void HandleMovement()
    {
        Vector3 worldDirection = CalculateworldDirection();
        currentMovement.x = worldDirection.x * CurrentSpeed;
        currentMovement.z = worldDirection.z * CurrentSpeed;
            if (crouching)
            {
                time += Time.deltaTime;
            }
        if(Input.MovementInput == Vector2.zero)
        {
            anim.SetBool("Walk", false);

                if(time > 1f)
                {
                    anim.speed = 0f;
                }

        }
        else
        {
            anim.SetBool("Walk", true);
                anim.speed = 1f;
        }
        if (!crouching)
            anim.speed = 1f;
        HandleJumping();
        characterController.Move(currentMovement * Time.deltaTime); 
        if (Input.CrouchTriggered && !crouching)
        {
            crouching = true;
            anim.SetBool("Crouching", true);
            anim.SetTrigger("Crouch");
            //lerp to new position
            MainCamera.transform.localPosition = Vector3.Lerp(MainCamera.transform.localPosition,CrouchCamPosition, 1f);
        }
        else if (!Input.CrouchTriggered && crouching)
        {
            crouching = false;
            anim.SetTrigger("Stand");
            anim.SetBool("Crouching", false); 

            MainCamera.transform.localPosition = new Vector3(MainCamera.transform.localPosition.x, camPosition, MainCamera.transform.localPosition.z);
            time = 0f;
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
    [Rpc(SendTo.ClientsAndHost)]
    void HandleRotationRPC(float mouseXRotation, float mouseYRotation)
    {
        //applies horizontal rotation
        transform.Rotate(0, mouseXRotation, 0);

        //applies vertical rotation to camera
        verticalRotation = Mathf.Clamp(verticalRotation - mouseYRotation, -upDownLookRange, upDownLookRange);
        MainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
}
