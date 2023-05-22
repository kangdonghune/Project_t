using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//json으로 읽을 파일에는 프로퍼티 말고 public으로 열어줘야 읽는데 문제가 없다.
[Serializable]
public class Item_Base
{
    public int ID;
    public string Name;
    public Define.ItemType Type;
    public bool Duplicate;
    public int Count;
}
