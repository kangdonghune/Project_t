using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : UI_Scene
{
    public enum Buttons
    {
        JoinButton,
    }

    public enum Texts
    {
        UserText,
        InfoText,
    }

    protected override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));

        GetText((int)Texts.UserText).text = Managers.Auth.User.Email;
        LobbyManager.Instance.OnAction -= OnUI;
        LobbyManager.Instance.OnAction += OnUI;
        LobbyManager.Instance.OffAction -= OffUI;
        LobbyManager.Instance.OffAction += OffUI;

    }

 

    private void OnUI()
    {
        for (int idx = 0; idx <= (int)Buttons.JoinButton; idx++)
        {
            GetButton(idx).interactable = true;
        }
    }

    private void OffUI()
    {
        for (int idx = 0; idx <= (int)Buttons.JoinButton; idx++)
        {
            GetButton(idx).interactable = true;
        }
    }
   
}
