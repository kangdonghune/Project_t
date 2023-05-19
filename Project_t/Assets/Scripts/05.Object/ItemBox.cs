using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviourPun
{
    //���� Ŭ���̾�Ʈ�� ���� ���� ȣ��Ƽ���� ����
    //������ ������Ʈ�� ���� ��ǻ�Ϳ��� ������ ���� ������Ʈ���� ����
    //=> Room�� ȣ��Ʈ�� ������ ������Ʈ �� ���޸� true�� ��ȯ�Ѵ�.
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
        _canvas = _itemBoxUI.GetComponent<Canvas>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _canvas.enabled = true;
        other.GetComponent<PlayerController>().Stop();
        //UI_InvenBase inven = Managers.UI.GetSceneUI<UI_Inventory>();
        //_itemBoxUI.SetTargetInven(inven);
        //Managers.UI.CanvasEnableChange<UI_Inventory>(false, inven);
    }

    private void OnTriggerExit(Collider other)
    {
        _canvas.enabled = false;
        //UI_InvenBase inven = Managers.UI.GetSceneUI<UI_Inventory>();
        //Managers.UI.CanvasEnableChange<UI_Inventory>(true, inven);
    }

    public void UpdateItemBox()
    {
        if(photonView.IsMine == true)
        {
            for(int idx = 0; idx < _itemBoxUI.SlotList.Count; idx++)
            {
                Item item = _itemBoxUI.SlotList[idx].Item;
                if(item == null)
                    photonView.RPC("RPC_BroadcastNullItem", RpcTarget.AllBuffered, idx);
                else
                    photonView.RPC("RPC_BroadcastItem", RpcTarget.AllBuffered, idx, item.Name, item.Type, item.Duplicate, item.Count);
            }
            photonView.RPC("RPC_BroadcastSlotUpdate", RpcTarget.AllBuffered);

        }
    }

    [PunRPC]
    private void RPC_BroadcastNullItem(int idx)
    {
        _itemBoxUI.SlotList[idx].Item = null;
    }

    [PunRPC]
    private void RPC_BroadcastItem(int idx, string name, Define.ItemType type, bool duplicate, int count)
    {
        Item item = new Item(name, type, duplicate, count);
        _itemBoxUI.SlotList[idx].Item = item;
    }

    [PunRPC]
    private void RPC_BroadcastSlotUpdate()
    {
        _itemBoxUI.UpdateSlots();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _triggerDist);
    }


}
