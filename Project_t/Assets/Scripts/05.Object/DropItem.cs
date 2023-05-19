using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviourPun
{
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
        _canvas = _itemBoxUI.GetComponent<Canvas>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _canvas.enabled = true;
        other.GetComponent<PlayerController>().Stop();
        UI_InvenBase inven = Managers.UI.GetSceneUI<UI_Inventory>();
        _itemBoxUI.SetTargetInven(inven);
        Managers.UI.CanvasEnableChange<UI_Inventory>(false, inven);
    }

    private void OnTriggerExit(Collider other)
    {
        _canvas.enabled = false;
        UI_InvenBase inven = Managers.UI.GetSceneUI<UI_Inventory>();
        Managers.UI.CanvasEnableChange<UI_Inventory>(true, inven);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _triggerDist);
    }

}
