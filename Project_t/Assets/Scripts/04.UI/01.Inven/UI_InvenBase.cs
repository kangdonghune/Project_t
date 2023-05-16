using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_InvenBase : UI_Scene
{
    protected int _gridCount = (int)Define.GridValue.None;
    protected GridLayoutGroup.Constraint _constraint = GridLayoutGroup.Constraint.FixedColumnCount;
    protected List<UI_Slot> _slotList;


    [SerializeField]
    protected int _slotCount = 0;

    protected override void Init()
    {
        base.Init();
    }

    protected virtual void SlotLoad()
    {
        Item item = new Item("SilverRing", Define.ItemType.Equipable, false);
        AddItemToSlot(item);
    }



    //아이템 인벤토리는 꽉 차있으면 새로운 아이템을 못 받는다.
    public void AddItemToSlot(UI_Slot slot)
    {
        if (AddItemToSlot(slot.Item))
            slot.RemoveItem();

    }

    //private로 걸어 외부에서 임의로 아이템만 추가하는 걸 방지
    protected bool AddItemToSlot(Item item)
    {
        //아이템이 중첩 가능한 소비템이고 이번 슬롯이 해당 아이템과 같다면
        if (item.Duplicate == true)
        {
            foreach (UI_Slot slot in _slotList)
            {
                if (slot.Item == null) //해당 슬롯이 비어있다면
                    continue;
                if (item.Name == slot.Item.Name) //해당 슬롯이 추가하고자 하는 아이템과 같다면
                {
                    if (slot.InsertItem(item) == true) //해당 슬롯에 아이템이 성공적으로 추가되었다면
                        return true;
                }
            }
        }
        //중복 가능한 아이템이 아니거나 중복 가능한 아이템을 소지한 경우가 아닌 경우
        foreach (UI_Slot slot in _slotList)
        {
            if (slot.Item == null) //해당 슬롯이 비어있고
            {
                if (slot.InsertItem(item) == true) //해당 슬롯에 아이템이 성공적으로 추가되었다면
                    return true;
            }
        }

        return false; //끝내 추가를 못했다면
    }

}
