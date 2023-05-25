using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public abstract class MonsterController : MonoBehaviourPun, IDamageable
{

    protected Define.MonState _state = Define.MonState.Sleep;
    protected Define.MonState _nextState = Define.MonState.None;
    protected Animator _ani;
    protected NavMeshAgent _agent;
    protected Transform _trans = null;
    protected CharacterController _ctrl;

    //����üũ �ڷ�ƾ ȣ�� �ð� 
    protected float _updateTime = 0.3f;

    protected Vector3 _spawnPos;
    protected Transform _target = null;
    protected Transform _oldTarget = null;

    protected int _targetMask = (1 << (int)Define.Layer.Player) | (1 << (int)Define.Layer.MyPlayer);
    protected float _moveSpeed = 0f; // �̵� �ӵ�
    protected float _turnSpeed = 0f; // ȸ�� �ӵ�
    [SerializeField]
    protected float _attackRange = 0f; //���� ���� �Ÿ�
    [SerializeField]
    protected float _chaseDist = 0f; // �ִ� �߰� ����
    protected float _patience = 0f; // �ִ� �߰� ������ ��� �������� �����ϴ� ��. 0�� �Ǹ� �߰��� �����ϰ� ����
    protected float _sleepCount = 0f; // idle ���¿��� sleep ���·� �Ѿ�� ���� ���� �ð�

    void Awake()
    {
        _ani = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updatePosition = false; //�̵��� ȸ���� ĳ���� ��Ʈ�ѷ��� ����
        _agent.updateRotation = false;
        _trans = transform;
        _ctrl = GetComponent<CharacterController>();
        Init();
    }

    protected abstract void Init();

    protected virtual void Move()
    {
        Vector3 lookPos = _agent.destination - _trans.position;
  
        lookPos.y = _trans.position.y; //x �� ���� ȸ���Ͽ� �𵨸��� �Ʒ��� ���� ���� �� ����
        if (lookPos == Vector3.zero)
        {
            Stop();
            return;
        }
        _trans.rotation = Quaternion.Slerp(_trans.rotation, Quaternion.LookRotation(lookPos), _moveSpeed * Time.deltaTime);
        _ctrl.Move(_agent.desiredVelocity.normalized * _moveSpeed * Time.deltaTime);
        _agent.velocity = _ctrl.velocity;
    }

    public void Stop()
    {
        if (photonView.IsMine == false)
            return;
        _state = Define.MonState.Idle;
        // ĳ���Ͱ� ���ߴ� �������� �׺� �޽��� ���ӵ��� �۵� ���θ� ���� ������� �׺�޽��� ��߳��� ���� �������� �ʴ´�.
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
    }

    #region InterfaceFunc
    public void OnDamged(GameObject attacker)
    {
        _target = attacker.transform;
    }

    #endregion


    private void OnDrawGizmos()
    {
        //�ִ� �߰� ����
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_spawnPos, _chaseDist);

        //���� ���� �Ÿ�
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
