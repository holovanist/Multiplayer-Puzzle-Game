using StarterAssets;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : NetworkBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed,sprintSpeed,maxYSpeed,GroundDrag,AirDrag;
    public bool AddAirDrag;

    [Header("Jumping")]
    public float jumpForce,jumpCooldown,airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed,crouchYScale;
    private float startYScale;
        

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    [HideInInspector]
    public bool grounded;

    

    [Header("References")]
    public Transform orientation;
    public TextMeshProUGUI Speed;
    Vector2 MoveInput;
    Vector3 moveDir;
    public Rigidbody rb;
    //private StarterAssetsInputs it;
    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }
    [HideInInspector]
    public bool crouching;
    [HideInInspector]
    public bool jumping;


    InputAction Move;
    InputAction Crouch;
    InputAction JumpInput;
    InputAction Sprint;
    bool crouch;
    bool sprint;
    private void Awake()
    {
        Move = InputSystem.actions.FindAction("Move");
        JumpInput = InputSystem.actions.FindAction("Jump");
        Crouch = InputSystem.actions.FindAction("Crouch");
        Sprint = InputSystem.actions.FindAction("Sprint");
    }
    void Start()
    {
        //it = GetComponent<StarterAssetsInputs>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 
        readyToJump = true;
        startYScale = transform.localScale.y;
        crouching = false;
        state = MovementState.walking;
    }
    private void Update()
    {
        if (!IsOwner) return;
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        SetVariablesRpc();
        MyInput();
        SpeedControlRpc();
        StateHandler();

    }
    [Rpc(SendTo.Server)]
    private void SetVariablesRpc()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        if (grounded)
            rb.linearDamping = GroundDrag;
        else if (AddAirDrag && !grounded)
            rb.linearDamping = AirDrag;
        else
            rb.linearDamping = 0f;
        if (Speed != null)
            Speed.SetText("Speed: " + string.Format("{0:0.00}", rb.linearVelocity.magnitude));
    }
    private void FixedUpdate()
    {
        if (!IsOwner) return;
        if (IsServer && IsLocalPlayer)
            MovePlayer(MoveInput);
        else if (IsLocalPlayer)
        MovePlayerRpc(MoveInput, orientation.forward, orientation.right, moveSpeed);
    }
    private void MyInput()
    {
        MoveInput = Move.ReadValue<Vector2>();
        crouch = Crouch.IsPressed();
        sprint = Sprint.IsPressed();
        if (JumpInput.WasPressedThisFrame() && readyToJump && grounded)
        {
            readyToJump = false;
            JumpRpc();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (crouch  && grounded)
        {
            CrouchingRpc();
        }
    }
    private void StateHandler()
    {
        if (crouch)
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        else if (grounded && sprint)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
            if (moveSpeed < sprintSpeed)
                moveSpeed = walkSpeed;
            else moveSpeed = sprintSpeed;
        }
    }
    [Rpc(SendTo.Server)]
    private void CrouchingRpc()
    {
        if (!crouching)
        {
            crouching = true;
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            transform.position = new Vector3(transform.position.x, transform.position.y - crouchYScale, transform.position.z);
        }
        else
        {
            crouching = false;
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            transform.position = new Vector3(transform.position.x, transform.position.y + crouchYScale, transform.position.z);
        }
    }
    [Rpc(SendTo.Server)]
    private void MovePlayerRpc(Vector2 _MoveInput, Vector3 _ForwardOrientation, Vector3 _RightOrientation, float MoveSpeed)
    {
        MoveInput = _MoveInput;
        moveDir = _ForwardOrientation * _MoveInput.y + _RightOrientation * _MoveInput.x;
        rb.AddForce(10f * moveSpeed * moveDir.normalized, ForceMode.Force);
        if (grounded)
            rb.AddForce(10f * MoveSpeed * moveDir.normalized, ForceMode.Force);
        else if(!grounded)
            rb.AddForce(10f * airMultiplier * moveSpeed * moveDir.normalized, ForceMode.Force);
    }
    private void MovePlayer(Vector2 _MoveInput)
    {
        moveDir = orientation.forward * _MoveInput.y + orientation.right * _MoveInput.x;
        if (grounded)
            rb.AddForce(10f * moveSpeed * moveDir.normalized, ForceMode.Force);
        else if (!grounded)
            rb.AddForce(10f * airMultiplier * moveSpeed * moveDir.normalized, ForceMode.Force);
    }
    [Rpc(SendTo.Server)]
    private void SpeedControlRpc()
    {
        Vector3 flatVel = new(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limetedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limetedVel.x, rb.linearVelocity.y, limetedVel.z);
        }           
        if(maxYSpeed != 0 && rb.linearVelocity.y > maxYSpeed)
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, maxYSpeed, rb.linearVelocity.z);
    }
    [Rpc(SendTo.Server)]
    private void JumpRpc()
    {
        jumping = true;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f , rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        jumping = false;
        readyToJump = true;
    }
}