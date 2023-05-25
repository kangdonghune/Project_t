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

    //상태체크 코루틴 호출 시간 
    protected float _updateTime = 0.3f;

    protected Vector3 _spawnPos;
    protected Transform _target = null;
    protected Transform _oldTarget = null;

    protected int _targetMask = (1 << (int)Define.Layer.Player) | (1 << (int)Define.Layer.MyPlayer);
    protected float _moveSpeed = 0f; // 이동 속도
    protected float _turnSpeed = 0f; // 회전 속도
    [SerializeField]
    protected float _attackRange = 0f; //공격 사정 거리
    [SerializeField]
    protected float _chaseDist = 0f; // 최대 추격 범위
    protected float _patience = 0f; // 최대 추격 범위를 벗어난 순간부터 감소하는 값. 0이 되면 추격을 종료하고 복귀
    protected float _sleepCount = 0f; // idle 상태에서 sleep 상태로 넘어가기 까지 남은 시간

    void Awake()
    {
        _ani = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updatePosition = false; //이동과 회전은 캐릭터 컨트롤러로 관리
        _agent.updateRotation = false;
        _trans = transform;
        _ctrl = GetComponent<CharacterController>();
        Init();
    }

    protected abstract void Init();

    protected virtual void Move()
    {
        Vector3 lookPos = _agent.destination - _trans.position;
  
        lookPos.y = _trans.position.y; //x 축 기준 회전하여 모델링이 아래로 기울어 지는 거 방지
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
        // 캐릭터가 멈추는 시점에서 네비 메쉬의 가속도와 작동 여부를 같이 멈춰줘야 네비메쉬가 어긋나서 따로 움직이지 않는다.
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
        //최대 추격 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_spawnPos, _chaseDist);

        //공격 사정 거리
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
