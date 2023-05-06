using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private int _order = 10;
    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    Dictionary<Type, List<UI_Scene>> _sceneDic = new Dictionary<Type, List<UI_Scene>>();


    public GameObject Root
    {
        get
        {

            GameObject root = GameObject.Find("@UI_Root");
            {
                if (root == null)
                    root = new GameObject { name = "@UI_Root" };
            }
            return root;
        }
    }

    //기존 ui와 우선순위 결정
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Utill.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true; //부모 캔버스의 소팅오더 여부와 별개로 자기만의 소팅오더를 가진다는 의미

        if(sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    //인벤토리나 스텟창 등 제거할 ui는 아니지만 키 입력에 따라 활성화, 비활성화가 필요한 경우 사용
    public void CanvasEnableChange<T>(bool isAble,int index = 0) where T : UI_Scene
    {
        Canvas canvas = _sceneDic[typeof(T)][index].GetComponent<Canvas>();
        if(canvas == null)
        {
            Debug.LogError($"CanvasDisable Failed ({typeof(T)})");
            return;
        }
        //이전 상태와 반대로 하게끔 하여 열렸을 땐 닫히도록 닫혔을 땐 열리도록
        canvas.enabled = !isAble;
    }

    public T CreateSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name; // 이름 null값이면 타입값으로 이름 설정
        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Utill.GetOrAddComponent<T>(go); //만약 UI 관련 컴퍼넌트가 없다면 추가

        if (_sceneDic.ContainsKey(typeof(T)) == false)
            _sceneDic.Add(typeof(T), new List<UI_Scene>());
        _sceneDic[typeof(T)].Add(sceneUI); 

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }

    public T CreatePopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name; // 이름 null값이면 타입값으로 이름 설정
        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Utill.GetOrAddComponent<T>(go); //만약 UI 관련 컴퍼넌트가 없다면 추가

        _popupStack.Push(popup);
        _order++;

        go.transform.SetParent(Root.transform);

        return popup;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return; // 스텍이 비어있다면
        if (_popupStack.Peek() != popup)
        {
            Debug.LogWarning($"Close Popup Falied : {popup}");
            return;
        }
        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return; // 스텍이 비어있다면

        UI_Popup popup =  _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;
        _order--;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }
}
