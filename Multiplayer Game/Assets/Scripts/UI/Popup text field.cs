using TMPro;
using UnityEngine;

public class Popuptextfield : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextMeshProUGUI Text;
    public UsernameObject test;
    public GameObject TextField;
    string text;
    string oldtext;
    bool TextBoxOpen;
    private void Update()
    {
        if(TextBoxOpen)
        {
            text = inputField.text;
            Text.text = text;
        }
    }
    public void OpenTextBox()
    {
        TextBoxOpen = true;
        TextField.SetActive(true);
        inputField.text = Text.text;
        oldtext = Text.text;
    }
    public void Confirm()
    {
        if (text != "")
        {
            inputField.text = "";
            test.Username = Text.text;
            TextField.SetActive(false);
            TextBoxOpen = false;
        }
        else
        {
            inputField.text = "";
            Text.text = oldtext;
            TextField.SetActive(false);
            TextBoxOpen = false;
        }
    }
    public void Cancel()
    {
        inputField.text = "";
        Text.text = oldtext;
        TextField.SetActive(false); 
        TextBoxOpen = false;
    }
}
