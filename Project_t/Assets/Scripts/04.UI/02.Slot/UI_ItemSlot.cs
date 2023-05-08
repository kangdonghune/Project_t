using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : UI_Slot
{
    protected override void OnClick(PointerEventData evt)
    {
        if (PointerEventData.InputButton.Right == evt.button)
        {
            switch (Item.Type)
            {
                case Define.ItemType.Consumable:
                    Consume(evt);
                    break;
                case Define.ItemType.Equipable:
                    Equip(evt);
                    break;
                case Define.ItemType.Projectable:
                    Use(evt);
                    break;

            }
        }
    }

    private void Consume(PointerEventData evt)
    {

        AddCount(-1);
        Debug.Log("소비 아이템 사용");

    }

    private void Equip(PointerEventData evt)
    {
        Debug.Log("장비창과 아이템창 교체");
    }



    private void Use(PointerEventData evt)
    {
        AddCount(-1);
        Debug.Log("투척 아이템 사용");
    }
}
