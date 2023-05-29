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

    //���콺 �̺�Ʈ ���� ����ũ
    private int _mask = (1 << (int)Define.Layer.Floor) | (1 << (int)Define.Layer.ItemBox) |
                        (1 << (int)Define.Layer.Monster) | (1 << (int)Define.Layer.Player);

    void Init()
    {
        _cam = Camera.main;
        _trans = GetComponent<Transform>();
        _agent = GetComponent<NavMeshAgent>();
        //�̵��� ȸ���� ĳ���� ��Ʈ�ѷ��� ����
        _agent.updatePosition = false;
        _agent.updateRotation = false;
        _ani = GetComponent<Animator>();
        _playerCtrl = GetComponent<PlayerController>();
        if(photonView.IsMine == true)
        {
            _inven = Managers.UI.GetSceneUI<UI_Inventory>();
        }


    }

    private void Start()
    {
        Init();
        if (photonView.IsMine == true)
        {
            //��ǲ �Ŵ����� ��ǲ ���� �Լ� ���� - �ߺ� �߰� ������ ���� �߰� �� ���� �۾� ����.
            Managers.Input.KeyAction -= KeyEvent;
            Managers.Input.KeyAction += KeyEvent;
            Managers.Input.MouseAction -= MouseEvent;
            Managers.Input.MouseAction += MouseEvent;
            _cam.gameObject.GetOrAddComponent<CameraController>().SetTarget(gameObject);
        }
    }

    //���콺 �Է��� �̵�
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
        RaycastHit[] hits; //���콺 Ŭ�� ��ġ�� ���� ������Ʈ�� ���� ��� ����(ex: ��, ����, ������, �ǹ�)
        hits = Physics.SphereCastAll(_cam.transform.position, 0.5f, ray.direction, 100.0f, _mask);
        bool isStop = false;
        foreach (RaycastHit hit in hits)
        {
            switch(hit.transform.gameObject.layer)
            {
                case (int)Define.Layer.Monster:
                case (int)Define.Layer.Player:
                    //���� �÷��̾��� Ÿ�ٰ� ��ŷ�� �÷��̾ �ٸ��ٸ�
                    if(_playerCtrl.Target != hit.transform)
                    {
                        if(_playerCtrl.State != Define.State.Ready)
                        {
                            _playerCtrl.Target = hit.transform;
                            _ani.SetBool("IsMove", true);
                            _agent.isStopped = false;
                            _agent.SetDestination(hit.transform.position);
                            _playerCtrl.State = Define.State.Chase;
                        }
                    }
                    isStop = true;
                    break;
                case (int)Define.Layer.ItemBox:
                    _playerCtrl.Target = null;
                    _ani.SetBool("IsMove", true);
                    _agent.isStopped = false;
                    _agent.SetDestination(hit.transform.position);
                    _playerCtrl.State = Define.State.Move;
                    isStop = true;
                    break;
                case (int)Define.Layer.Floor:
                    RaycastHit FloorHit;
                    if (Physics.Raycast(ray, out FloorHit, 100.0f))
                    {
                        _playerCtrl.Target = null;
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

    //Ű �Է��� ��ų, ui ��� ���
    private void KeyEvent(Define.KeyEvent evt)
    {
        switch (evt)
        {
            case Define.KeyEvent.Down:
                KeyDown();
                break;
        }
    }
    //Ű �Է¿� ���� �Է� ó��

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
