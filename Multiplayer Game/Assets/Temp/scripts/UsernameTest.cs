using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UsernameTest : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextMeshProUGUI Username;
    public UsernameObject test;
    public InputActionAsset input;
    InputAction Enter;
    private void Awake()
    {
        Enter = input.FindAction("Enter");
    }
    string Chattext;

    void Update()
    {
        if (Enter.WasPressedThisFrame())
        {
            Chattext = inputField.text;
            if (Chattext != "")
                Username.text = Chattext;
            inputField.text = "";
        test.Username = Username.text;
        }

    }
}
