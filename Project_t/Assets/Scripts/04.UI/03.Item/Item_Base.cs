using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//json���� ���� ���Ͽ��� ������Ƽ ���� public���� ������� �дµ� ������ ����.
[Serializable]
public class Item_Base
{
    public int ID;
    public string Name;
    public Define.ItemType Type;
    public bool Duplicate;
    public int Count;
}
