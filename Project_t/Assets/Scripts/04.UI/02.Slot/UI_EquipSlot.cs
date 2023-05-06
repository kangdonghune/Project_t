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
        if (item == null) // ��ȯ�ϰ��� �ϴ� ����� �� �����̸� ��ȯ ����(�� ���ĭ�� ���� ����)
            return true;
        return slotType == item.Type; 
    }

}
