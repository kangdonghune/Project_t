using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        //TODO
        //추후 오브젝트 풀링 관련하여 추가

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"01.Prefabs/{path}");
        //로드 실패 시
        if (original == null)
            return null;
        //TODO
        //추후 오브젝트 풀링 관련하여 추가
        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;

        return go;

    }

    public T Copy<T>(T origin, Transform parent = null) where T : Object
    {
        if (origin == null)
            return null;
        T copy = Object.Instantiate(origin, parent);
        copy.name = $"{origin.name}(Copy)";
        return copy;
    }

    public void Destroy(GameObject go, float time = 0.0f)
    {
        if (go == null)
            return;
        //TODO
        //추후 오브젝트 풀링 관련하여 추가

        Object.Destroy(go, time);
    }
}
