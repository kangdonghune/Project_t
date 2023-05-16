using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.UnKnown;

    void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        Object evt = GameObject.FindObjectOfType(typeof(EventSystem));
        if (evt == null)
        {
            Debug.LogWarning("not exist @EventSystem. Create @EventSystem");
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
        }
    }

    public abstract void Clear();


    void OnEnable()
    {
        Clear();
    }
}
