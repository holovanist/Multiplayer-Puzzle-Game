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
        
    }
    private void Update()
    {
        moveDirection = move.action.ReadValue<Vector2>();
    }
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y , moveDirection.y * moveSpeed);
    }
}
