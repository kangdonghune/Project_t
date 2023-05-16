using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    static GameManager s_Instatnce = null; // 싱글턴 유일성 보장
    public static GameManager Instance { get { Init(); return s_Instatnce; } }

    public List<GameObject> spawnList = new List<GameObject>();
    private void Start()
    {
        Init();
    }


    private static void Init()
    {
        if (s_Instatnce == null)
        {
            GameObject go = GameObject.Find("@GameManager");
            if (go == null) //하이라키 창에 매니저 오브젝트가 없다면
            {
                Debug.LogWarning("@GameManager is not Exist");
                return;
            }
            s_Instatnce = go.GetComponent<GameManager>();
        }
    }

    public void SpawnPlayer(int index)
    {

        Managers.Resource.PunInstantiate("Player", spawnList[index].transform.position, spawnList[index].transform.rotation);

    }

    //몬스터나 아이템 상자 등은 유저마다 생성을 해주면 문제가 생길 수 있다.
    //따라서 마스터만 생성하도록 제한
    public void SpawnMonster(string Name)
    {
        if (PhotonNetwork.IsMasterClient == false)
            return;
        Managers.Resource.PunInstantiate("DogKnight");
    }

    //go = 아이템 박스 프리팹을 붙여줄 오브젝트
    public void SpawnItemBox(GameObject go)
    {
        if (PhotonNetwork.IsMasterClient == false)
            return;
    
    }

    //나 자신이 방을 나가는 경우에만 호출 : PhotonNetwork.LeaveRoom();
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        //나만 방을 나가야지 포톤으로 로드 씬을 하면 다 같이 나가져 버릴 수 있다.
        Managers.Scene.LoadScene(Define.Scene.Lobby);
    }

}
