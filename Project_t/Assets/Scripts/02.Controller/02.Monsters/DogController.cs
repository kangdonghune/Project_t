using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DogController : MonsterController
{ 
    //애니메이션 파라미터
    private int hashAttackTrigger = Animator.StringToHash("Attack");

    private GameObject player;

    void Start()
    {

        //상태 체크는 생성한 측에서 한정적으로 관리
        if (photonView.IsMine == true)
        {
            StartCoroutine("CoCheckState");
        }
    }

    protected override void Init()
    {
        //변수 초기화
        _spawnPos = transform.position;
        _moveSpeed = 10f;
        _attackRange = 2f;
        _patience = 2f;
        _sleepCount = 0f;
        _chaseDist = 10f;

        if (photonView.IsMine == false) //내 클라이언트 플레이어가 아니라면
        {
            //리모트 플레이어의 이동,회전은 포톤이 관리하니 이게 활성화되어있으면 문제가 발생
            _agent.enabled = false;
            _ctrl.enabled = false;
        }
    }
    
    void Update()
    {
        //마찬가지로 위치,애니메이션 관련 동작들은 마스터에서 photonview가 배포
        if (photonView.IsMine == false)
            return;

        CheckState();
    }

    private void CheckState()
    {
        ReturnCount();
        switch (_state)
        {
            case Define.MonState.Sleep:
                break;
            case Define.MonState.Idle:
                break;
            case Define.MonState.Chase:
                Chase();
                break;
            case Define.MonState.Attack:
                break;
            case Define.MonState.Return:
                Return();
                break;
        }
    }


    private void ScanTarget()
    {
        //이미 타겟이 지정이 되어 있거나 복귀 상태라면 생략 
        if (_target != null || _state == Define.MonState.Return)
            return;
        Collider[] colliders = Physics.OverlapSphere(_spawnPos, _chaseDist, _targetMask);

        float targetDist = float.MaxValue;
        GameObject target = null;
        foreach (Collider col in colliders)
        {
            float thisDist = (col.transform.position - transform.position).magnitude;
            if(thisDist < targetDist)
            {
                targetDist = thisDist;
                target = col.gameObject;
            }
        }
        if(target != null)
            OnDamged(target);
    }


    #region stateFunc
    IEnumerator CoCheckState()
    {
        while (true)
        {
            switch (_state)
            {
                case Define.MonState.Sleep:
                    ScanTarget();
                    C_Sleep();
                    break;
                case Define.MonState.Idle:
                    ScanTarget();
                    C_Idle();
                    break;
                case Define.MonState.Chase:
                    C_Chase();
                    break;
                case Define.MonState.Attack:
                    C_Attack();
                    break;
                case Define.MonState.Return:
                    C_Return();
                    break;

            }
            yield return new WaitForSeconds(_updateTime); // 0.3초 간격으로 상태체크(매 업데이트 순간 체크하는 건 비효율적인 부분)
        }
    }

    private void C_Sleep()
    {
        if (_target != null)
        {
            _ani.SetBool("isRage", true);
            _state = Define.MonState.Idle;
        }
    }

    private void C_Idle()
    {
        if (_target == null)
        {
            _sleepCount += _updateTime;
            if (_sleepCount > 2f)
            {
                _ani.SetBool("isRage", false);
                _state = Define.MonState.Sleep;
                _target = null;
            }
            return;
        }

        float targetDist = (_target.position - _trans.position).magnitude;
        if (targetDist < _chaseDist)
        {
            _state = Define.MonState.Chase;
            _ani.SetBool("isChase", true);
        }
    }
    private void C_Chase()
    {
        if(_target != null)
        {
            _agent.isStopped = false;
            _agent.SetDestination(_target.position); //0.3초 단위로 목적지 재설정
        }
    }
    private void C_Attack()
    {
        //TODO 공격 관련하여 주기적으로 해줄 것이 생기면 추가할 것
    }
    private void C_Return()
    {
        _ani.SetBool("isChase", false);
        _ani.SetBool("isReturn", true);
        _agent.isStopped = false;
        _agent.SetDestination(_spawnPos); //초기 위치로 귀환
    }

    #endregion

    private void ReturnCount()
    {
        float spawnDist = (_spawnPos - _trans.position).magnitude;
        //추격 범위를 벗어난 상태라면
        if (_target == null || spawnDist > _chaseDist)
        {
            _patience -= Time.deltaTime;
            if (_patience < 0f)
            {
                _state = Define.MonState.Return;
                _nextState = Define.MonState.Return;
                _patience = 2f; //인내심 수치 초기화
                return;
            }
        }
    }

    private void Chase()
    {
        if(_target == null)
        {
            _state = Define.MonState.Return;
            _nextState = Define.MonState.Return;
            _patience = 2f; //인내심 수치 초기화
            return;
        }
        float targetDist = (_target.position - _trans.position).magnitude;
        if (targetDist < _attackRange)
        {
            _ani.SetTrigger(hashAttackTrigger);
            _state = Define.MonState.Attack;
            _nextState = Define.MonState.Chase;
            Vector3 lookPos = _target.position;
            lookPos.y = _trans.position.y; //x 축 기준 회전하여 모델링이 아래로 기울어 지는 거 방지
            _trans.LookAt(lookPos);
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
            return;
        }
        Move();
    }

    private void Return()
    {
        float spawnDist = (_spawnPos - _trans.position).magnitude;
        if (spawnDist < 0.1f)
        {
            _ani.SetBool("isReturn", false);
            _state = Define.MonState.Idle;
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
            _target = null;
            return;
        }
        Move();
    }
    public void AnimationEnd()
    {
        if (photonView.IsMine == false)
            return;

        if (_nextState == Define.MonState.None)
        {
            Debug.LogError($"{gameObject.name}: NextState is None {_state}");
            return;
        }
        _state = _nextState;
        _nextState = Define.MonState.None;
    }

    private void OnDestroy()
    {
        if(photonView.IsMine == true)
            StopAllCoroutines(); //제거 전 모든 코루틴 제거
    }
}
