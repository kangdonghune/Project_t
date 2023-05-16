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
    public enum UIEvent { Enter, Exit, Click,BeginDrag, Drag, EndDrag}
    public enum KeyEvent { Down, Press, None }
    
    //etc
    public enum Layer {Floor = 6, ItemBox = 7, }
    public enum ItemType { Consumable, Equipable, Projectable, None}
    public enum GridValue { Inven = 5, ItemBox = 4, None}

    //Scene
    public enum Scene { UnKnown, SignIn, Lobby, Game}
}
