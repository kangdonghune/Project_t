using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerInput : MonoBehaviourPun
{
    private Camera _cam = null;

    private Transform _trans = null;
    private NavMeshAgent _agent = null;
    private Animator _ani = null;
    private PlayerController _playerCtrl = null;
    private UI_Inventory _inven = null;

    //마우스 이벤트 관련 마스크
    private int _mask = (1 << (int)Define.Layer.Floor) | (1 << (int)Define.Layer.ItemBox);

    void Init()
    {
        _cam = Camera.main;
        _trans = GetComponent<Transform>();
        _agent = GetComponent<NavMeshAgent>();
        //이동과 회전은 캐릭터 컨트롤러가 조절
        _agent.updatePosition = false;
        _agent.updateRotation = false;
        _ani = GetComponent<Animator>();
        _playerCtrl = GetComponent<PlayerController>();


    }

    private void Start()
    {
        Init();
        if (photonView.IsMine == true)
        {
            //인풋 매니저에 인풋 관련 함수 전달 - 중복 추가 방지를 위해 추가 전 빼는 작업 먼저.
            Managers.Input.KeyAction -= KeyEvent;
            Managers.Input.KeyAction += KeyEvent;
            Managers.Input.MouseAction -= MouseEvent;
            Managers.Input.MouseAction += MouseEvent;
            _cam.gameObject.GetOrAddComponent<CameraController>().SetTarget(gameObject);
        }
    }

    //마우스 입력은 이동
    private void MouseEvent(Define.MouseEvent evt)
    {
        switch (evt)
        {
            case Define.MouseEvent.RClick:
                MousePicking();
                break;
        }
    }

    private void MousePicking()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits; //마우스 클릭 위치에 여러 오브젝트가 있을 경우 가정(ex: 땅, 몬스터, 아이템, 건물)
        hits = Physics.SphereCastAll(_cam.transform.position, 0.5f, ray.direction, 100.0f, _mask);
        bool isStop = false;
        foreach (RaycastHit hit in hits)
        {
            switch(hit.transform.gameObject.layer)
            {
                case (int)Define.Layer.ItemBox:
                    _agent.SetDestination(hit.transform.position);
                    isStop = true;
                    break;
                case (int)Define.Layer.Floor:
                    RaycastHit FloorHit;
                    if (Physics.Raycast(ray, out FloorHit, 100.0f))
                    {
                        _ani.SetBool("IsMove", true);
                        _agent.isStopped = false;
                        _agent.SetDestination(FloorHit.point);
                        _playerCtrl.State = Define.State.Move;
                    }
                    break;
            }

            if (isStop == true)
                break;
        }
    }

    //키 입력은 스킬, ui 등등 담당
    private void KeyEvent(Define.KeyEvent evt)
    {
        switch (evt)
        {
            case Define.KeyEvent.Down:
                KeyDown();
                break;
        }
    }
    //키 입력에 따른 입력 처리

    private void KeyDown()
    {
        if(Input.GetKeyDown(KeyCode.I) == true)
        {
            InvenEnableChange();
        }
    }

    public void InvenEnableChange()
    {
        Canvas canvas = _inven.GetComponent<Canvas>();
        Managers.UI.CanvasEnableChange<UI_Inventory>(canvas.enabled);

    }
}
