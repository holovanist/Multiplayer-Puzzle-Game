using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : NetworkBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] InputActionAsset playerControls;
    [Header("Action Map Name Reference")]
    [SerializeField] string actionMapName = "Player";
    [Header("Action Name References")]
    [SerializeField] string movement = "Movement";
    [SerializeField] string rotation = "Rotation";
    [SerializeField] string jump = "Jump";
    [SerializeField] string sprint = "Sprint";
    [SerializeField] string pause = "Pause";
    [SerializeField] string leftClick = "Left Click";
    [SerializeField] string crouch = "Crouch";
    [SerializeField] string interact = "Interact";

    InputAction movementAction;
    InputAction rotationAction;
    InputAction jumpAction;
    InputAction sprintAction;
    InputAction pauseAction; 
    InputAction leftClickAction; 
    InputAction crouchAction;
    InputAction interactAction;

    public Vector2 MovementInput {  get; private set; }
    public Vector2 RotationInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool SprintTriggered { get; private set; }
    public bool PauseTriggered { get; private set; }
    public bool LeftClickTriggered { get; private set; }
    public bool CrouchTriggered {  get; private set; }
    public bool InteractTriggered {  get; private set; }


    void Start()
    {
        if(!IsLocalPlayer) return;
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);
        movementAction = mapReference.FindAction(movement);
        rotationAction = mapReference.FindAction(rotation);
        jumpAction = mapReference.FindAction(jump);
        sprintAction = mapReference.FindAction(sprint);
        pauseAction = mapReference.FindAction(pause);
        leftClickAction = mapReference.FindAction(leftClick);
        crouchAction = mapReference.FindAction(crouch);
        interactAction = mapReference.FindAction(interact);

        SubscribeActionValuesToInputEvents();
    }
    void SubscribeActionValuesToInputEvents()
    {
        movementAction.performed += inputInfo => MovementInput = inputInfo.ReadValue<Vector2>();
        movementAction.canceled += inputInfo => MovementInput = Vector2.zero;

        rotationAction.performed += inputInfo => RotationInput = inputInfo.ReadValue<Vector2>();
        rotationAction.canceled += inputInfo => RotationInput = Vector2.zero;

        jumpAction.performed += inputInfo => JumpTriggered = true;
        jumpAction.canceled += inputInfo => JumpTriggered = false;

        sprintAction.performed += inputInfo => SprintTriggered = true;
        sprintAction.canceled += inputInfo => SprintTriggered = false;
        
        pauseAction.performed += inputInfo => PauseTriggered = true;
        pauseAction.canceled += inputInfo => PauseTriggered = false;

        leftClickAction.performed += inputInfo => LeftClickTriggered = true;
        leftClickAction.canceled += inputInfo => LeftClickTriggered = false;

        crouchAction.performed += inputInfo => CrouchTriggered = true;
        crouchAction.canceled += inputInfo => CrouchTriggered = false;
        
        interactAction.performed += inputInfo => InteractTriggered = true;
        interactAction.canceled += inputInfo => InteractTriggered = false;
    }
    void OnEnable()
    {
        playerControls.FindActionMap(actionMapName).Enable();
    }
    void OnDisable()
    {
        //playerControls.FindActionMap(actionMapName).Disable();
    }
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
