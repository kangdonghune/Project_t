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

    //�÷��̾� �̵��� ī�޶� �̵��� ������ �� ƽ���� �ٲ� �� �ֱ⿡ ����Ʈ ������Ʈ���� ī�޶� ��ġ �̵�
    private void LateUpdate()
    {
        _trans.position = _target.position + Dir;
    }
}
