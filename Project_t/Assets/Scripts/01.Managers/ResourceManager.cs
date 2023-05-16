using Photon.Pun;
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

    public GameObject PunInstantiateMonster(string name, Vector3? position = null, Quaternion? rotation = null)
    {
        return PunInstantiate($"{name}", position, rotation);
    }

    //단순 오브젝트가 아니라 서버 연동이 된 오브젝트들이라면(서버가 포지션,로테이션 등을 받아서 전파)
    //포톤네트워크의 인스테시에이트를 써야한다.
    public GameObject PunInstantiate(string name, Vector3? position = null , Quaternion? rotation = null)
    {
        GameObject original = Load<GameObject>($"01.Prefabs/{name}");
        //따로 값 설정이 없다면 프리팹의 디펄트 값으로
        if(position == null || rotation == null)
        {
            position = original.transform.position;
            rotation = original.transform.rotation;
        }
        //포톤네트워크로 생성하는 프리팹은 모두 포톤 뷰 컴퍼넌트를 소유하고 있어야 한다.
        GameObject go = PhotonNetwork.Instantiate($"01.Prefabs/{name}", (Vector3)position , (Quaternion)rotation);
        go.name = name + PhotonNetwork.NickName;
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
