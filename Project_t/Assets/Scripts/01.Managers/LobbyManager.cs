using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    static LobbyManager s_Instatnce = null; // �̱��� ���ϼ� ����
    public static LobbyManager Instance { get { Init(); return s_Instatnce; } }


    private readonly string gameVersion = "1";
    private UI_Lobby _lobbyUI;

    //���漭���� �����Ǵ� ui ���� Ȱ��ȭ ��Ȱ��ȭ ����
    public Action OnAction;
    public Action OffAction;

    private Button connectButton;
    private TMP_Text infoText;

    public void Start()
    {
        Init();
        //������ Ŭ���̾�Ʈ�� Ŭ���̾�Ʈ���� �� ����ȭ ����
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = Managers.Auth.User.Email;
        PhotonNetwork.GameVersion = gameVersion;
        //���������� ��� ����. ���� �ܿ��� �ΰ������� ����
        PhotonNetwork.ConnectUsingSettings();

        _lobbyUI = Managers.UI.CreateSceneUI<UI_Lobby>();
        Managers.Corutine.CallWaitForOneFrame(() =>
        {
            connectButton = _lobbyUI.GetButton((int)(UI_Lobby.Buttons.JoinButton));
            Utill.ChangeButtonEvent(connectButton, "Join", Connect);
            connectButton.interactable = false;
            infoText = _lobbyUI.GetText((int)UI_Lobby.Texts.InfoText);
            infoText.text = "Connecting to Master Server....";
        });

    }

    private static void Init()
    {
        if (s_Instatnce == null)
        {
            GameObject go = GameObject.Find("@LobbyManager");
            if (go == null) //���̶�Ű â�� �Ŵ��� ������Ʈ�� ���ٸ�
            {
                Debug.LogWarning("@LobbyManager is not Exist");
                return;
            }
            s_Instatnce = go.GetComponent<LobbyManager>(); 
        }
    }

    //�����ͷ� ���� ���� ��
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        OnAction?.Invoke();
        infoText.text = "Online: Connected to Master Server";
    }
    //������ �������� ��
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        OffAction?.Invoke();
        infoText.text = $"Offline: Connected Disabled {cause.ToString()} -Try reconnecting...";

        //������ �õ�
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        //�ߺ����� �õ� ����
        OffAction.Invoke();

        if(PhotonNetwork.IsConnected == true) //���� �õ��ϴ� �߿� ������ ���� ��츦 ����
        {
            infoText.text = "Connecting to Random Room...";
            PhotonNetwork.JoinRandomRoom(); //������ �� �ִ� ������ �� ����
        }
        else
        {
            infoText.text = $"Offline: Connected Disabled - Try reconnecting...";
            PhotonNetwork.ConnectUsingSettings();
        }

    }

    //ȣ��Ʈ�� ���� ���� ����
    public void EnterGame()
    {
        PhotonNetwork.LoadLevel(Utill.GetSceneName(Define.Scene.Game));
    }
    //ȣ��Ʈ �̿ܿ��� ���� ��ȯ
    public void Ready()
    {
        //Ŭ�� �� �ٽ� �濡 ���� ������
        PhotonNetwork.LeaveRoom();
        Utill.ChangeButtonEvent(connectButton, "Join", Connect);
    }

    //�� ���� ���� ��� �Ǵ� �� ���� ���
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        _lobbyUI.GetText((int)UI_Lobby.Texts.InfoText).text = "There is no empty Room. Create new Room.";
        //RoomOptions.CleanupCacheOnLeave Ŭ���̾�Ʈ�� ���� ���� �� ���� ������Ʈ�� ������ �� ����
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });//�� �̸����� �� ���� ������ �� ���� �ְ� �� �ɼǿ��� ���� ���ѻ����� �� �� �ִ�.
    }   

    //�� ���ӿ� ������
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        _lobbyUI.GetText((int)UI_Lobby.Texts.InfoText).text = "Connected with Room";
        //�� �Ŵ����� �̵��ϸ� ���� ������ ���� �Ѿ��. ���� ����ȭ�� �� �Ǵ� ���X
        //ȣ��Ʈ�� ȣ���ϸ� �� ���� �ο��� �� ���� ������ �̵��Ѵ�. ����ȭ�� ���� �ȴ�.
        if (PhotonNetwork.IsMasterClient == true)
            Utill.ChangeButtonEvent(connectButton, "Enter", EnterGame);
        else
            Utill.ChangeButtonEvent(connectButton, "Ready", Ready);

    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Create Room Success");
    }

}
