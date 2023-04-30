using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        //TODO
        //���� ������Ʈ Ǯ�� �����Ͽ� �߰�

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefab/{path}");
        //�ε� ���� ��
        if (original == null)
            return null;
        //TODO
        //���� ������Ʈ Ǯ�� �����Ͽ� �߰�
        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;

        return go;

    }
    public void Destroy(GameObject go, float time = 0.0f)
    {
        if (go == null)
            return;
        //TODO
        //���� ������Ʈ Ǯ�� �����Ͽ� �߰�

        Object.Destroy(go, time);
    }
}