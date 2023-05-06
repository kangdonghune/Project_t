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
       
        if (KeyAction != null) //예약된 키 입력 관련 함수가 있을 경우
        {
            if (Input.anyKeyDown)
                KeyAction.Invoke(Define.KeyEvent.Down);// Invoke() 를 안해줘도 자체적으로 하지만 invoke()를 붙여주는 것으로 델리게이트라는 걸 명시
            if (Input.anyKey)
                KeyAction.Invoke(Define.KeyEvent.Press);
            if (!Input.anyKey)
                KeyAction.Invoke(Define.KeyEvent.None);
        }

        //UI 쪽으로 입력이 들어간 상태라면 아래 마우스 관련 입력을 생략
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (MouseAction != null)//예약된 마우스 입력 함수가 있을 경우
        {
            if (Input.GetMouseButton(0)) // 왼쪽 0
            {
                if (false == _LBpressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.LPointerDown);
                    _LBpressedTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.LPress); //다음 순회 때부턴 누르고 있는 거로 처리
                _LBpressed = true;

            }

            else
            {
                if (true == _LBpressed)
                {
                    if (Time.time < _LBpressedTime + 0.2f) // 마우스 누른 후 0.2초가 지나지 않았다면
                        MouseAction.Invoke(Define.MouseEvent.LClick);
                    MouseAction.Invoke(Define.MouseEvent.LPointerUp);
                }
                _LBpressed = false;
                _LBpressedTime = 0;
            }

            if (Input.GetMouseButton(1)) // 오른쪽 0
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
