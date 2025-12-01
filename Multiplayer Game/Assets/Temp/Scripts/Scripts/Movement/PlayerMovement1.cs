using StarterAssets;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : NetworkBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed,sprintSpeed,maxYSpeed,GroundDrag;

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
    float horizontalInput;
    float verticalInput;
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
        MyInput();
        SpeedControl();
        StateHandler();
        if (grounded)
            rb.linearDamping = GroundDrag;
        else
            rb.linearDamping = 0f;
        if(Speed != null)
        Speed.SetText("Speed: " + string.Format("{0:0.00}", rb.linearVelocity.magnitude));
    }
    private void FixedUpdate()
    {
        if (!IsOwner) return;
        MovePlayer();
    }
    private void MyInput()
    {
        horizontalInput = Move.ReadValue<Vector2>().x;
        verticalInput = Move.ReadValue<Vector2>().y;
        if (JumpInput.WasPressedThisFrame() && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (Crouch.WasPressedThisFrame() && !crouching && grounded)
        {
            crouching = true;
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            transform.position = new Vector3(transform.position.x, transform.position.y - crouchYScale, transform.position.z);
        }
        else if(Crouch.WasPressedThisFrame() && grounded)
        {
            crouching = false;
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            transform.position = new Vector3(transform.position.x, transform.position.y + crouchYScale, transform.position.z);
        }
    }
    private void StateHandler()
    {
        if (Crouch.WasPressedThisFrame())
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        else if (grounded && Sprint.IsPressed())
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
    private void MovePlayer()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (grounded)
            rb.AddForce(10f * moveSpeed * moveDir.normalized, ForceMode.Force);
        else if(!grounded)
            rb.AddForce(10f * airMultiplier * moveSpeed * moveDir.normalized, ForceMode.Force);
    }
    private void SpeedControl()
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
    private void Jump()
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