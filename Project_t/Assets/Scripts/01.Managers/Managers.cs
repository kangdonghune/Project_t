using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_Instatnce = null; // �̱��� ���ϼ� ����
    //�ٸ� �Ŵ����� �Ѱ� ���������� ���� ȣ��� ��� �Ŵ����� �ߺ� ������ ������ �ִ�. �׷��� ������ Init() �Լ��� ����
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
            if(go == null) //���̶�Ű â�� �Ŵ��� ������Ʈ�� ���ٸ�
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);//���� ���� ����
            s_Instatnce = go.GetComponent<Managers>(); //���� �ٸ� �Ŵ����� �Ŵ��� ������ ���� �ڵ����� ���̶�Űâ�� ������ �Ŵ����� ��� ����
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
