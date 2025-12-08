using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Movement")]
    public float walkSpeed;
    private float moveSpeed = 5;
    public float sprintSpeed;

    public float GroundDrag;
    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public float maxGroudTime;
    [HideInInspector]
    public bool grounded;



    [Header("References")]
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    float timer;

    PlayerInputHandler PlayerInputHandler;
    Vector3 moveDir;
    Rigidbody rb;

    public MovementState state; 
    
    [Header("Multiplayer Data")]
    [SerializeField] private int tickRate = 60;
    private int currentTick;
    private float time;
    private float tickTime;

    private const int BUFFERSIZE = 1024;
    private MovementData[] clientMovementData = new MovementData[BUFFERSIZE];
    readonly float maxPositionError = 0.5f;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }
    [HideInInspector]
    public bool crouching; 
    private void Awake()
    {
        tickTime = 1f / tickRate;
    }
    void Start()
    {
        if(!IsOwner) return;
        PlayerInputHandler = GetComponent<PlayerInputHandler>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        startYScale = transform.localScale.y;
        crouching = false;
    }
    private void Update()
    {
        if (!IsLocalPlayer) return;
        time += Time.deltaTime;
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        MyInput();
        SpeedControl();
        StateHandler();
        if (grounded)
            rb.linearDamping = GroundDrag;
        else
            rb.linearDamping = 0f;
    }

    private void FixedUpdate()
    {
        if(!IsLocalPlayer) return;
        if (grounded)
            timer += Time.fixedDeltaTime;
        else timer = 0;
        while (time > tickTime)
        {
            currentTick++;
            time -= tickTime;

            MovePlayer();
        }
    }

    private void MyInput()
    {
        horizontalInput = PlayerInputHandler.MovementInput.x;
        verticalInput = PlayerInputHandler.MovementInput.y;

        if (PlayerInputHandler.JumpTriggered && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (PlayerInputHandler.CrouchTriggered && !crouching)
        {
            crouching = true;
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            if (grounded)
                rb.linearVelocity = Vector3.down * 5f;
        }
        else if (!PlayerInputHandler.CrouchTriggered)
        {
            crouching = false;
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }
    private void StateHandler()
    {
        if (PlayerInputHandler.CrouchTriggered)
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        else if (grounded && PlayerInputHandler.SprintTriggered)
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
        }
    }


    private void MovePlayer()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (grounded)
            rb.linearVelocity = 10f * moveSpeed * tickTime * moveDir.normalized;
        else if (!grounded)
            rb.linearVelocity = 10f * airMultiplier * moveSpeed * tickTime * moveDir.normalized;
        clientMovementData[currentTick % BUFFERSIZE] = new MovementData
        {
            tick = currentTick,
            movementDirection = 10f * moveSpeed * tickTime * moveDir.normalized,
            position = transform.position,
            Grounded = grounded,

        };

        if (currentTick < 2) return;

        MoveServerRpc(clientMovementData[currentTick % BUFFERSIZE], clientMovementData[(currentTick - 1) % BUFFERSIZE],
            new ServerRpcParams { Receive = new ServerRpcReceiveParams { SenderClientId = OwnerClientId } });
    }
    private void SpeedControl()
    { 
        Vector3 flatVel = new(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limetedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new(limetedVel.x, rb.linearVelocity.y, limetedVel.z);
        }
    }
    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(jumpForce * tickTime * transform.up, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
    [ServerRpc]
    private void MoveServerRpc(MovementData currentMovementData, MovementData lastMovementData, ServerRpcParams parameters)
    {
        Vector3 startPosition = transform.position;

        Vector3 moveVector = lastMovementData.movementDirection;
        Physics.simulationMode = SimulationMode.Script;
        transform.position = lastMovementData.position;
        rb.linearVelocity = moveVector;
        Vector3 correctPosition = transform.position;
        transform.position = startPosition;
        Physics.simulationMode = SimulationMode.FixedUpdate;

        if (Vector2.Distance(correctPosition, currentMovementData.position) > maxPositionError)
        {
            ReconciliateClientRpc(currentMovementData.tick, new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new List<ulong>() { parameters.Receive.SenderClientId }
                }
            });
        }
    }
    [ClientRpc]
    private void ReconciliateClientRpc(int activationTick, ClientRpcParams parameters)
    {
        Vector3 correctPosition = clientMovementData[(activationTick - 1) % BUFFERSIZE].position;

        Physics.simulationMode = SimulationMode.Script;
        while (activationTick <= currentTick)
        {
            Vector3 moveVector = clientMovementData[(activationTick - 1) % BUFFERSIZE].movementDirection.normalized * moveSpeed;
            transform.position = correctPosition;
            rb.linearVelocity = moveVector;
            Physics.Simulate(Time.fixedDeltaTime);
            correctPosition = transform.position;
            clientMovementData[activationTick % BUFFERSIZE].position = correctPosition;
            activationTick++;
        }
        Physics.simulationMode = SimulationMode.FixedUpdate;

        transform.position = correctPosition;
    }
    [Serializable]
    public class MovementData : INetworkSerializable
    {
        public int tick;
        public float MoveSpeed;
        public Vector3 movementDirection;
        public Vector3 position;
        public bool Grounded;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref tick);
            serializer.SerializeValue(ref MoveSpeed);
            serializer.SerializeValue(ref movementDirection);
            serializer.SerializeValue(ref position);
            serializer.SerializeValue(ref Grounded);
        }
    }
}