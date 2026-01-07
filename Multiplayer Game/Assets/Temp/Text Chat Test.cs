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
    string username;
    public UsernameObject Username;

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
    string Chattext;
    string ServerText;
    string ServerUsername;
    void Update()
    {
        if(Escape.IsPressed())
        {
            Chat.SetActive(true);
        }
        if(Enter.WasPressedThisFrame())
        {
            Chattext = inputField.text;
            ServerText = Chattext;
                username = Username.Username;
            if (Chattext != "" && ServerText != null)
            {
                SendChatRPC(Chattext, username);
            }
            Debug.Log(ServerText);
            inputField.text = "";
        }
        else if(Enter.WasReleasedThisFrame())
        {
            ScrollRect.verticalNormalizedPosition = 0;
        }

    }
    [Rpc(SendTo.ClientsAndHost)]
    public void SendChatRPC(string Chattext, string username)
    {
        ServerUsername = username;
        ServerText = Chattext;
        ChatText.text += "\n" + "("+ ServerUsername + ")" + ServerText;
    }    
}
