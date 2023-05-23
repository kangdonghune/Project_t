using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Slot : UI_Base
{
    protected enum Images
    {
        ItemImage
    }

    protected enum Texts
    {
        ItemCount
    }

    public Item Item { get; set; }
    protected Define.ItemType slotType = Define.ItemType.None;
    public int slotNum = 0;
    public UI_Popup DragImageUI;
    public GameObject DragImage;
    protected UI_Root _UIRoot;
    private UI_InvenBase inven;
    protected override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<TMP_Text>(typeof(Texts));
        Item = Managers.Data.ItemDict[0];// 디펄트값은 0번 아이템으로
        inven = gameObject.FindParent<UI_InvenBase>();
        _UIRoot = transform.root.gameObject.GetComponent<UI_Root>();
        BindUIEvent();
        UpdateSlot();
    }


    //슬롯에 아이템 추가 후 슬롯 갱신
    public virtual bool CheckType(Item item) { return true; }
    public virtual bool InsertItem(Item item) 
    {
        //슬롯에 들어갈 수 없는 아이템이라면 제외
        if (CheckType(item) == false)
            return false;
        //현재 슬롯의 아이템 또는 삽입하고자 하는 아이템이 비어있다면
        if(Item.ID == 0 || item.ID == 0)
        {
            Item = item;
        }
        //삽입하고자 하는 아이템이 같은 종류라면
        else if (Item == item)
        {
            //현재 아이템이 중복 소유가 된다면
            if (Item.Duplicate == true)
                Item.AddCount(item.Count); //개수만 추가
        }
        //삽입하고자 하는 아이템이 다른 종류라면
        else
            Item = item; //슬롯의 아이템을 해당 아이템으로 변경
        UpdateSlot();
        inven.InvenChanged();
        return true;
    }

    public void AddCount(int count) 
    {
        Item.AddCount(count);
        if (Item.Count <= 0) RemoveItem();
        else UpdateSlot();
        inven.InvenChanged();
    }
    public void RemoveItem() { Item = Managers.Data.ItemDict[0]; UpdateSlot();
        inven.InvenChanged();
    }

    public void ChangeSlot<T>(T slot) where T : UI_Slot
    {
        Item myItem = Item;
        //양쪽 슬롯 모두 아이템을 교환하는데 문제가 없다면 교체
        if (CheckType(slot.Item) == true || slot.CheckType(myItem) == true)
        {
            InsertItem(slot.Item);
            slot.InsertItem(myItem);
        }
        inven.InvenChanged();
    }

    //이걸 할 때 인벤토리까지 전달을 해줘야 인벤토리가 포톤으로 전파를 해줄 수 있다.
    public void UpdateSlot()
    {
        if (Item.ID == 0)
        {
            Get<Image>((int)Images.ItemImage).sprite = null;
            Get<Image>((int)Images.ItemImage).color = new Color(1, 1, 1, 0);
            Get<TMP_Text>((int)Texts.ItemCount).text = "";
            return;
        }
        Get<Image>((int)Images.ItemImage).sprite =  Managers.Resource.Load<Sprite>($"02.Images/ItemIcon/{Item.Name}");
        Get<Image>((int)Images.ItemImage).color = new Color(1, 1, 1, 1);
        Get<TMP_Text>((int)Texts.ItemCount).text = $"{Item.Count}";
    }

    protected virtual void BindUIEvent()
    {
        gameObject.AddUIEvent(OnEnter, Define.UIEvent.Enter);
        gameObject.AddUIEvent(OnExit, Define.UIEvent.Exit);
        gameObject.AddUIEvent(OnClick, Define.UIEvent.Click);
        gameObject.AddUIEvent(OnBeginDrag, Define.UIEvent.BeginDrag);
        gameObject.AddUIEvent(OnDrag, Define.UIEvent.Drag);
        gameObject.AddUIEvent(OnEndDrag, Define.UIEvent.EndDrag);
    }

    protected virtual void OnEnter(PointerEventData evt)
    {
        _UIRoot.FocusSlot = this;
    }
    protected virtual void OnExit(PointerEventData evt)
    {
        _UIRoot.FocusSlot = null;
    }
    protected virtual void OnClick(PointerEventData evt)
    {

    }

    protected virtual void OnBeginDrag(PointerEventData evt)
    {
        DragImageUI = Managers.UI.CreatePopupUI<UI_Popup>("UI_DragImage");
        GameObject DragSlot = DragImageUI.gameObject.FindChild("ItemSlot", true);
        DragImage = Managers.Resource.Copy<GameObject>(Get<Image>((int)Images.ItemImage).gameObject, DragSlot.transform);
        DragImage.GetComponent<Image>().raycastTarget = false;
        Get<Image>((int)Images.ItemImage).enabled = false;
        Get<TMP_Text>((int)Texts.ItemCount).enabled = false;
    }
    protected virtual void OnDrag(PointerEventData evt)
    {
        DragImage.transform.position = Input.mousePosition;
    }
    protected virtual void OnEndDrag(PointerEventData evt)
    {
        Managers.UI.ClosePopupUI(DragImageUI);
        Get<Image>((int)Images.ItemImage).enabled = true;
        Get<TMP_Text>((int)Texts.ItemCount).enabled = true;
        if (_UIRoot.FocusSlot != null)
        {
            ChangeSlot<UI_Slot>(_UIRoot.FocusSlot);
        }
        else //현재 포커스 된 슬롯이 없다면 인벤토리 밖으로 나가있다는 의미
        {
            RemoveItem(); //현재 슬롯의 아이템을 제거
        }
    }

}
