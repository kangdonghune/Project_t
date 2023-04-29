using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_Instatnce = null; // 싱글턴 유일성 보장
    //다른 매니저가 총괄 매지저보다 먼저 호출될 경우 매니저가 중복 생성될 위험이 있다. 그래서 별도의 Init() 함수로 관리
    static Managers Instance { get { Init(); return s_Instatnce; } } 

    private InputManager m_Input = new InputManager();
    public static InputManager Input { get { return Instance?.m_Input; } }
    private ResourceManager m_Resource = new ResourceManager();
    public static ResourceManager Resource { get { return Instance.m_Resource; } }


    void Start()
    {
        Init(); 
    }

    private void Update()
    {
        m_Input.OnUpdate();
    }

    private static void Init()
    {
        if(s_Instatnce == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if(go == null) //하이라키 창에 매니저 오브젝트가 없다면
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);//임의 삭제 방지
            s_Instatnce = go.GetComponent<Managers>(); //이후 다른 매니저가 매니저 접근할 때는 자동으로 하이라키창에 생성한 매니저로 통괄 접근
        }
    }
}
