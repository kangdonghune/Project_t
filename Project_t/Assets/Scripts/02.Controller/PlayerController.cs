using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//player 라이프 타임 관련 관리
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    private Transform _trans = null;
    private NavMeshAgent _agent = null;
    private CharacterController _ctrl = null;
    private Animator _ani = null;
    [HideInInspector]
    public Define.State State = Define.State.Idle;

    //임시 스텟
    public float speed = 5.0f;
    public float turnSpeed = 10.0f;
    private Vector3 _desVelocity;

    void Start()
    {
        _trans = transform;
        _agent = GetComponent<NavMeshAgent>();
        _ctrl = GetComponent<CharacterController>();
        _ani = GetComponent<Animator>();

    }

    void Update()
    {
        switch(State)
        {
            case Define.State.Move:
                Move();
                break;
        }
    }

    void Move()
    {
        float dist = (_agent.destination - _trans.position).magnitude;
        if (dist <= 0.5f)
        {
            State = Define.State.Idle;
            _ani.SetBool("IsMove", false);
            // 캐릭터가 멈추는 시점에서 네비 메쉬의 가속도와 작동 여부를 같이 멈춰줘야 네비메쉬가 어긋나서 따로 움직이지 않는다.
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
            return;
        }
        Vector3 lookPos = _agent.destination - _trans.position;
        lookPos.y = _trans.position.y; //x 축 기준 회전하여 모델링이 아래로 기울어 지는 거 방지
        _trans.rotation = Quaternion.Slerp(_trans.rotation, Quaternion.LookRotation(lookPos), speed * Time.deltaTime);
        _ctrl.Move(_agent.desiredVelocity.normalized * speed * Time.deltaTime);
        _agent.velocity = _ctrl.velocity;
    }
}
