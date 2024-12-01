using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerifyPanel : MonoBehaviour
{
    [SerializeField] GameObject nickNamePanel;

    private void OnEnable()
    {
        SendVerifyEmail();
    }

    private void OnDisable()
    {
        if (checkVerifyRoutine != null)
            StopCoroutine(checkVerifyRoutine);
    }

    private void SendVerifyEmail()
    {
        FirebaseUser user = BackendManager.Auth.CurrentUser;

        user.SendEmailVerificationAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SendEmailVerificationAsync was canceled.");
                gameObject.SetActive(false);
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SendEmailVerificationAsync encountered an error: " + task.Exception);
                gameObject.SetActive(false);
                return;
            }

            Debug.Log("Email sent successfully.");

            checkVerifyRoutine = StartCoroutine(CheckVerifyRoutine());
        });
    }

    Coroutine checkVerifyRoutine;

    IEnumerator CheckVerifyRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(3f);

        while (true)
        {
            // 인 증 확 인
            BackendManager.Auth.CurrentUser.ReloadAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                    return;

                if (BackendManager.Auth.CurrentUser.IsEmailVerified == true)
                {
                    nickNamePanel.gameObject.SetActive(true);
                    gameObject.SetActive(false);
                }
            });

            yield return delay;
        }
    }
}
