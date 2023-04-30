using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DogController : MonsterController
{

    //�ִϸ��̼� �Ķ����
    private int hashAttackTrigger = Animator.StringToHash("Attack");

    //�ӽ� �÷��̾� ����
    private GameObject _player;

    
    void Start()
    {
        Init();
        StartCoroutine("CoCheckState");
    }

    protected override void Init()
    {
        //���� �ʱ�ȭ
        _spawnPos = transform.position;
        _moveSpeed = 10f;
        _attackRange = 2f;
        _patience = 2f;
        _sleepCount = 0f;
        _chaseDist = 10f;

        _player = GameObject.FindWithTag("Player");
    }
    
    void Update()
    {
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

    #region stateFunc
    IEnumerator CoCheckState()
    {
        while (true)
        {
            switch (_state)
            {
                case Define.MonState.Sleep:
                    C_Sleep();
                    break;
                case Define.MonState.Idle:
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
            yield return new WaitForSeconds(_updateTime); // 0.3�� �������� ����üũ(�� ������Ʈ ���� üũ�ϴ� �� ��ȿ������ �κ�)
        }
    }

    private void C_Sleep()
    {
        if (_target != null)
        {
            _ani.SetBool("isRage", true);
            _state = Define.MonState.Idle;
        }

        //�ӽ÷� ���� �Ÿ� ���� �� ������ �Լ� ȣ��
        float playerDist = (_player.transform.position - _trans.position).magnitude;
        if(playerDist < _chaseDist)
        {
            OnDamged(_player);
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
        _agent.isStopped = false;
        _agent.SetDestination(_target.position); //0.3�� ������ ������ �缳��
    }
    private void C_Attack()
    {
        //TODO ���� �����Ͽ� �ֱ������� ���� ���� ����� �߰��� ��
    }
    private void C_Return()
    {
        _ani.SetBool("isChase", false);
        _ani.SetBool("isReturn", true);
        _agent.isStopped = false;
        _agent.SetDestination(_spawnPos); //�ʱ� ��ġ�� ��ȯ
    }

    #endregion

    private void ReturnCount()
    {
        if (_target == null)
            return;

        float spawnDist = (_spawnPos - _trans.position).magnitude;
        //�߰� ������ ��� ���¶��
        if (spawnDist > _chaseDist)
        {
            _patience -= Time.deltaTime;
            if (_patience < 0f)
            {
                _state = Define.MonState.Return;
                _nextState = Define.MonState.Return;
                _patience = 2f; //�γ��� ��ġ �ʱ�ȭ
                return;
            }
        }
    }

    private void Chase()
    {
        float targetDist = (_target.position - _trans.position).magnitude;
        if (targetDist < _attackRange)
        {
            _ani.SetTrigger(hashAttackTrigger);
            _state = Define.MonState.Attack;
            _nextState = Define.MonState.Chase;
            Vector3 lookPos = _target.position;
            lookPos.y = _trans.position.y; //x �� ���� ȸ���Ͽ� �𵨸��� �Ʒ��� ���� ���� �� ����
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
        }
        Move();
    }
    public void AnimationEnd()
    {
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
        StopAllCoroutines(); //���� �� ��� �ڷ�ƾ ����
    }
}