using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//player 라이프 타임 관련 관리
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviourPun, IDamageable
{
    private Transform _trans = null;
    private NavMeshAgent _agent = null;
    private CharacterController _ctrl = null;
    private Animator _ani = null;
    //애니메이션 파라미터
    private int hashAttackTrigger = Animator.StringToHash("IsAttack");


    [HideInInspector]
    public Define.State State = Define.State.Idle;
    protected Define.State _nextState = Define.State.None;

    public bool IsMasterClientLocal => PhotonNetwork.IsMasterClient && photonView.IsMine;

    public Transform Target { get; set; } = null;

    //임시 스텟
    public float AttackRange = 2.0f;
    public float Speed = 10.0f;
    public float TurnSpeed = 10.0f;

    [SerializeField]
    private Transform firePos;

    void Start()
    {
        _trans = transform;
        _agent = GetComponent<NavMeshAgent>();
        _ctrl = GetComponent<CharacterController>();
        _ani = GetComponent<Animator>();

        //내가 마스터 클라이언트라면 기초적인 생성 명령
        if(IsMasterClientLocal == true)
        {
            if (Managers.Scene.CurrentScene.SpawnDefault() == false)
                Debug.LogWarning($"default Spawn Failed {Managers.Scene.CurrentScene}");
        }

        if(photonView.IsMine == false) //내 클라이언트 플레이어가 아니라면
        {
            gameObject.layer = (int)Define.Layer.Player;
            //리모트 플레이어의 이동,회전은 포톤이 관리하니 이게 활성화되어있으면 문제가 발생
            _agent.enabled = false;
            _ctrl.enabled = false;
        }
        else
        {
            gameObject.layer = (int)Define.Layer.MyPlayer;
            StartCoroutine(CoUpdate());
            _agent.avoidancePriority = PhotonNetwork.LocalPlayer.ActorNumber;
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
            case Define.State.Chase:
                Chase();
                break;
            case Define.State.Attack:
                Attack();
                break;
            case Define.State.Ready:
                break;
            case Define.State.None:
                break;
        }
    }

    IEnumerator CoUpdate()
    {
        if (photonView.IsMine == false)
            yield break;
        while(true)
        {
            switch (State)
            {
                case Define.State.Chase:
                    C_Chase();
                    break;
            }
            yield return new WaitForSeconds(0.3f);
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
        _trans.rotation = Quaternion.Slerp(_trans.rotation, Quaternion.LookRotation(lookPos), Speed * Time.deltaTime);
        _ctrl.Move(_agent.desiredVelocity.normalized * Speed * Time.deltaTime);
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
        _agent.transform.position = _ctrl.transform.position; // 혹시라도 어긋난 경우 멈췄을 때 재정렬
    }

    void Chase()
    {
        if (Target == null)
        {
            Stop();
            return;
        }
        float targetDist = (_agent.destination - _trans.position).magnitude;
        if (targetDist <= AttackRange)
        {
            Stop();
            State = Define.State.Attack;
            Vector3 lookPos = Target.position;
            lookPos.y = _trans.position.y; //x 축 기준 회전하여 모델링이 아래로 기울어 지는 거 방지
            _trans.LookAt(lookPos);
            return;
        }
        Move();
    }
    void Attack()
    {
        _ani.SetTrigger(hashAttackTrigger);
        _nextState = Define.State.Chase;
        State = Define.State.Ready; // 공격이 끝날 때까진 다시 Attack 호출 안하도록 대기 상태로
    }

    public virtual void AttackExcute()
    {
        if (photonView.IsMine == false)
            return;
        if (State != Define.State.Ready)
            return;
        OnFire();
    }

    public void AnimationEnd()
    {
        if (photonView.IsMine == false)
            return;

        if (_nextState == Define.State.None)
        {
            Debug.LogError($"{gameObject.name}: NextState is None {State}");
            return;
        }
        State = _nextState;
        _nextState = Define.State.None;
    }

    public void OnDamged(GameObject attacker)
    {
        
    }

    void C_Chase()
    {
        if(State == Define.State.Chase)
            _agent.SetDestination(Target.position); //0.3초 단위로 목적지 재설정
    }

    void OnFire()
    {
        GameObject go = Managers.Resource.PunInstantiate("Bullet", firePos.position, firePos.rotation);
        Bullet bullet = go.GetOrAddComponent<Bullet>();
        bullet.Master = gameObject;
        bullet.SetTarget(Target);
    }
}
