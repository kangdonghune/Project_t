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

        //임의로 넣어놨던 아이템들은 전부 제거
        foreach (Transform child in grid.transform)
            Managers.Resource.Destroy(child.gameObject);
        for (int i = 0; i < _slotCount; i++)
        {
            _slotList.Add(Managers.Resource.Instantiate("UI/Scene/Slot/ItemSlot", grid.transform).GetOrAddComponent<UI_ItemSlot>());
            _slotList[i].slotNum = i;
        }
        //슬롯을 만들어도 바인딩은 다음 틱에서 실행되니 인벤 로드는 코루틴을 이용해 한 틱 쉰 다음에 실행시켜야 바인딩 널레퍼런스 문제가 안 생긴다.
        Managers.Corutine.CallWaitForOneFrame(()=> SlotLoad());
        Managers.UI.CanvasEnableChange<UI_Inventory>(true);
    }

}
