using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    static GameManager s_Instatnce = null; // �̱��� ���ϼ� ����
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
            if (go == null) //���̶�Ű â�� �Ŵ��� ������Ʈ�� ���ٸ�
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

    //���ͳ� ������ ���� ���� �������� ������ ���ָ� ������ ���� �� �ִ�.
    //���� �����͸� �����ϵ��� ����
    public void SpawnMonster(string Name)
    {
        if (PhotonNetwork.IsMasterClient == false)
            return;
        Managers.Resource.PunInstantiate("DogKnight");
    }

    //go = ������ �ڽ� �������� �ٿ��� ������Ʈ
    public void SpawnItemBox(GameObject go)
    {
        if (PhotonNetwork.IsMasterClient == false)
            return;
    
    }

    //�� �ڽ��� ���� ������ ��쿡�� ȣ�� : PhotonNetwork.LeaveRoom();
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        //���� ���� �������� �������� �ε� ���� �ϸ� �� ���� ������ ���� �� �ִ�.
        Managers.Scene.LoadScene(Define.Scene.Lobby);
    }

}
