using UnityEngine;
using TMPro;

public class UI_LoginAndRegister : MonoBehaviour
{
    [Header("Login")]
    [SerializeField] private TMP_InputField lEmail;
    [SerializeField] private TMP_InputField lPassword;

    [Header("Register")]
    [SerializeField] private TMP_InputField rEmail;
    [SerializeField] private TMP_InputField rPassword;
}
