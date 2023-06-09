using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemBox : UI_InvenBase
{

    enum GameObjects
    {
        InvenBackGround,
    }

    public UI_InvenBase targetInven = null;
    public ItemBox ItemBox { get; set; }

    protected override void Init()
    {
        base.Init();
        _gridCount = (int)Define.GridValue.ItemBox;
        _constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        SlotList = new List<UI_Slot>();
        _slotCount = 8;
        Bind<GameObject>(typeof(GameObjects));
        GridLayoutGroup grid = Get<GameObject>((int)GameObjects.InvenBackGround).GetOrAddComponent<GridLayoutGroup>();
        grid.constraintCount = _gridCount;
        grid.constraint = _constraint;

        //임의로 넣어놨던 아이템들은 전부 제거
        foreach (Transform child in grid.transform)
            Managers.Resource.Destroy(child.gameObject);
        for (int i = 0; i < _slotCount; i++)
        {
            SlotList.Add(Managers.Resource.Instantiate("UI/Scene/Slot/ItemBoxSlot", grid.transform).GetOrAddComponent<UI_ItemBoxSlot>());
        }
        //해당 창 꺼버리기
        Managers.UI.CanvasEnableChange<UI_ItemBox>(true);
    }

    public override void SlotsLoad()
    {
        Item item = Managers.Data.ItemDict[2];
        AddItemToSlot(item);
    }

    public override void InvenChanged()
    {
        base.InvenChanged();
        ItemBox.UpdateItemBox();
    }

    public void SetTargetInven(UI_InvenBase inven)
    {
        targetInven = inven;
    }
}
