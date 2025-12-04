using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
public class MovementTest2 : NetworkBehaviour
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
    [Header("Multiplayer Data")]
    [SerializeField] private int tickRate = 60;
    private int currentTick;
    private float time;
    private float tickTime;

    private const int BUFFERSIZE = 1024;
    private MovementData[] clientMovementData = new MovementData[BUFFERSIZE];
    Vector2 CurrentRotation;
    private void Awake()
    {
        tickTime = 1f / tickRate;
    }
    void Start()
    {
        PlayerInputHandler = GetComponent<PlayerInputHandler>();
        PlayerInputHandler.LockCursor();
        MouseLocked = true;
    }
    void Update()
    {
        time += Time.deltaTime;
        if (!IsOwner || !IsClient) 
        {
            MainCamera.GetComponent<Camera>().enabled = false;
            MainCamera.gameObject.GetComponent<AudioListener>().enabled = false;
            return;
        }
        while (time > tickTime)
        {
            currentTick++;
            time -= tickTime;


            MovementAndRotation();
        }

        if (PlayerInputHandler.PauseTriggered)
        {
            PlayerInputHandler.UnlockCursor();
            MouseLocked = false;
        }
        if (!MouseLocked && PlayerInputHandler.LeftClickTriggered)
        {
            PlayerInputHandler.LockCursor();
            MouseLocked = true;
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
    void MovementAndRotation()
    {
        HandleMovement(PlayerInputHandler.MovementInput);
        HandleRotation(PlayerInputHandler.RotationInput);
        HandleJumping(PlayerInputHandler.JumpTriggered);
    }
    void HandleMovement(Vector2 Input)
    {
        Vector3 worldDirection = CalculateworldDirection(Input);
        currentMovement.x = worldDirection.x * CurrentSpeed;
        currentMovement.z = worldDirection.z * CurrentSpeed;

        characterController.Move(currentMovement * Time.deltaTime);

        clientMovementData[currentTick % BUFFERSIZE] = new MovementData
        {
            tick = currentTick,
            movementDirection = currentMovement,
            position = transform.position,
            rotation = CurrentRotation,
        };
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
        CurrentRotation = new(MainCamera.localRotation.x, transform.localPosition.y);
    }
    [Rpc(SendTo.Server)]
    private void MoveServerRpc()
    {
    }
    public class MovementData : INetworkSerializable
    {
        public int tick;
        public Vector3 movementDirection;
        public Vector3 position;
        public Vector2 rotation;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref tick);
            serializer.SerializeValue(ref movementDirection);
            serializer.SerializeValue(ref position);
            serializer.SerializeValue(ref rotation);
        }
    }
}
