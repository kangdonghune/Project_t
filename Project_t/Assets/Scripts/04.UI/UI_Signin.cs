using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Signin : UI_Scene
{
    enum Buttons
    {
        SignInButton,
        LogInButton,
    }

    enum Texts
    {
        InputEmailText,
        InputPasswordText,
    }

    protected override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));
        Button SiinInButton = Get<Button>((int)Buttons.SignInButton);
        SiinInButton.onClick.AddListener(SignIN); //���� �� ��ư�� �̺�Ʈ �߰�
        Button LogInButton = Get<Button>((int)Buttons.LogInButton);
        LogInButton.onClick.AddListener(LogIN); //���� �� ��ư�� �̺�Ʈ �߰�

        //���� ������ ���õ� ui�� �̺�Ʈ�� on Off ����
        Managers.Auth.OnAction -= OnUI;
        Managers.Auth.OnAction += OnUI;
        Managers.Auth.OffAction -= OffUI;
        Managers.Auth.OffAction += OffUI;
    }


    private void OnUI()
    {
        for(int idx = 0; idx <= (int)Buttons.LogInButton; idx++)
        {
            GetButton(idx).interactable = true;
        }
    }

    private void OffUI()
    {
        for (int idx = 0; idx <= (int)Buttons.LogInButton; idx++)
        {
            GetButton(idx).interactable = false;
        }
    }

    private void SignIN()
    {
        string email = Get<TMP_Text>((int)Texts.InputEmailText).text;
        string password = Get<TMP_Text>((int)Texts.InputPasswordText).text;
        Managers.Auth.SignIN(email, password);
    }

    private void LogIN()
    {
        string email = Get<TMP_Text>((int)Texts.InputEmailText).text;
        string password = Get<TMP_Text>((int)Texts.InputPasswordText).text;
        Managers.Auth.LogIn(email, password);
    }
}
