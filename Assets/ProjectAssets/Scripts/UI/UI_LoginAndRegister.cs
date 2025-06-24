using UnityEngine;
using TMPro;

public class UI_LoginAndRegister : MonoBehaviour
{
    [Header("Login")]
    [SerializeField] private TMP_InputField lEmail;
    [SerializeField] private TMP_InputField lPassword;
    [SerializeField] private TMP_Text lErrorText;

    [Header("Register")]
    [SerializeField] private TMP_InputField rEmail;
    [SerializeField] private TMP_InputField rPassword;
    [SerializeField] private TMP_Text rErrorText;

    private void Start()
    {
        ClearErrorMessage(lErrorText);
        ClearErrorMessage(rErrorText);
    }

    public void ClearAllInputFields()
    {
        ClearLoginFields();
        ClearRegisterFields();
        ClearErrorMessage(lErrorText);
        ClearErrorMessage(rErrorText);
    }

    public void ClearLoginFields()
    {
        if (lEmail != null)
        {
            lEmail.text = "";
        }   
        if (lPassword != null)
        {
            lPassword.text = "";
        }    
    }

    public void ClearRegisterFields()
    {
        if (rEmail != null)
        {
            rEmail.text = "";
        }  
        if (rPassword != null)
        {
            rPassword.text = "";
        }  
    }

    public void ClearErrorMessage(TMP_Text errorText)
    {
        if (errorText != null)
        {
            errorText.text = "";
        }        
    }

    public void AttemptLogin()
    {
        ClearErrorMessage(lErrorText);

        if (lEmail == null || lPassword == null)
        {
            ShowError(lErrorText, "Login input fields are not assigned!");
            return;
        }

        string email = lEmail.text.Trim();
        string password = lPassword.text;

        if (ValidateCredentials(email, password, lErrorText))
        {
            AuthenticationManager.Instance.Login(email, password, lErrorText);
        }
    }

    public void AttemptRegister()
    {
        ClearErrorMessage(rErrorText);

        if (rEmail == null || rPassword == null)
        {
            ShowError(rErrorText, "Register input fields are not assigned!");
            return;
        }

        string email = rEmail.text.Trim();
        string password = rPassword.text;

        if (ValidateCredentials(email, password, rErrorText))
        {
            AuthenticationManager.Instance.Register(email, password, rErrorText);
        }
    }

    private bool ValidateCredentials(string email, string password, TMP_Text errorText)
    {
        if (string.IsNullOrEmpty(email))
        {
            ShowError(errorText, "Email field is empty!");
            return false;
        }

        if (string.IsNullOrEmpty(password))
        {
            ShowError(errorText, "Password field is empty!");
            return false;
        }

        if (!email.Contains("@") || !email.Contains("."))
        {
            ShowError(errorText, "Please enter a valid email address!");
            return false;
        }

        if (password.Length < 6)
        {
            ShowError(errorText, "Password should be at least 6 characters long!");
            return false;
        }

        return true;
    }

    private void ShowError(TMP_Text uiText, string message)
    {
        if (uiText != null)
        {
            uiText.text = message;
            uiText.color = Color.red;
        }
    }
}