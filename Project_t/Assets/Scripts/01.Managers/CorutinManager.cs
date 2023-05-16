using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorutinManager 
{
 
    public void CallWaitForOneFrame(Action act)
    {
        Managers.Instance.StartCoroutine(DoCallWaitForOneFrame(act));
    }

    public void CallWaitForSeconds(float seconds, Action act)
    {
        Managers.Instance.StartCoroutine(DoCallWaitForSeconds(seconds, act));
    }

    private IEnumerator DoCallWaitForOneFrame(Action act)
    {
        yield return 0;
        act();
    }

    private IEnumerator DoCallWaitForSeconds(float seconds, Action act)
    {
        yield return new WaitForSeconds(seconds);
        act();
    }

    public void Clear()
    {
        Managers.Instance.StopAllCoroutines();
    }
}
