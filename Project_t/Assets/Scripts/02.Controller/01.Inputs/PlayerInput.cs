using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerInput : MonoBehaviour
{
    private Camera _cam = null;

    private Transform _trans = null;
    private NavMeshAgent _agent = null;
    private Animator _ani = null;
    private PlayerController _playerCtrl = null;

    //���콺 �̺�Ʈ ���� ����ũ
    private int _mask = (1 << (int)Define.Layer.Floor);

    void Start()
    {
        _cam = Camera.main;
        _trans = GetComponent<Transform>();
        _agent = GetComponent<NavMeshAgent>();
        //�̵��� ȸ���� ĳ���� ��Ʈ�ѷ��� ����
        _agent.updatePosition = false;
        _agent.updateRotation = false;
        _ani = GetComponent<Animator>();
        _playerCtrl = GetComponent<PlayerController>();


        //��ǲ �Ŵ����� ��ǲ ���� �Լ� ���� - �ߺ� �߰� ������ ���� �߰� �� ���� �۾� ����.
        Managers.Input.KeyAction -= KeyEvent; 
        Managers.Input.KeyAction += KeyEvent;
        Managers.Input.MouseAction -= MouseEvent;
        Managers.Input.MouseAction += MouseEvent;

    }
    //Ű �Է��� ��ų, ui ��� ���
    private void KeyEvent(Define.KeyEvent evt)
    {
        switch(evt)
        {
            case Define.KeyEvent.Down:
                break;
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
        foreach (RaycastHit hit in hits)
        {
            switch(hit.transform.gameObject.layer)
            {
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
        }
    }
}
