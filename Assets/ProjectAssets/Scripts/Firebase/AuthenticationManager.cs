using System.Collections;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using UnityEngine.Events;
using TMPro;

public class AuthenticationManager : NonPersistentSingleton<AuthenticationManager>
{
    [Header("User Data")]
    [SerializeField] private UserDataSO currentUserData;

    [Header("Auth Events")]
    public UnityEvent OnLoginSuccessful;
    public UnityEvent OnRegisterSuccessful;

    private FirebaseAuth authReference;

    private void Awake()
    {
        authReference = FirebaseAuth.GetAuth(FirebaseApp.DefaultInstance);
        currentUserData.ResetData();
    }

    public void Register(string email, string password)
    {
        StartCoroutine(RegisterUserCoroutine(email, password));
    }

    public void Register(string email, string password, TMP_Text errorText)
    {
        StartCoroutine(RegisterUserCoroutine(email, password, errorText));
    }
    public void Login(string email, string password)
    {
        StartCoroutine(LoginUserCoroutine(email, password));
    }

    public void Login(string email, string password, TMP_Text errorText)
    {
        StartCoroutine(LoginUserCoroutine(email, password, errorText));
    }

    public void LogOut()
    {
        authReference.SignOut();
        currentUserData.ResetData();
    }

    private IEnumerator RegisterUserCoroutine(string email, string password)
    {
        var registerTask = authReference.CreateUserWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            Debug.LogError("Registration failed: " + registerTask.Exception);
        }
        else
        {
            OnRegisterSuccessful?.Invoke();
        }
    }

    private IEnumerator RegisterUserCoroutine(string email, string password, TMP_Text errorText)
    {
        var registerTask = authReference.CreateUserWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            errorText.text = "Registration failed: " + registerTask.Exception;
        }
        else
        {
            OnRegisterSuccessful?.Invoke();
        }
    }

    private IEnumerator LoginUserCoroutine(string email, string password)
    {
        var loginTask = authReference.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogError("Login failed: " + loginTask.Exception);
        }
        else
        {
            currentUserData.SetUserData(loginTask.Result.User.UserId, loginTask.Result.User.Email);
            OnLoginSuccessful?.Invoke();
            Debug.Log("Logged in: " + currentUserData.UserId + " , " + currentUserData.Email);
        }
    }

    private IEnumerator LoginUserCoroutine(string email, string password, TMP_Text errorText)
    {
        var loginTask = authReference.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            errorText.text = "Login failed: " + loginTask.Exception;
        }
        else
        {
            currentUserData.SetUserData(loginTask.Result.User.UserId, loginTask.Result.User.Email);
            OnLoginSuccessful?.Invoke();
            Debug.Log("Logged in: " + currentUserData.UserId + " , " + currentUserData.Email);
        }
    }
}