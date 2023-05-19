using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeInputs : MonoBehaviour
{
    EventSystem system;
    public Selectable firstInput;
    public Button submitButton;


    void Start()
    {
        system = EventSystem.current;
        // ó���� �̸��� Input Field�� �����ϵ��� �Ѵ�.
        firstInput.Select();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            // Tab + LeftShift�� ���� Selectable ��ü�� ����
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
            if (next != null)
            {
                next.Select();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Tab�� �Ʒ��� Selectable ��ü�� ����
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next != null)
            {
                next.Select();
            }
        }
    }
}
