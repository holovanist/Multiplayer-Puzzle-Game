using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

#if ENABLE_INPUT_SYSTEM
    [RequireComponent(typeof(PlayerInputHandler))]
#endif
public class Interact : MonoBehaviour
{
    private InputAction _Interact;

    [SerializeField]
    float interactRange = 4;
    [SerializeField]
    TextMeshProUGUI InteractText;
    public float InteractDelay;
    public GameObject _Camera;
    float DelayTimer = 0;
    Lever TempLever;
    Monitor TempMonitor;
    // Start is called before the first frame update
    void Start()
    {
        if (InteractText != null)
            InteractText.enabled = false;
        _Interact = GetComponent<PlayerInputHandler>().playerControls.FindAction("Interact");
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new(_Camera.transform.position, _Camera.transform.forward);
        if (InteractDelay < DelayTimer && Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            if (HitInteractableObject(hit))
            {
                if (InteractText != null)
                    InteractText.enabled = true;
            }
        }
        else
        {
            if (InteractText != null)
                InteractText.enabled = false;
        }
        DelayTimer += Time.deltaTime;
        if(_Interact.IsPressed())
            Interaction();
        else if(TempLever != null)
        {
            TempLever.Interact = false;
        }
    }
    private void Interaction()
    {
        Ray ray = new(_Camera.transform.position, _Camera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            if (hit.collider.gameObject.CompareTag("Lever"))
            {
                Lever InteractObject = hit.collider.gameObject.GetComponent<Lever>();
                TempLever = InteractObject;
                if(!InteractObject.WasPulled)
                InteractObject.Interact = true;
                if(!InteractObject.HoldLever)
                {
                    if (DelayTimer > InteractDelay)
                    {
                        DelayTimer = 0;
                        InteractObject.OnInteract();
                    }
                }
                else
                {
                    if(InteractObject.IsButton && InteractObject.HoldLever)
                    {
                        InteractObject.OnInteract();
                    }
                    else if (DelayTimer > InteractDelay)
                    {
                        InteractObject.OnInteract();
                        if(InteractObject.LeverActive)
                        {
                            DelayTimer = -1;
                        }
                        else if (InteractObject.Timer > InteractObject.HoldTime && !InteractObject.LeverActive)
                        {
                            DelayTimer = 0;
                        }
                    }
                }
            }
            else if (hit.collider.gameObject.CompareTag("Monitor"))
            {
                Monitor InteractObject = hit.collider.gameObject.GetComponent<Monitor>();
                TempMonitor = InteractObject;
                if (DelayTimer > InteractDelay)
                {
                    DelayTimer = 0;
                    InteractObject.SetBool(GetComponentInChildren<Camera>());
                }
            }
            else if (hit.collider.gameObject.CompareTag("Rotate Wire clockwise"))
            {
                Circutboard InteractObject = hit.collider.gameObject.GetComponent<Circutboard>();
                if (DelayTimer > InteractDelay)
                {
                    DelayTimer = 0;
                    InteractObject.RotateWireClockwiseRPC();
                }
            }
            else if (hit.collider.gameObject.CompareTag("Rotate Wire counter clockwise"))
            {
                Circutboard InteractObject = hit.collider.gameObject.GetComponent<Circutboard>();
                if (DelayTimer > InteractDelay)
                {
                    DelayTimer = 0;
                    InteractObject.RotateWireCounterClockwiseRPC();
                }
            }
        }
    }
    bool HitInteractableObject(RaycastHit hit)
    {
        bool interactable = false;
        if(hit.collider.gameObject.CompareTag("Lever") || hit.collider.gameObject.CompareTag("Monitor") || hit.collider.gameObject.CompareTag("Rotate Wire clockwise") || hit.collider.gameObject.CompareTag("Rotate Wire counter clockwise"))
        {
            interactable = true;
        }
        else
        {
            interactable = false;
        }
        return interactable;
    }
}
