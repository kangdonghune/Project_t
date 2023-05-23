using Firebase.Extensions;
using Firebase.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface ILoader<Key,Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    private Dictionary<string, Action> _bindDict = new Dictionary<string, Action>();

    public Dictionary<int, Item> ItemDict { get; private set; } = new Dictionary<int, Item>();

    public void Init()
    {
        Binding();
        foreach(var pair in _bindDict)
        {
            OnDownloadFile(pair.Key, pair.Value);
        }


    }

    private void Binding()
    {
        _bindDict.Add("Item/ItemData.json", () => { ItemDict = LoadJson<ItemData, int, Item>("Item/ItemData.json").MakeDict(); });
    }

    public void OnDownloadFile(string path, Action action)
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        StorageReference storage_ref = storage.GetReferenceFromUrl("gs://unity-project-t-b0134.appspot.com/");
        StorageReference isStorage_ref = storage_ref.Child($"/{path}");
        string local_url = $"{Application.persistentDataPath}/{path}";
        //해당 경로에서 폴더만 자르기
        int folderIdx = local_url.LastIndexOf("/");
        string local_folder = local_url.Substring(0, folderIdx);
        if(Directory.Exists($"{local_folder}") == false)
        {
            //저장할 폴더 생성
            Directory.CreateDirectory($"{local_folder}");
        }
        Debug.Log(string.Format("{0}", local_url));
        isStorage_ref.GetFileAsync(local_url).ContinueWithOnMainThread(file_task =>
        {
            //다운 중 실패하거나 중단되지 않았다면
            if (!file_task.IsFaulted && !file_task.IsCanceled)
            {
                Debug.Log($"OnDownload Success : /{path}");
                action.Invoke();
            }
            else
            {
                Debug.LogWarning($"OnDownload Failed : {path}");
                Debug.LogWarning(file_task.Exception.ToString());
            }
        }
        );
    }

    public void OnUpdateFile(string path)
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        StorageReference storage_ref = storage.GetReferenceFromUrl("gs://unity-project-t-b0134.appspot.com/");
        string loacl_url = Application.persistentDataPath + $"/{path}";
        StorageReference update_ref = storage_ref.Child($"/{path}");
        update_ref.PutFileAsync(loacl_url).ContinueWithOnMainThread((file_task) =>
        {
            //다운 중 실패하거나 중단되지 않았다면
            if (!file_task.IsFaulted && !file_task.IsCanceled)
            {
                Debug.Log($"Update Success : /{path}");
            }
            else
            {
                Debug.LogWarning($"Update Failed : /{path}");
                Debug.LogWarning(file_task.Exception.ToString());
            }
        }
        );
    }




    Loader LoadJson<Loader, Key,Value>(string path) where Loader : ILoader<Key,Value>
    {
        string text = Managers.Resource.LoadJsonByFirebase<TextAsset>(path);
        return JsonUtility.FromJson<Loader>(text);
    }
}
