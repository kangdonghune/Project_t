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
        _trans = transform;
    }

    public void SetTarget(GameObject go) { _target = go.transform; }

    //�÷��̾� �̵��� ī�޶� �̵��� ������ �� ƽ���� �ٲ� �� �ֱ⿡ ����Ʈ ������Ʈ���� ī�޶� ��ġ �̵�
    private void LateUpdate()
    {
        if (_target == null)
            return;
        _trans.position = _target.position + Dir;
    }
}
