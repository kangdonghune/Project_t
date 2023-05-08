using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Debug.LogWarning("FindParent �Լ��� ���� Gameobject �� null ���Դϴ�.");
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
            Debug.LogWarning("FindChild �Լ��� ���� Gameobject �� null ���Դϴ�.");
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

}