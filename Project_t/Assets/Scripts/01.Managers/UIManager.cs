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

    //���� ui�� �켱���� ����
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Utill.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true; //�θ� ĵ������ ���ÿ��� ���ο� ������ �ڱ⸸�� ���ÿ����� �����ٴ� �ǹ�

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

    //�κ��丮�� ����â �� ������ ui�� �ƴ����� Ű �Է¿� ���� Ȱ��ȭ, ��Ȱ��ȭ�� �ʿ��� ��� ���
    //�ش� ui�� ���� �� �����ϴ� ��쿡�� 2���ڷ� Ű�� ������ �ϴ� ĵ���� ����(�̷� ���� ������ �ͱ��ϴ�.)
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
        //���� ���¿� �ݴ�� �ϰԲ� �Ͽ� ������ �� �������� ������ �� ��������
        canvas.enabled = !isAble;
    }

    public T GetSceneUI<T>() where T : UI_Scene
    {
        return _sceneDic[typeof(T)][0] as T;
    }

    //���� ui�� ��� ���� ���� �� �ƴ� ���ɼ��� �����Ѵ�.
    public T CreateWorldUI<T>(Transform parent, string name = null) where T : UI_BasePun
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name; // �̸� null���̸� Ÿ�԰����� �̸� ����
        //�÷��̾ ���� �̵��ϴµ� �÷��̾ pun���� �������� ��쵵 ���Ͽ� �μӵ� ����UI�� ����ٴϴ� �� üũ
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
            name = typeof(T).Name; // �̸� null���̸� Ÿ�԰����� �̸� ����
        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Utill.GetOrAddComponent<T>(go); //���� UI ���� ���۳�Ʈ�� ���ٸ� �߰�

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
            name = typeof(T).Name; // �̸� null���̸� Ÿ�԰����� �̸� ����
        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Utill.GetOrAddComponent<T>(go); //���� UI ���� ���۳�Ʈ�� ���ٸ� �߰�

        _popupStack.Push(popup);
        _order++;

        go.transform.SetParent(Root.transform);

        return popup;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return; // ������ ����ִٸ�
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
            return; // ������ ����ִٸ�

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
