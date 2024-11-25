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
        // 확장성을 위해 싱글톤으로 관리 (씬 전환 시 유지)
        SetSingleton();
    }

    private void Start()
    {
        // Firebase 의존성 상태 비동기 확인 및 초기화
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
                // 의존성 확인 및 문제 없을 경우 초기화
                Debug.Log("파이어베이스 의존성 체크 성공 !");

                app = FirebaseApp.DefaultInstance;
                auth = FirebaseAuth.DefaultInstance;
                database = FirebaseDatabase.DefaultInstance;
            }
            else
            {
                // 문제 발생의 경우 로그와 함께 에러 처리
                Debug.LogError($"{task.Result} : 파이어베이스 의존성 체크 실패");

                app = null;
                auth = null;
                database = null;
            }
        });
    }
}
