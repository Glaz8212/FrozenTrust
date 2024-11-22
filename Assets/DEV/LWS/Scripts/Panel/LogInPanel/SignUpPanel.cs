using Firebase.Extensions;
using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;

public class SignUpPanel : MonoBehaviour
{
    [SerializeField] Text emailInputField;
    [SerializeField] Text passwordInputField;
    [SerializeField] Text passwordConfirmField;

    public void SignUp()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;
        string confirm = passwordConfirmField.text;

        if (email.IsNullOrEmpty())
            return;

        if (password != confirm)
            return;

        BackendManager.Auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            AuthResult result = task.Result;
            Debug.Log($"Firebase user created successfully: {result.User.DisplayName} ({result.User.UserId})");
            gameObject.SetActive(false);
        });
    }
}
