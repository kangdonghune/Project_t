using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//player ������ Ÿ�� ���� ����
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviourPun, IDamageable
{
    private Transform _trans = null;
    private NavMeshAgent _agent = null;
    private CharacterController _ctrl = null;
    private Animator _ani = null;
    //�ִϸ��̼� �Ķ����
    private int hashAttackTrigger = Animator.StringToHash("IsAttack");


    [HideInInspector]
    public Define.State State = Define.State.Idle;
    protected Define.State _nextState = Define.State.None;

    public bool IsMasterClientLocal => PhotonNetwork.IsMasterClient && photonView.IsMine;

    public Transform Target { get; set; } = null;

    //�ӽ� ����
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

        //���� ������ Ŭ���̾�Ʈ��� �������� ���� ���
        if(IsMasterClientLocal == true)
        {
            if (Managers.Scene.CurrentScene.SpawnDefault() == false)
                Debug.LogWarning($"default Spawn Failed {Managers.Scene.CurrentScene}");
        }

        if(photonView.IsMine == false) //�� Ŭ���̾�Ʈ �÷��̾ �ƴ϶��
        {
            gameObject.layer = (int)Define.Layer.Player;
            //����Ʈ �÷��̾��� �̵�,ȸ���� ������ �����ϴ� �̰� Ȱ��ȭ�Ǿ������� ������ �߻�
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
        //�� ������Ʈ�� �� Ŭ���̾�Ʈ�� ���� �� �ƴϸ�
        if (photonView.IsMine == false)
            return;

        float dist = (_agent.destination - _trans.position).magnitude;
        if (dist <= 0.5f)
        {
            Stop();
            return;
        }
        Vector3 lookPos = _agent.destination - _trans.position;
        lookPos.y = _trans.position.y; //x �� ���� ȸ���Ͽ� �𵨸��� �Ʒ��� ���� ���� �� ����
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
        // ĳ���Ͱ� ���ߴ� �������� �׺� �޽��� ���ӵ��� �۵� ���θ� ���� ������� �׺�޽��� ��߳��� ���� �������� �ʴ´�.
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        _agent.transform.position = _ctrl.transform.position; // Ȥ�ö� ��߳� ��� ������ �� ������
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
            lookPos.y = _trans.position.y; //x �� ���� ȸ���Ͽ� �𵨸��� �Ʒ��� ���� ���� �� ����
            _trans.LookAt(lookPos);
            return;
        }
        Move();
    }
    void Attack()
    {
        _ani.SetTrigger(hashAttackTrigger);
        _nextState = Define.State.Chase;
        State = Define.State.Ready; // ������ ���� ������ �ٽ� Attack ȣ�� ���ϵ��� ��� ���·�
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
            _agent.SetDestination(Target.position); //0.3�� ������ ������ �缳��
    }

    void OnFire()
    {
        GameObject go = Managers.Resource.PunInstantiate("Bullet", firePos.position, firePos.rotation);
        Bullet bullet = go.GetOrAddComponent<Bullet>();
        bullet.Master = gameObject;
        bullet.SetTarget(Target);
    }
}
