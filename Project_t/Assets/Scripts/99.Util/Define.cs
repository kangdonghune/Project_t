using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum MonState { Sleep, Idle, Chase, Return, Attack, Die, None}
    public enum State { Idle, Move}
    public enum MouseEvent { LPress, LPointerDown, LPointerUp, LClick, RPress, RPointerDown, RPointerUp, RClick }
    public enum KeyEvent { Down, Press, None }
    public enum Layer { Floor = 6, }
}
