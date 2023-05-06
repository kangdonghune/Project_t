using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    //state
    public enum MonState { Sleep, Idle, Chase, Return, Attack, Die, None}
    public enum State { Idle, Move}

    //event
    public enum MouseEvent { LPress, LPointerDown, LPointerUp, LClick, RPress, RPointerDown, RPointerUp, RClick }
    public enum UIEvent { Click, Drag}
    public enum KeyEvent { Down, Press, None }
    
    //etc
    public enum Layer { Floor = 6, }
    public enum ItemType { Consumable, Equipable, Projectable, None}
    public enum GridValue { Inven = 5, ItemBox = 4, None}
}
