using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_InvenBase : UI_Scene
{
    protected int _gridCount = (int)Define.GridValue.None;
    protected GridLayoutGroup.Constraint _constraint = GridLayoutGroup.Constraint.FixedColumnCount;
    public List<UI_Slot> SlotList { get; protected set; }


    [SerializeField]
    protected int _slotCount = 0;

    protected override void Init()
    {
        base.Init();
    }

    protected virtual void SlotsLoad()
    {
        Item item = Managers.Data.ItemDict[1];
        AddItemToSlot(item);
    }

    public virtual void InvenChanged()
    {

    }

    public virtual void UpdateSlots()
    {
        for(int idx = 0; idx < SlotList.Count; idx++)
        {
            SlotList[idx].UpdateSlot();
        }
    }

    public virtual void UpdateSlots(List<UI_Slot> slots)
    {
        if(SlotList.Count != slots.Count)
        {
            Debug.Log("UpdateSlots �� �Ű������� �ش� ���ӿ�����Ʈ�� �κ��丮�� �ٸ��ϴ�.");
            return;
        }    

        for(int idx = 0; idx < slots.Count; idx++)
        {
            SlotList[idx].Item = slots[idx].Item;
        }
        UpdateSlots();
    }



    //������ �κ��丮�� �� �������� ���ο� �������� �� �޴´�.
    public void AddItemToSlot(UI_Slot slot)
    {
        if (AddItemToSlot(slot.Item))
            slot.RemoveItem();

    }

    //private�� �ɾ� �ܺο��� ���Ƿ� �����۸� �߰��ϴ� �� ����
    protected bool AddItemToSlot(Item item)
    {
        //�������� ��ø ������ �Һ����̰� �̹� ������ �ش� �����۰� ���ٸ�
        if (item.Duplicate == true)
        {
            foreach (UI_Slot slot in SlotList)
            {
                if (slot.Item == null) //�ش� ������ ����ִٸ�
                    continue;
                if (item.Name == slot.Item.Name) //�ش� ������ �߰��ϰ��� �ϴ� �����۰� ���ٸ�
                {
                    if (slot.InsertItem(item) == true) //�ش� ���Կ� �������� ���������� �߰��Ǿ��ٸ�
                        return true;
                }
            }
        }
        //�ߺ� ������ �������� �ƴϰų� �ߺ� ������ �������� ������ ��찡 �ƴ� ���
        foreach (UI_Slot slot in SlotList)
        {
            if (slot.Item == null) //�ش� ������ ����ְ�
            {
                if (slot.InsertItem(item) == true) //�ش� ���Կ� �������� ���������� �߰��Ǿ��ٸ�
                    return true;
            }
        }

        return false; //���� �߰��� ���ߴٸ�
    }

}
