using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemBoxSlot : UI_Slot
{
    public UI_ItemBox itemBoxUI = null;

    protected override void Init()
    {
        base.Init();
        itemBoxUI = gameObject.FindParent<UI_ItemBox>();
    }

    protected override void OnEndDrag(PointerEventData evt)
    {
        base.OnEndDrag(evt);
    }

    protected override void OnClick(PointerEventData evt)
    {
        if (itemBoxUI.targetInven == null)
        {
            Debug.Log("ContactInven null : UI_ItemBox OnClick()");
            return;
        }
        //�ش� ������ ��Ŭ�� �ߴٸ�
        if (PointerEventData.InputButton.Right == evt.button)
        {
            itemBoxUI.targetInven.AddItemToSlot(this);
        }
    }
}
