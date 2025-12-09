using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace player
{
#if ENABLE_INPUT_SYSTEM
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class Interact : MonoBehaviour
    {
        private PlayerInputHandler _input;

        [SerializeField]
        float interactRange = 4;
        [SerializeField]
        TextMeshProUGUI InteractText;
        public float InteractDelay;
        float timer = 0;
        // Start is called before the first frame update
        void Start()
        {
            if (InteractText != null)
                InteractText.enabled = false;
            _input = GetComponent<PlayerInputHandler>();
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
            Ray ray = new(Camera.main.transform.position, Camera.main.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
            {
                if (hit.collider.gameObject.CompareTag("Lever"))
                {
                    if(InteractText != null)
                        InteractText.enabled = true;
                    if (_input.InteractTriggered  && timer > InteractDelay)
                    {
                        timer = 0;
                        //needs a small cooldown
                        if (InteractText != null)
                            InteractText.enabled = false;
                        hit.collider.gameObject.GetComponent<Lever>().OnInteract();
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
}