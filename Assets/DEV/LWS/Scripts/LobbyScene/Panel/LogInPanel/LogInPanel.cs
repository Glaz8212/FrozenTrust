using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Firebase.Extensions;
using Firebase.Auth;
using Photon.Pun;
using WebSocketSharp;
using UnityEngine.UI;

public class LogInPanel : UIBinder
{
    [Header ("로그인 패널")]
    [SerializeField] Text emailInputField;
    [SerializeField] Text passwordInputField;
    [SerializeField] GameObject errorPanel;
    [SerializeField] GameObject verifyPanel;
    [SerializeField] GameObject nickNamePanel;
    [SerializeField] GameObject SignUpPanel;
    [SerializeField] GameObject ResetPasswordPanel;

    private void Awake()
    {
        BindAll();
    }

    private void Start()
    {
        // 로그인 패널
        AddEvent("LogInButton", EventType.Click, LogIn);
        AddEvent("SignUpButton", EventType.Click, SignUp);
        AddEvent("ResetPasswordButton", EventType.Click, ResetPassword);
    }

    private void LogIn(PointerEventData eventData)
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        BackendManager.Auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("유저 로그인 실패");
                errorPanel.SetActive(true);
                return;
            }

            AuthResult result = task.Result;
            Debug.Log($"유저 로그인 성공");

            CheckUserInfo();
        });
    }

    private void CheckUserInfo()
    {
        FirebaseUser user = BackendManager.Auth.CurrentUser;
        if (user == null)
            return;

        Debug.Log($"display name : {user.DisplayName}");
        Debug.Log($"email : {user.Email}");
        Debug.Log($"email verified : {user.IsEmailVerified}");
        Debug.Log($"user id : {user.UserId}");

        if (!user.IsEmailVerified)
        {
            // 이메일 인증 진행
            verifyPanel.gameObject.SetActive(true);
        }
        else if (user.DisplayName.IsNullOrEmpty())
        {
            // 닉네임 설정 진행
            nickNamePanel.gameObject.SetActive(true);
        }
        else
        {
            // 접속 진행
            PhotonNetwork.LocalPlayer.NickName = user.DisplayName;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void SignUp(PointerEventData eventData)
    {
        SignUpPanel.gameObject.SetActive(true);
    }

    private void ResetPassword(PointerEventData eventData)
    {
        ResetPasswordPanel.gameObject.SetActive(true);
    }
}
