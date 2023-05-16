using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEX 
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>();}}

    public void LoadScene(Define.Scene type)
    {
        SceneManager.LoadScene(Utill.GetSceneName(type));
    }

    
}
