using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Player Control Parameters")]
    [SerializeField] float walkSpeed = 3.0f;
    [SerializeField] float sprintMultiplier = 2.0f;
    [SerializeField] float jumpForce = 5.0f;
    [SerializeField] float gravityMultiplier = 1.0f;
    [SerializeField] float mouseSensitivity = 0.1f;
    [SerializeField] float upDownLookRange = 80f;
    
    [Header("References")]    
    Animator anim;
    Crouching Crouch;
    [SerializeField] CharacterController characterController;
    [SerializeField] Camera MainCamera;
    [SerializeField] Camera UICamera;
    [SerializeField] GameObject PlayerModel;
    PlayerInputHandler Input;
    InputAction crouchinput;
    InputAction Jumpinput;
    Vector3 currentMovement;
    float verticalRotation;
    float CurrentSpeed => walkSpeed * (Input.SprintTriggered ? sprintMultiplier : 1) / (Input.CrouchTriggered && characterController.isGrounded ? crouchDivider : 1); 
    
    [Header("Crouching")]
    public float crouchDivider;
    Vector3 camPosition;
    public Vector3 CrouchCamPosition;
    bool crouching; 
    
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool WasGrounded;


    void Start()
    {
        Crouch = GetComponent<Crouching>();
        anim = GetComponentInChildren<Animator>();
        if(!IsLocalPlayer) GetComponent<PlayerMovement>().enabled = false;
        if (!IsOwner)
        {
            UICamera.enabled = false;
            MainCamera.GetComponent<Camera>().enabled = false;
            MainCamera.gameObject.GetComponent<AudioListener>().enabled = false;
        }
        if(IsLocalPlayer)
        {
            PlayerModel.GetComponent<SkinnedMeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            GetComponent<PlayerInput>().enabled = true;
            GetComponent<PlayerInputHandler>().enabled = true;
        }
        camPosition = MainCamera.transform.localPosition;
        Input = GetComponent<PlayerInputHandler>();
        Input.LockCursor();
        crouchinput = Input.playerControls.FindAction("Crouch");
        Jumpinput = Input.playerControls.FindAction("Jump");
    }
    private void Update()
    {
        if(transform.position.y <= -1000)
        {
            characterController.enabled = false;
            transform.position = GetComponent<Checkpoint>().position;
            characterController.enabled = true;
        }
        HandleAnimationRPC(crouching, Input.MovementInput, Input.SprintTriggered);
        HandleJumping();
        HandleCrouching();
        if(crouch && crouching)
        {
            Crouch.CrouchRPC(crouching, 1.4f, new(0, .65f, 0), 2, new(0, 0, 0));
            crouch = false;
        }
        else if (uncrouch && !crouching)
        {
            Crouch.CrouchRPC(false, 1.8f, new(0, .9f, 0), 3.6f, new(0, .7f, 0));
            uncrouch = false;
        }
        if (IsServer)
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
    bool wasCrouching = false;
    void HandleJumping()
    {
        if(crouching && Jumpinput.WasPressedThisFrame())
        {
            currentMovement.y = -0.5f;
            HandleCrouching(true);
        }
        else if (characterController.isGrounded && !crouching)
        {
            //currentMovement.y = -0.5f;
            if(Jumpinput.WasPressedThisFrame() || Jumpinput.IsPressed() && !wasCrouching)
            {
                wasCrouching = false;
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

        characterController.Move(currentMovement * Time.deltaTime);
        if (crouching && MainCamera.transform.localPosition != CrouchCamPosition)
        {
            MainCamera.transform.localPosition = Vector3.Lerp(MainCamera.transform.localPosition,CrouchCamPosition, .1f);
        }
        if (!crouching && MainCamera.transform.localPosition != camPosition)
        {
            MainCamera.transform.localPosition = Vector3.Lerp(MainCamera.transform.localPosition, camPosition, .1f);
        }
    }
    //[Rpc(SendTo.Everyone)]
    private void HandleAnimationRPC(bool Crouch, Vector2 MovementInput, bool SprintTriggered)
    {
        /*if (IsLocalPlayer)
        {*/
            if (Crouch)
            {
                time += Time.deltaTime;
            }
            if (MovementInput == Vector2.zero)
            {
                anim.SetBool("Walk", false);

                if (time > 1f)
                    SetAnimationSpeedRPC(0);
            }
            else
            {
                anim.SetBool("Walk", true);
                SetAnimationSpeedRPC(1);
            }
            if (!Crouch)
            {
                SetAnimationSpeedRPC(1);
                time = 0f;
            }
            if (SprintTriggered && MovementInput.y == 1)
            {
                SetAnimationSpeedRPC(2);
                anim.SetBool("BackwordsWalk", false);
            }
            else if (!SprintTriggered && MovementInput.y == 1)
            {
                SetAnimationSpeedRPC(1);
                anim.SetBool("BackwordsWalk", false);
            }
            else if (SprintTriggered && MovementInput.y == -1)
            {
                anim.SetBool("BackwordsWalk", true);
                SetAnimationSpeedRPC(2);
            }
            else if (!SprintTriggered && MovementInput.y == -1)
            {
                anim.SetBool("BackwordsWalk", true);
                SetAnimationSpeedRPC(1);
            }
            else if (MovementInput.y == 0)
                anim.SetBool("BackwordsWalk", false);
            if (characterController.isGrounded && !WasGrounded)
            {
                anim.SetBool("Grounded", true);
                WasGrounded = true;
            }
            if (!characterController.isGrounded && WasGrounded)
            {
                if (Crouch) return;
                anim.SetTrigger("Jump");
                anim.SetBool("Grounded", false);
                WasGrounded = false;
            }
            if (!Crouch && IsLocalPlayer)
                anim.SetTrigger("Stand");
            else if (IsLocalPlayer)
                anim.SetTrigger("Crouch");
            if (IsLocalPlayer)
                anim.SetBool("Crouching", Crouch);
       // }
    }
    [Rpc(SendTo.Everyone)]
    void SetAnimationSpeedRPC(float Speed)
    {
        anim.speed = Speed;
    }   
    bool crouch;
    bool uncrouch;
    void HandleCrouching()
    {
        if (crouchinput.WasPressedThisFrame() && !crouching && characterController.isGrounded)
        {
            crouching = true;
            crouch = true;
            wasCrouching = true;
        }
        else if (crouchinput.WasPressedThisFrame() && crouching)
        {
            if(crouchinput.WasPressedThisFrame())
            crouching = false;
            uncrouch = true;
            wasCrouching = false;

            time = 0f;
        }
    }    
    void HandleCrouching(bool Jumping)
    {
        if (Jumping)
        {
            crouching = false;
            Crouch.CrouchRPC(false, 1.8f, new(0, .9f, 0), 3.6f, new(0, .7f, 0));
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
