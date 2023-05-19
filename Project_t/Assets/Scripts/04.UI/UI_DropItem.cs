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
            //내가 만든 플레이어의 정보만 위치 조정을 내가하고 나머지는 리모트로 처리
            transform.position = transform.parent.position + Vector3.up * (transform.parent.GetComponent<Collider>().bounds.size.y);
        }
        //카메라 방향을 쳐다보는 건 리모트 여부와 상관없이 모든 월드 ui가 날 쳐다보도록
        transform.rotation = Camera.main.transform.rotation;// ui가 일정한 방향으로 보여지게 처리
    }
}
