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
    //���̾� ���̽��� ������ �� �ִ� �� ���ο� ���� ��
    private bool IsFirebaseReady { get; set; } = false;
    //�ߺ� ������ �����ϱ� ���� ���̾� ���̽��� ���� ������ ���ο� ���� ��
    private bool IsSignInOnProgress { get; set; } = false;

    //���̾� ���̽� ��ü ���ø����̼��� �����ϴ� ����
    public FirebaseApp firebaseApp { get; private set; }
    //���̾� ���̽� �߿����� Auth ���� ����� �����ϴ� ����
    public FirebaseAuth firebaseAuth { get; private set; }
    //���̾�̽� Auth�� ���ؼ� ������ ���� ������ �Ҵ��ϴ� ����
    public FirebaseUser User { get; private set; }

    //���Ӱ� ���õ� ui on off�� �ش� ui ��ũ��Ʈ���� �ϰ� ���⼱ �̺�Ʈ�� ����
    public Action OnAction;
    public Action OffAction;

    public void Init()
    {
        //OffAction?.Invoke(); //���̾� ���̽� ���� �� ���� ���� ui�� ��Ȱ��ȭ
        //async �Լ��̱⿡ �ݹ��� �ɾ� ������ �ؾ��Ѵ�.
        //continuewith() �Լ��� pc ȯ�濡�� ������ ���� ��Ȥ �� ���۷����� �߻��ϴ� ��찡 �־� onmainthread �Լ��� ��ü
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var result = task.Result;
            if (result != DependencyStatus.Available) //���̾� ���̽��� ��� �Ұ� ���¶��
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
            bool signed = firebaseAuth.CurrentUser != FirebaseAuth.DefaultInstance.CurrentUser; ; //���� ������ �����Ѵٸ�
            if (signed == false && User != null) //���� ������ ����Ʈ ���̰� ��ϵ� ������ ���ٸ�
                Debug.Log("Log Out");
            else if (signed == true && User == null) //���� ������ ����Ʈ ���� �ƴ����� ��ϵ� ������ ���ٸ�
                Debug.Log("Sign IN");
            else if (signed == true && User != null)//���� ������ ����Ʈ���� �ƴϰ� ������ ��ϵǾ� �ִٸ�
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
        OffAction?.Invoke();// �α��� ���� ���� ui ��Ȱ��ȭ
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
        OffAction?.Invoke();// �α��� ���� ���� ui ��Ȱ��ȭ
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
