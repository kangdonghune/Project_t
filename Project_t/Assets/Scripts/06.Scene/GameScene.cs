using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Game; 
        Managers.UI.CreateSceneUI<UI_Inventory>();
        GameManager.Instance.SpawnPlayer(PhotonNetwork.LocalPlayer.ActorNumber - 1);
    }

 
    public override void Clear()
    {
        Managers.Clear();
    }

}
