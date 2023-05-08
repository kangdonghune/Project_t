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
        Debug.Log("�Һ� ������ ���");

    }

    private void Equip(PointerEventData evt)
    {
        Debug.Log("���â�� ������â ��ü");
    }



    private void Use(PointerEventData evt)
    {
        AddCount(-1);
        Debug.Log("��ô ������ ���");
    }
}
