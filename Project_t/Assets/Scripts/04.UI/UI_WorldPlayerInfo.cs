using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_WorldPlayerInfo : UI_BasePun
{
    enum Texts
    {
        UserID,
    }

    protected override void Init()
    {
        Bind<TMP_Text>(typeof(Texts));

        if (photonView.IsMine == true)
        {
            GetText((int)Texts.UserID).color = Color.green;
            GetText((int)Texts.UserID).text = PhotonNetwork.NickName;
        }
        else
        {
            GetText((int)Texts.UserID).color = Color.red;
            GetText((int)Texts.UserID).text = photonView.Owner.NickName;
        }
    }

    //punrpc ����
    //[PunRPC]
    //public void SendMyName(string name)
    //{
    //    GetText((int)Texts.UserID).text = name;
    //}

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
