using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InputWindow : MonoBehaviour
{
    [SerializeField] Button confirmButton;
    [SerializeField] Button cancelButton;
    [SerializeField] TextMeshProUGUI TitleText;
    [SerializeField] TMP_InputField inputField;
    private void Awake()
    {
        Hide();   
    }
    public void GetTitle(string titleString)
    {
        TitleText.text = titleString;
    }
    public void GetInputText(string inputString)
    {
        inputField.text = inputString;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }    
}
