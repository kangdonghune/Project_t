using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inventory : UI_InvenBase
{
    enum GameObjects
    {
        InvenBackGround,
    }

    protected override void Init()
    {
        base.Init();
        _gridCount = (int)Define.GridValue.Inven;
        _constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _slotList = new List<UI_Slot>();
        _slotCount = 20;
        Bind<GameObject>(typeof(GameObjects));
        GridLayoutGroup grid = Get<GameObject>((int)GameObjects.InvenBackGround).GetOrAddComponent<GridLayoutGroup>();
        grid.constraintCount = _gridCount;
        grid.constraint = _constraint;

        //���Ƿ� �־���� �����۵��� ���� ����
        foreach (Transform child in grid.transform)
            Managers.Resource.Destroy(child.gameObject);
        for (int i = 0; i < _slotCount; i++)
        {
            _slotList.Add(Managers.Resource.Instantiate("UI/Scene/Slot/ItemSlot", grid.transform).GetOrAddComponent<UI_ItemSlot>());
            _slotList[i].slotNum = i;
        }
        //������ ���� ���ε��� ���� ƽ���� ����Ǵ� �κ� �ε�� �ڷ�ƾ�� �̿��� �� ƽ �� ������ ������Ѿ� ���ε� �η��۷��� ������ �� �����.
        Managers.Corutine.CallWaitForOneFrame(()=> SlotLoad());
        Managers.UI.CanvasEnableChange<UI_Inventory>(true);
    }

}
