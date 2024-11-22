using Firebase.Auth;
using Firebase.Extensions;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class NickNamePanel : MonoBehaviour
{
    [SerializeField] Text nickNameInputField;

    public void Confirm()
    {
        FirebaseUser user = BackendManager.Auth.CurrentUser;
        string nickName = nickNameInputField.text;
        if (nickName.IsNullOrEmpty())
            return;

        UserProfile profile = new UserProfile();
        profile.DisplayName = nickName;

        user.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("UpdateUserProfileAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                return;
            }

            Debug.Log("User profile updated successfully.");
            Debug.Log($"display name : {user.DisplayName}");
            Debug.Log($"email : {user.Email}");
            Debug.Log($"email verified : {user.IsEmailVerified}");
            Debug.Log($"user id : {user.UserId}");

            PhotonNetwork.LocalPlayer.NickName = nickName;
            PhotonNetwork.ConnectUsingSettings();
            gameObject.SetActive(false);
        });
    }
}
