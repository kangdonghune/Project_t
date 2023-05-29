using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviourPun
{
    //현재 클라이언트가 방을 만든 호스티인지 여부
    //생성된 오브젝트가 현재 컴퓨터에서 생성된 로컬 오브젝트인지 여부
    //=> Room의 호스트가 생성된 오브젝트 인 경우메만 true를 반환한다.
    public bool IsMasterClientLocal => PhotonNetwork.IsMasterClient && photonView.IsMine;

    [SerializeField]
    private float _triggerDist = 1.5f;
    private SphereCollider _collider;
    private UI_ItemBox _itemBoxUI;
    private Canvas _canvas;

    private void Init()
    {
        _collider = gameObject.GetComponent<SphereCollider>();
        _collider.radius = _triggerDist;
        _collider.isTrigger = true;
    }

    private void Start()
    {
        Init();
        _itemBoxUI = Managers.UI.CreateSceneUI<UI_ItemBox>();
        _itemBoxUI.ItemBox = this;
        Managers.Corutine.CallWaitForOneFrame(() => _itemBoxUI.SlotsLoad());
        _canvas = _itemBoxUI.GetComponent<Canvas>();
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerController>().Stop();
        if(other.GetComponent<PlayerController>().photonView.IsMine == true)
        {
            _canvas.enabled = true;
        }
        //UI_InvenBase inven = Managers.UI.GetSceneUI<UI_Inventory>();
        //_itemBoxUI.SetTargetInven(inven);
        //Managers.UI.CanvasEnableChange<UI_Inventory>(false, inven);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>().photonView.IsMine == true)
        {
            _canvas.enabled = false;
        }
        //UI_InvenBase inven = Managers.UI.GetSceneUI<UI_Inventory>();
        //Managers.UI.CanvasEnableChange<UI_Inventory>(true, inven);
    }

    public void UpdateItemBox()
    {
        if(photonView.IsMine == true)
        {
            int[] IDs = new int[_itemBoxUI.SlotList.Count];
            for (int idx = 0; idx < _itemBoxUI.SlotList.Count; idx++)
                IDs[idx] = _itemBoxUI.SlotList[idx].Item.ID;
            photonView.RPC("RPC_BroadcastItem", RpcTarget.OthersBuffered, IDs);
        }
    }

    [PunRPC]
    private void RPC_BroadcastItem(int[] IDs)
    {
        for(int idx = 0; idx < IDs.Length; idx++)
            _itemBoxUI.SlotList[idx].Item = Managers.Data.ItemDict[IDs[idx]];
        _itemBoxUI.UpdateSlots();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _triggerDist);
    }


}
