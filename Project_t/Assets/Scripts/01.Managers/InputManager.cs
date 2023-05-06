using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action<Define.KeyEvent> KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    bool _LBpressed = false;
    bool _RBpressed = false;
    float _LBpressedTime = 0;
    float _RBpressedTime = 0;

    public void OnUpdate()
    {
       
        if (KeyAction != null) //����� Ű �Է� ���� �Լ��� ���� ���
        {
            if (Input.anyKeyDown)
                KeyAction.Invoke(Define.KeyEvent.Down);// Invoke() �� �����൵ ��ü������ ������ invoke()�� �ٿ��ִ� ������ ��������Ʈ��� �� ���
            if (Input.anyKey)
                KeyAction.Invoke(Define.KeyEvent.Press);
            if (!Input.anyKey)
                KeyAction.Invoke(Define.KeyEvent.None);
        }

        //UI ������ �Է��� �� ���¶�� �Ʒ� ���콺 ���� �Է��� ����
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (MouseAction != null)//����� ���콺 �Է� �Լ��� ���� ���
        {
            if (Input.GetMouseButton(0)) // ���� 0
            {
                if (false == _LBpressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.LPointerDown);
                    _LBpressedTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.LPress); //���� ��ȸ ������ ������ �ִ� �ŷ� ó��
                _LBpressed = true;

            }

            else
            {
                if (true == _LBpressed)
                {
                    if (Time.time < _LBpressedTime + 0.2f) // ���콺 ���� �� 0.2�ʰ� ������ �ʾҴٸ�
                        MouseAction.Invoke(Define.MouseEvent.LClick);
                    MouseAction.Invoke(Define.MouseEvent.LPointerUp);
                }
                _LBpressed = false;
                _LBpressedTime = 0;
            }

            if (Input.GetMouseButton(1)) // ������ 0
            {
                if (!_RBpressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.RPointerDown);
                    _RBpressedTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.RPress);
                _RBpressed = true;

            }

            else
            {
                if (_RBpressed)
                {
                    if (Time.time < _RBpressedTime + 0.2f)
                        MouseAction.Invoke(Define.MouseEvent.RClick);
                    MouseAction.Invoke(Define.MouseEvent.RPointerUp);
                }
                _RBpressed = false;
                _RBpressedTime = 0;
            }

        }
    }
}
