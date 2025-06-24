using UnityEngine;

[CreateAssetMenu(fileName = "UserData", menuName = "Firebase/UserData")]
public class UserDataSO : ScriptableObject
{
    [Header("Data")]
    [SerializeField] private string userId = "";
    [SerializeField] private string email = "";
    [SerializeField] private bool firstSession = true;

    public string UserId
    {
        get
        {
            return userId;
        }
        set
        {
            userId = value;
        }
    }

    public string Email
    {
        get
        {
            return email;
        }
        set
        {
            email = value;
        }
    }

    public bool IsLoggedIn
    {
        get
        {
            if (string.IsNullOrEmpty(userId) == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool FirstSession
    {
        get
        {
            return firstSession;
        }
        set
        {
            firstSession = value;
        }
    }

    public void ResetData()
    {
        userId = "";
        email = "";
    }

    public void SetUserData(string id, string email)
    {
        userId = id;
        this.email = email;
    }
}