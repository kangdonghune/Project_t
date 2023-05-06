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

    //�κ��丮�� ����â �� ������ ui�� �ƴ����� Ű �Է¿� ���� Ȱ��ȭ, ��Ȱ��ȭ�� �ʿ��� ��� ���
    public void CanvasEnableChange<T>(bool isAble,int index = 0) where T : UI_Scene
    {
        Canvas canvas = _sceneDic[typeof(T)][index].GetComponent<Canvas>();
        if(canvas == null)
        {
            Debug.LogError($"CanvasDisable Failed ({typeof(T)})");
            return;
        }
        //���� ���¿� �ݴ�� �ϰԲ� �Ͽ� ������ �� �������� ������ �� ��������
        canvas.enabled = !isAble;
    }

    public T CreateSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name; // �̸� null���̸� Ÿ�԰����� �̸� ����
        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Utill.GetOrAddComponent<T>(go); //���� UI ���� ���۳�Ʈ�� ���ٸ� �߰�

        if (_sceneDic.ContainsKey(typeof(T)) == false)
            _sceneDic.Add(typeof(T), new List<UI_Scene>());
        _sceneDic[typeof(T)].Add(sceneUI); 

        go.transform.SetParent(Root.transform);

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
}
