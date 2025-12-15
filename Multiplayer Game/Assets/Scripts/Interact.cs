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
    float timer = 0;
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
        timer += Time.deltaTime;
        if(_Interact.IsPressed())
            LeverInteraction();
        else if(TempLever != null)
        {
            TempLever.Interact = false;
        }
    }
    Lever TempLever;
    private void LeverInteraction()
    {
        Ray ray = new(_Camera.transform.position, _Camera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            if (hit.collider.gameObject.CompareTag("Lever"))
            {
                Lever InteractObject = hit.collider.gameObject.GetComponent<Lever>();
                if (InteractText != null)
                    InteractText.enabled = true;
                TempLever = InteractObject;
                if(!InteractObject.WasPulled)
                InteractObject.Interact = true;
                if(!InteractObject.HoldLever)
                {
                    if (timer > InteractDelay)
                    {
                        timer = 0;
                        if (InteractText != null)
                            InteractText.enabled = false;
                        InteractObject.OnInteract();
                    }
                }
                else
                {
                    if(InteractObject.IsButton && InteractObject.HoldLever)
                    {
                        if (InteractText != null)
                            InteractText.enabled = false;
                        InteractObject.OnInteract();
                    }
                    else if (timer > InteractDelay)
                    {
                        if (InteractText != null)
                            InteractText.enabled = false;
                        InteractObject.OnInteract();
                        if(InteractObject.LeverActive)
                        {
                            timer = -1;
                        }
                        else if (InteractObject.Timer > InteractObject.HoldTime && !InteractObject.LeverActive)
                        {
                            timer = 0;
                        }
                    }
                }
            }
            else
            {
                if (InteractText != null)
                    InteractText.enabled = false;
            }
        }
        else
        {
            if (InteractText != null)
                InteractText.enabled = false;
        }
    }
}
