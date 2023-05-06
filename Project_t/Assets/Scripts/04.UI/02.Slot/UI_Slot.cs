using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Slot : UI_Base
{
    enum Images
    {
        ItemImage
    }

    enum Texts
    {
        ItemCount
    }


    public Item Item { get; set; }
    protected Define.ItemType slotType = Define.ItemType.None;

    protected override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<TMP_Text>(typeof(Texts));
        Item = null;
    }


    //슬롯에 아이템 추가 후 슬롯 갱신
    public virtual bool CheckType(Item item) { return true; }
    public virtual bool AddItem(Item item) 
    {
        if (CheckType(item) == false)
            return false;
        //슬롯이 비어있지 않고 슬롯의 아이템이 중복 소유가 가능하다면
        if (Item != null && Item.Duplicate == true)
            Item.AddCount(item.Count);
        else
            Item = item;
        UpdateSlot();
        return true;
    }
    public void RemoveItem() { Item = null; UpdateSlot(); }

    public void ChangeSlot<T>(T slot) where T : UI_Slot
    {
        Item myItem = Item;
        //현재 슬롯에 교환하고자 하는 슬롯 아이템을 추가할 수 없거나 교환하고자 하는 슬롯에 내 아이템을 넣을 수 없다면
        if (CheckType(slot.Item) == true || slot.CheckType(myItem) == true)
        {
            //바꾸기 전으로 전환
            AddItem(slot.Item);
            slot.AddItem(myItem);
        }
    }

    public void UpdateSlot()
    {
        if (Item == null)
        {
            Get<Image>((int)Images.ItemImage).sprite = null;
            Get<TMP_Text>((int)Texts.ItemCount).text = "";
            return;
        }
        Get<Image>((int)Images.ItemImage).sprite =  Managers.Resource.Load<Sprite>($"02.Images/ItemIcon/{Item.Name}");
        Get<TMP_Text>((int)Texts.ItemCount).text = $"{Item.Count}";
    }
}
