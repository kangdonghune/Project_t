using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_Instatnce = null; // �̱��� ���ϼ� ����
    //�ٸ� �Ŵ����� �Ѱ� ���������� ���� ȣ��� ��� �Ŵ����� �ߺ� ������ ������ �ִ�. �׷��� ������ Init() �Լ��� ����
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
            if(go == null) //���̶�Ű â�� �Ŵ��� ������Ʈ�� ���ٸ�
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);//���� ���� ����
            s_Instatnce = go.GetComponent<Managers>(); //���� �ٸ� �Ŵ����� �Ŵ��� ������ ���� �ڵ����� ���̶�Űâ�� ������ �Ŵ����� ��� ����
        }
    }
}
