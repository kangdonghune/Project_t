using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonsterController
{


    //�ִϸ��̼� �Ķ����
    private int hashAttackTrigger = Animator.StringToHash("Attack");

    void Start()
    {
        Init();
        StartCoroutine("CoCheckState");
    }
    protected override void Init()
    {
        //���۳�Ʈ �ʱ�ȭ
        _ani = GetComponent<Animator>();
        _trans = transform;
   
        //���� �ʱ�ȭ
        _spawnPos = transform.position;
        _moveSpeed = 10f;
        _attackRange = 2f;
        _patience = 2f;
        _sleepCount = 0f;
        _chaseDist = 10f;
    }

    IEnumerator CoCheckState()
    {
        while (true)
        {
            switch(_state)
            {
                case Define.MonState.Sleep:
                    Sleep();
                    break;
                case Define.MonState.Idle:
                    Idle();
                    break;
                case Define.MonState.Chase:
                    Chase();
                    break;
                case Define.MonState.Attack:
                    Attack();
                    break;
                case Define.MonState.Return:
                    break;

            }
            yield return new WaitForSeconds(_updateTime); // 0.3�� �������� ����üũ(�� ������Ʈ ���� üũ�ϴ� �� ��ȿ������ �κ�)
        }
    }

    private void Sleep()
    {

    }

    private void Idle()
    {
        if (_target == null)
        {
            _sleepCount += _updateTime;
            if (_sleepCount > 2f)
                _state = Define.MonState.Sleep;
            return;
        }

        float targetDist = (_target.position - _trans.position).magnitude;
        if (targetDist < _chaseDist)
        {
            _state = Define.MonState.Chase;
            _ani.SetBool("IsChase", true);
        }
    }
    private void Chase()
    {
        float spawnDist = (_target.position - _trans.position).magnitude;
        //�߰� ������ ��� ���¶��
        if(spawnDist > _chaseDist)
        {
            _patience -= _updateTime;
            if(_patience < 0f)
            {
                _state = Define.MonState.Return;
                _ani.SetBool("IsChase", false);
                return;
            }
        }
        _agent.SetDestination(_target.position);
    }
    private void Attack()
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        _ani.SetTrigger(hashAttackTrigger);
    }
    private void Return()
    {

    }

    public void AttackEnd()
    {
        _agent.isStopped = false;
        _state = Define.MonState.Chase;
    }

    private void OnDestroy()
    {
        StopAllCoroutines(); //���� �� ��� �ڷ�ƾ ����
    }
}
