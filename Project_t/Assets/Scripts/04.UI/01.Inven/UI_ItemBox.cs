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

    protected override void Init()
    {
        base.Init();
        _gridCount = (int)Define.GridValue.ItemBox;
        _constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _slotList = new List<UI_Slot>();
        _slotCount = 8;
        Bind<GameObject>(typeof(GameObjects));
        GridLayoutGroup grid = Get<GameObject>((int)GameObjects.InvenBackGround).GetOrAddComponent<GridLayoutGroup>();
        grid.constraintCount = _gridCount;
        grid.constraint = _constraint;

        //���Ƿ� �־���� �����۵��� ���� ����
        foreach (Transform child in grid.transform)
            Managers.Resource.Destroy(child.gameObject);
        for (int i = 0; i < _slotCount; i++)
        {
            _slotList.Add(Managers.Resource.Instantiate("UI/Scene/Slot/ItemBoxSlot", grid.transform).GetOrAddComponent<UI_ItemBoxSlot>());
        }
        //������ ���� ���ε��� ���� ƽ���� ����Ǵ� �κ� �ε�� �ڷ�ƾ�� �̿��� �� ƽ �� ������ ������Ѿ� ���ε� �η��۷��� ������ �� �����.
        Managers.Corutine.CallWaitForOneFrame(() => SlotLoad());
        //�ش� â ��������
        Managers.UI.CanvasEnableChange<UI_ItemBox>(true);
    }

    protected override void SlotLoad()
    {
        Item item = new Item("Apple", Define.ItemType.Consumable, true, 10);
        AddItemToSlot(item);
    }

    public void SetTargetInven(UI_InvenBase inven)
    {
        targetInven = inven;
    }
}
