using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignInScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.SignIn;
        Managers.UI.CreateSceneUI<UI_Signin>(); //���� ui ����
    }

    public override void Clear()
    {
        Managers.Clear();
    }

}
