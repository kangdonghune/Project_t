using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//player 라이프 타임 관련 관리
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviourPun
{
    private Transform _trans = null;
    private NavMeshAgent _agent = null;
    private CharacterController _ctrl = null;
    private Animator _ani = null;
    [HideInInspector]
    public Define.State State = Define.State.Idle;

    public bool IsMasterClientLocal => PhotonNetwork.IsMasterClient && photonView.IsMine;


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

        if(photonView.IsMine == false) //내 클라이언트 플레이어가 아니라면
        {
            //리모트 플레이어의 이동,회전은 포톤이 관리하니 이게 활성화되어있으면 문제가 발생
            _agent.enabled = false;
            _ctrl.enabled = false;
        }
    }

    void Update()
    {
        if (photonView.IsMine == false)
            return;

        switch (State)
        {
            case Define.State.Move:
                Move();
                break;
        }
    }

    void Move()
    {
        //이 오브젝트가 내 클라이언트가 만든 게 아니면
        if (photonView.IsMine == false)
            return;

        float dist = (_agent.destination - _trans.position).magnitude;
        if (dist <= 0.5f)
        {
            Stop();
            return;
        }
        Vector3 lookPos = _agent.destination - _trans.position;
        lookPos.y = _trans.position.y; //x 축 기준 회전하여 모델링이 아래로 기울어 지는 거 방지
        _trans.rotation = Quaternion.Slerp(_trans.rotation, Quaternion.LookRotation(lookPos), speed * Time.deltaTime);
        _ctrl.Move(_agent.desiredVelocity.normalized * speed * Time.deltaTime);
        _agent.velocity = _ctrl.velocity;
   }

  

    public void Stop()
    {
        if (photonView.IsMine == false)
            return;
        State = Define.State.Idle;
        _ani.SetBool("IsMove", false);
        // 캐릭터가 멈추는 시점에서 네비 메쉬의 가속도와 작동 여부를 같이 멈춰줘야 네비메쉬가 어긋나서 따로 움직이지 않는다.
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
    }
}
