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
    static LobbyManager s_Instatnce = null; // 싱글턴 유일성 보장
    public static LobbyManager Instance { get { Init(); return s_Instatnce; } }


    private readonly string gameVersion = "1";
    private UI_Lobby _lobbyUI;

    //포톤서버와 연동되는 ui 관련 활성화 비활성화 관리
    public Action OnAction;
    public Action OffAction;

    private Button connectButton;
    private TMP_Text infoText;

    public void Start()
    {
        Init();
        //마스터 클라이언트와 클라이언트들의 씬 동기화 여부
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = Managers.Auth.User.Email;
        PhotonNetwork.GameVersion = gameVersion;
        //설정정보를 담아 접속. 버젼 외에도 부가정보도 가능
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
            if (go == null) //하이라키 창에 매니저 오브젝트가 없다면
            {
                Debug.LogWarning("@LobbyManager is not Exist");
                return;
            }
            s_Instatnce = go.GetComponent<LobbyManager>(); 
        }
    }

    //마스터로 연결 성공 시
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        OnAction?.Invoke();
        infoText.text = "Online: Connected to Master Server";
    }
    //연결이 끊어졌을 때
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        OffAction?.Invoke();
        infoText.text = $"Offline: Connected Disabled {cause.ToString()} -Try reconnecting...";

        //재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        //중복접속 시도 방지
        OffAction.Invoke();

        if(PhotonNetwork.IsConnected == true) //연결 시도하는 중에 연결이 끊긴 경우를 예방
        {
            infoText.text = "Connecting to Random Room...";
            PhotonNetwork.JoinRandomRoom(); //참가할 수 있는 무작위 방 참가
        }
        else
        {
            infoText.text = $"Offline: Connected Disabled - Try reconnecting...";
            PhotonNetwork.ConnectUsingSettings();
        }

    }

    //호스트만 게임 시작 가능
    public void EnterGame()
    {
        PhotonNetwork.LoadLevel(Utill.GetSceneName(Define.Scene.Game));
    }
    //호스트 이외에는 대기로 전환
    public void Ready()
    {
        //클릭 시 다시 방에 입장 전으로
        PhotonNetwork.LeaveRoom();
        Utill.ChangeButtonEvent(connectButton, "Join", Connect);
    }

    //빈 방이 없는 경우 또는 그 외의 경우
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        _lobbyUI.GetText((int)UI_Lobby.Texts.InfoText).text = "There is no empty Room. Create new Room.";
        //RoomOptions.CleanupCacheOnLeave 클라이언트가 룸을 떠날 때 관련 오브젝트를 삭제할 지 여부
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });//방 이름으로 방 입장 조건을 걸 수도 있고 룸 옵션에서 여러 제한사항을 걸 수 있다.
    }   

    //방 접속에 성공함
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        _lobbyUI.GetText((int)UI_Lobby.Texts.InfoText).text = "Connected with Room";
        //씬 매니저로 이동하면 로컬 유저의 씬만 넘어간다. 따라서 동기화가 안 되니 사용X
        //호스트가 호출하면 방 참가 인원이 다 같은 씬으로 이동한다. 동기화도 같이 된다.
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
