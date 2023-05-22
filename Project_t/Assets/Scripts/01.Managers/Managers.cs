using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_Instatnce = null; // 싱글턴 유일성 보장
    //다른 매니저가 총괄 매지저보다 먼저 호출될 경우 매니저가 중복 생성될 위험이 있다. 그래서 별도의 Init() 함수로 관리
    public static Managers Instance { get { Init(); return s_Instatnce; } }

    private AuthManager m_Auth = new AuthManager();
    public static AuthManager Auth { get { return Instance?.m_Auth; } }
    private CorutinManager m_Corutine = new CorutinManager();
    public static CorutinManager Corutine { get { return Instance?.m_Corutine; } }
    private DataManager m_Data = new DataManager();
    public static DataManager Data { get { return Instance?.m_Data; } }
    private InputManager m_Input = new InputManager();
    public static InputManager Input { get { return Instance?.m_Input; } }
    private ResourceManager m_Resource = new ResourceManager();
    public static ResourceManager Resource { get { return Instance?.m_Resource; } }
    private SceneManagerEX m_SceneMananger = new SceneManagerEX();
    public static SceneManagerEX Scene { get { return Instance?.m_SceneMananger; } }
    private UIManager m_UIManager = new UIManager();
    public static UIManager UI { get { return Instance?.m_UIManager; } }



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
            s_Instatnce.m_Auth.Init();
            s_Instatnce.m_Data.Init();
        }
 
    }

    public static void Clear()
    {
        Input.Clear();
        UI.Clear();
        Auth.Clear();
        Corutine.Clear();
    }
}
