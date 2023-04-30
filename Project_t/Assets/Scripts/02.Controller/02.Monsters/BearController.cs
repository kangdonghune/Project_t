using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonsterController
{


    //애니메이션 파라미터
    private int hashAttackTrigger = Animator.StringToHash("Attack");

    void Start()
    {
        Init();
        StartCoroutine("CoCheckState");
    }
    protected override void Init()
    {
        //컴퍼넌트 초기화
        _ani = GetComponent<Animator>();
        _trans = transform;
   
        //변수 초기화
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
            yield return new WaitForSeconds(_updateTime); // 0.3초 간격으로 상태체크(매 업데이트 순간 체크하는 건 비효율적인 부분)
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
        //추격 범위를 벗어난 상태라면
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
        StopAllCoroutines(); //제거 전 모든 코루틴 제거
    }
}
