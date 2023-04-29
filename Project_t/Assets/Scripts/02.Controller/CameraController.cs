using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _target;
    private Transform _trans;
    public Vector3 Dir = new Vector3(0, 10, -15);
    void Start()
    {
        _target = GameObject.FindWithTag("Player").transform;
        _trans = transform;
    }

    //플레이어 이동과 카메라 이동의 순서가 매 틱마다 바뀔 수 있기에 레이트 업데이트에서 카메라 위치 이동
    private void LateUpdate()
    {
        _trans.position = _target.position + Dir;
    }
}
