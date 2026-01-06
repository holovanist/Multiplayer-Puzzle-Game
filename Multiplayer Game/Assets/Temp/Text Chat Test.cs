using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class TextChatTest : NetworkBehaviour
{
    //needs to work in multiplayer
    //needs to be used on a screen not just floating
    public TMP_InputField inputField;
    public GameObject Chat;
    public TextMeshProUGUI ChatText;
    public ScrollRect ScrollRect;

    public InputActionAsset input;
    InputAction Escape;
    InputAction Enter;
    private void Awake()
    {
        Escape = input.FindAction("Pause");
        Enter = input.FindAction("Enter");
    }
    private void Start()
    {
        Chat.SetActive(false);
    }
    void Update()
    {
        if(Escape.IsPressed())
        {
            Chat.SetActive(true);
        }
        if(Enter.WasPressedThisFrame())
        {
            string Chattext;
            Chattext = inputField.text;
            if(Chattext != "")
            ChatText.text += "\n" + "(Player 1)"+ Chattext;
        }
        else if(Enter.WasReleasedThisFrame())
        {
            ScrollRect.verticalNormalizedPosition = 0;
        }

    }
}
