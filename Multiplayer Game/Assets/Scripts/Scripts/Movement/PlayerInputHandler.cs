using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
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

    InputAction movementAction;
    InputAction rotationAction;
    InputAction jumpAction;
    InputAction sprintAction;
    InputAction pauseAction; 

    public Vector2 MovementInput {  get; private set; }
    public Vector2 RotationInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool SprintTriggered { get; private set; }
    public bool PauseTriggered { get; private set; }


    void Start()
    {
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);
        movementAction = mapReference.FindAction(movement);
        rotationAction = mapReference.FindAction(rotation);
        jumpAction = mapReference.FindAction(jump);
        sprintAction = mapReference.FindAction(sprint);
        pauseAction = mapReference.FindAction(pause);

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
    }
    void OnEnable()
    {
        playerControls.FindActionMap(actionMapName).Enable();
    }
    void OnDisable()
    {
        playerControls.FindActionMap(actionMapName).Disable();
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
