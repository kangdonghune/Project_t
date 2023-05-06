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


    //���Կ� ������ �߰� �� ���� ����
    public virtual bool CheckType(Item item) { return true; }
    public virtual bool AddItem(Item item) 
    {
        if (CheckType(item) == false)
            return false;
        //������ ������� �ʰ� ������ �������� �ߺ� ������ �����ϴٸ�
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
        //���� ���Կ� ��ȯ�ϰ��� �ϴ� ���� �������� �߰��� �� ���ų� ��ȯ�ϰ��� �ϴ� ���Կ� �� �������� ���� �� ���ٸ�
        if (CheckType(slot.Item) == true || slot.CheckType(myItem) == true)
        {
            //�ٲٱ� ������ ��ȯ
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
