using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DropItem : UI_BasePun
{
    enum Images
    {
        ItemImage,
    }

    protected override void Init()
    {
        Bind<Image>(typeof(Images));
    }
    private void Update()
    {
        if (photonView.IsMine == true)
        {
            //���� ���� �÷��̾��� ������ ��ġ ������ �����ϰ� �������� ����Ʈ�� ó��
            transform.position = transform.parent.position + Vector3.up * (transform.parent.GetComponent<Collider>().bounds.size.y);
        }
        //ī�޶� ������ �Ĵٺ��� �� ����Ʈ ���ο� ������� ��� ���� ui�� �� �Ĵٺ�����
        transform.rotation = Camera.main.transform.rotation;// ui�� ������ �������� �������� ó��
    }
}
