using NewMovment;
using StarterAssets;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovement1 pm;
    private StarterAssetsInputs it;

    [Header("Settings")]
    public bool useCameraForward = true;
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVel = true;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float maxDashYSpeed;
    public float dashDuration;

    [Header("Cooldown")]
    public float dashCoolDown;
    private float dashCdTimer;

    private void Start()
    {
        pm = GetComponent<PlayerMovement1>();
        rb = GetComponent<Rigidbody>();
        it = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        if (it.Dash) Dash();

        if(dashCdTimer > 0 )
        {
            dashCdTimer -= Time.deltaTime;
        }
    }

    private void Dash()
    {
        if (dashCdTimer > 0) return;
        else dashCdTimer = dashCoolDown;
        pm.dashing = true;
        pm.maxYSpeed = maxDashYSpeed;

        Transform forwardT;

        if (useCameraForward)
            forwardT = playerCam;
        else forwardT = orientation;

        Vector3 dir = GetDirection(forwardT);

        Vector3 forceToApply = dir * dashForce + orientation.up * dashUpwardForce;

        if(disableGravity)
            rb.useGravity = false;

        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);
    }

    private Vector3 delayedForceToApply;
    private void DelayedDashForce()
    {
        if(resetVel)
            rb.linearVelocity = Vector3.zero;

        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        pm.dashing = false;
        pm.maxYSpeed = 0f;
        if(disableGravity)
            rb.useGravity = true;
    }

    private Vector3 GetDirection(Transform forwardT)
    {
        float horizontalInput = it.move.x;
        float verticalInput = it.move.y;

        Vector3 dir = new();

        if (allowAllDirections)
            dir = forwardT.forward * verticalInput + forwardT.right * horizontalInput;
        else
            dir = forwardT.forward;
        if (verticalInput == 0 && horizontalInput == 0)
            dir = forwardT.forward;

        return dir;
    }

}
