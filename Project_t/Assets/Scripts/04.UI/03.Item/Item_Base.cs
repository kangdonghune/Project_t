using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_Base
{
    public string Name { get; protected set; }
    public Define.ItemType Type { get; protected set; }
    public bool Duplicate { get; protected set; }
    public int Count { get; protected set; }
}
