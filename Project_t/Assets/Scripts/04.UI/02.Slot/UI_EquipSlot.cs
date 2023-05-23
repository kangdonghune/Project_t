using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EquipSlot : UI_Slot
{

    protected override void Init()
    {
        base.Init();
        slotType = Define.ItemType.Equipable;
    }
    public override bool CheckType(Item item) 
    {
        if (item.ID == 0) // 교환하고자 하는 대상이 빈 슬롯이면 교환 가능(내 장비칸을 비운다 개념)
            return true;
        return slotType == item.Type; 
    }

}
