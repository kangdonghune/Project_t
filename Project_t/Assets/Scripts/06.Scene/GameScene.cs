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

    //마스터만 해당 함수를 호출하도록 관리
    public override bool SpawnDefault()
    {
        Managers.Resource.PunInstantiate("DogPolyart");
        //Managers.Resource.PunInstantiate("Item Box");
        return true;
    }
 
    public override void Clear()
    {
        Managers.Clear();
    }

}
