using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    //state
    public enum MonState { Sleep, Idle, Chase, Return, Attack, Die, None}
    public enum State { Idle, Move, Chase, Attack, Ready, None}

    //event
    public enum MouseEvent { LPress, LPointerDown, LPointerUp, LClick, RPress, RPointerDown, RPointerUp, RClick }
    public enum UIEvent { Select, DeSelect, Enter, Exit, Click,BeginDrag, Drag, EndDrag}
    public enum KeyEvent { Down, Press, None }
    
    //etc
    public enum Layer {Floor = 6, ItemBox = 7, Player = 8, MyPlayer = 9, Monster = 10}
    public enum ItemType { None, Consumable, Equipable, Projectable}
    public enum GridValue { Inven = 5, ItemBox = 4, None}

    //Scene
    public enum Scene { UnKnown, SignIn, Lobby, Game}

}
