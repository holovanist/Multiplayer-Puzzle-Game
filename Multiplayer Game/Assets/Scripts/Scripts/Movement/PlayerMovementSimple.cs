using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementSimple : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float moveSpeed = 5;
    Vector2 moveDirection;
    public InputActionReference move;
    void Start()
    {
        CursorManager.Main.LockCursor();
    }
    private void Update()
    {
        //Set moveDir to move action imput
        moveDirection = move.action.ReadValue<Vector2>();
    }
    void FixedUpdate()
    {
        //Set player rb velocity to imput dir * move speed
        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y , moveDirection.y * moveSpeed);
    }
}
