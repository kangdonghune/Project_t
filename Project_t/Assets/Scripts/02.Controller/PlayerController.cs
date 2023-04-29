using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//player ������ Ÿ�� ���� ����
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    private Transform _trans = null;
    private NavMeshAgent _agent = null;
    private CharacterController _ctrl = null;
    private Animator _ani = null;
    [HideInInspector]
    public Define.State State = Define.State.Idle;

    //�ӽ� ����
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
            // ĳ���Ͱ� ���ߴ� �������� �׺� �޽��� ���ӵ��� �۵� ���θ� ���� ������� �׺�޽��� ��߳��� ���� �������� �ʴ´�.
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
            return;
        }
        Vector3 lookPos = _agent.destination - _trans.position;
        lookPos.y = _trans.position.y; //x �� ���� ȸ���Ͽ� �𵨸��� �Ʒ��� ���� ���� �� ����
        _trans.rotation = Quaternion.Slerp(_trans.rotation, Quaternion.LookRotation(lookPos), speed * Time.deltaTime);
        _ctrl.Move(_agent.desiredVelocity.normalized * speed * Time.deltaTime);
        _agent.velocity = _ctrl.velocity;
    }
}
