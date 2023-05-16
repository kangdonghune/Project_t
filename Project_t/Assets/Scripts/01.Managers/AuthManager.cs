using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AuthManager 
{
    //파이어 베이스를 구동할 수 있는 지 여부에 대한 값
    private bool IsFirebaseReady { get; set; } = false;
    //중복 사인을 방지하기 위해 파이어 베이스가 진행 중인지 여부에 대한 값
    private bool IsSignInOnProgress { get; set; } = false;

    //파이어 베이스 전체 어플리케이션을 관리하는 변수
    public FirebaseApp firebaseApp { get; private set; }
    //파이어 베이스 중에서도 Auth 관련 기능을 관리하는 변수
    public FirebaseAuth firebaseAuth { get; private set; }
    //파이어베이스 Auth를 통해서 가져온 유저 정보를 할당하는 변수
    public FirebaseUser User { get; private set; }

    //접속과 관련된 ui on off는 해당 ui 스크립트에서 하고 여기선 이벤트로 관리
    public Action OnAction;
    public Action OffAction;

    public void Init()
    {
        //OffAction?.Invoke(); //파이어 베이스 연결 전 접속 관련 ui는 비활성화
        //async 함수이기에 콜백을 걸어 관리를 해야한다.
        //continuewith() 함수가 pc 환경에서 버젼에 따라 간혹 널 레퍼런스가 발생하는 경우가 있어 onmainthread 함수로 대체
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var result = task.Result;
            if (result != DependencyStatus.Available) //파이어 베이스가 사용 불가 상태라면
            {
                Debug.LogError(result.ToString());
                IsFirebaseReady = false;
            }
            else
            {
                Debug.Log("Firebase is Ready");
                IsFirebaseReady = true;
                firebaseApp = FirebaseApp.DefaultInstance;
                firebaseAuth = FirebaseAuth.DefaultInstance;
                firebaseAuth.SignOut();
                firebaseAuth.StateChanged -= Onchanged;
                firebaseAuth.StateChanged += Onchanged;
            }
            //OnAction?.Invoke();
        }
        );
    }

    private void Onchanged(object sender, EventArgs e)
    {
        if(firebaseAuth.CurrentUser != User)
        {
            bool signed = firebaseAuth.CurrentUser != FirebaseAuth.DefaultInstance.CurrentUser; ; //현재 유저가 존재한다면
            if (signed == false && User != null) //현재 유저가 디폴트 값이고 등록된 유저가 없다면
                Debug.Log("Log Out");
            else if (signed == true && User == null) //현재 유저가 디펄트 값은 아니지만 등록된 유저가 없다면
                Debug.Log("Sign IN");
            else if (signed == true && User != null)//현재 유저가 디펄트값이 아니고 유저도 등록되어 있다면
                Debug.Log("Log IN");
            
        }
    }

    public void SignIN(string email, string password)
    {
        if (IsFirebaseReady == false || IsSignInOnProgress == true || User != null)
        {
            return;
        }
        IsSignInOnProgress = true;
        OffAction?.Invoke();// 로그인 동안 관련 ui 비활성화
        firebaseAuth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if(task.IsCanceled)
            {
                Debug.LogError("SignIn Failed");
                IsSignInOnProgress = false;
                OnAction?.Invoke();
                return;
            }
            if(task.IsFaulted)
            {
                Debug.LogError("SignIN Cancled");
                IsSignInOnProgress = false;
                OnAction?.Invoke();
                return;
            }

            FirebaseUser newUser = task.Result.User;
            IsSignInOnProgress = false;
            Debug.Log("SignIN Success");
            OnAction?.Invoke();
        });
    }

    public void LogIn(string email, string password)
    {
        if(IsFirebaseReady == false || IsSignInOnProgress == true || User != null)
        {
            return;
        }
        IsSignInOnProgress = true;
        OffAction?.Invoke();// 로그인 동안 관련 ui 비활성화
        firebaseAuth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread((task) =>
        {
            Debug.Log($"sign in status : {task.Status} [{email},{password}] ");

            if (task.IsFaulted)
            {
                Debug.LogError($"{task.Exception}");
                IsSignInOnProgress = false;
                OnAction?.Invoke();
            }
            else if (task.IsCanceled)
            {
                Debug.LogError("LogIN Cancled");
                IsSignInOnProgress = false;
                OnAction?.Invoke();
            }
            else
            {
                User = firebaseAuth.CurrentUser;
                Debug.Log(User.Email);
                Managers.Scene.LoadScene(Define.Scene.Lobby);
            }
        });

    }

    public void LogOut()
    {
        firebaseAuth.SignOut();
        User = null;
    }

    public void Clear()
    {
        OnAction = null;
        OffAction = null;
    }
}
