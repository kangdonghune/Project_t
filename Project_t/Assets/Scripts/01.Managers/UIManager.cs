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
                {
                    root = new GameObject { name = "@UI_Root" };
                    root.AddComponent<UI_Root>();
                }

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

    public void ChangeOrder(GameObject go, int order)
    {
        Canvas canvas = Utill.GetOrAddComponent<Canvas>(go);
        canvas.sortingOrder = order;
    }

    //인벤토리나 스텟창 등 제거할 ui는 아니지만 키 입력에 따라 활성화, 비활성화가 필요한 경우 사용
    //해당 ui가 여러 개 존재하는 경우에는 2인자로 키고 끄고자 하는 캔버스 지정(이럴 일이 있을까 싶긴하다.)
    public void CanvasEnableChange<T>(bool isAble, UI_Scene UI = null) where T : UI_Base
    {
        Canvas canvas;
        if (UI == null)
            canvas = _sceneDic[typeof(T)][0].GetComponent<Canvas>();
        else
            canvas = _sceneDic[typeof(T)].Find(sceneUI => sceneUI = UI).GetComponent<Canvas>(); 
        if(canvas == null)
        {
            Debug.LogError($"CanvasDisable Failed ({typeof(T)})");
            return;
        }
        //이전 상태와 반대로 하게끔 하여 열렸을 땐 닫히도록 닫혔을 땐 열리도록
        canvas.enabled = !isAble;
    }

    public T GetSceneUI<T>() where T : UI_Scene
    {
        return _sceneDic[typeof(T)][0] as T;
    }

    //월드 ui의 경우 나만 보는 게 아닐 가능성이 존재한다.
    public T CreateWorldUI<T>(Transform parent, string name = null) where T : UI_BasePun
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name; // 이름 null값이면 타입값으로 이름 설정
        //플레이어를 따라 이동하는데 플레이어가 pun에서 관리해줄 경우도 산하에 부속된 월드UI가 따라다니는 지 체크
        GameObject go;
        go = Managers.Resource.PunInstantiate($"UI/World/{name}", Vector3.zero, Quaternion.identity);

        Canvas canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
        go.transform.SetParent(parent);
        return Utill.GetOrAddComponent<T>(go);
    }

    public T CreateSceneUI<T>(string name = null, Transform parent = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name; // 이름 null값이면 타입값으로 이름 설정
        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Utill.GetOrAddComponent<T>(go); //만약 UI 관련 컴퍼넌트가 없다면 추가

        if (_sceneDic.ContainsKey(typeof(T)) == false)
            _sceneDic.Add(typeof(T), new List<UI_Scene>());
        _sceneDic[typeof(T)].Add(sceneUI);

        if (parent == null)
            go.transform.SetParent(Root.transform);
        else
            go.transform.SetParent(parent);
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

    public void Clear()
    {
        CloseAllPopupUI();
        //foreach (var pair in _sceneDic)
        //{
        //    foreach (UI_Scene scene in pair.Value)
        //        Managers.Resource.Destroy(scene.gameObject);
        //}
        _sceneDic.Clear();
    }
}
