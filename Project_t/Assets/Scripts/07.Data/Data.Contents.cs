using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Item
[Serializable]
public class ItemData : ILoader<int, Item>
{
    public List<Item> items = new List<Item>();

    public Dictionary<int, Item> MakeDict()
    {
        Dictionary<int, Item> dict = new Dictionary<int, Item>();
        foreach (Item item in items)
            dict.Add(item.ID, item);
        return dict;
    }
}
#endregion
