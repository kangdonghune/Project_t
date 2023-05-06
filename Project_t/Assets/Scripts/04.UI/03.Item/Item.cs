using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Item_Base
{
    public Item(string name, Define.ItemType type, bool duplic = false,int count = 1)
    {
        this.Name = name;
        this.Type = type;
        this.Duplicate = duplic;
        this.Count = count;
    }

    public void AddCount(int count) { this.Count += count; }
}
