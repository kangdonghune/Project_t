using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Utill 
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
        {
            component = go.AddComponent<T>();
        }
        return component;
    }

    public static T FindParent<T>(GameObject go, string name = null) where T : UnityEngine.Object
    {
        if (go == null)
        {
            Debug.LogWarning("FindParent 함수의 인자 Gameobject 가 null 값입니다.");
            return null;
        }
        T component = go.transform.parent.GetComponent<T>();
        if (component == null)
            return FindParent<T>(go.transform.parent.gameObject, name);
        return component;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
        {
            Debug.LogWarning("FindChild 함수의 인자 Gameobject 가 null 값입니다.");
            return null;
        }

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }


        Debug.LogWarning($"Don't find {name} in { go.name}");
        return null;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform != null)
            return transform.gameObject;

        Debug.LogWarning($"Don't find GameObject {name} in { go.name}");
        return null;
    }


    public static string GetSceneName(Define.Scene type) { return System.Enum.GetName(typeof(Define.Scene), type); }

    public static void ChangeButtonEvent(Button button, string text = null, UnityEngine.Events.UnityAction action = null)
    {
        if (text != null)
            button.gameObject.FindChild<TMP_Text>().text = text;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }

}
