using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackendManager : MonoBehaviour
{
    public static BackendManager instance;

    private FirebaseApp app;
    public static FirebaseApp App { get { return instance.app; } }

    private FirebaseAuth auth;
    public static FirebaseAuth Auth { get { return instance.auth; } }

    private FirebaseDatabase database;
    public static FirebaseDatabase Database { get { return instance.database; } }

    private void Awake()
    {
        // Ȯ�强�� ���� �̱������� ���� (�� ��ȯ �� ����)
        SetSingleton();
    }

    private void Start()
    {
        // Firebase ������ ���� �񵿱� Ȯ�� �� �ʱ�ȭ
        CheckDependency();
    }

    private void SetSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void CheckDependency()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                // ������ Ȯ�� �� ���� ���� ��� �ʱ�ȭ
                Debug.Log("���̾�̽� ������ üũ ���� !");

                app = FirebaseApp.DefaultInstance;
                auth = FirebaseAuth.DefaultInstance;
                database = FirebaseDatabase.DefaultInstance;
            }
            else
            {
                // ���� �߻��� ��� �α׿� �Բ� ���� ó��
                Debug.LogError($"{task.Result} : ���̾�̽� ������ üũ ����");

                app = null;
                auth = null;
                database = null;
            }
        });
    }
}
