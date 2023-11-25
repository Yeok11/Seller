using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Test3 : MonoBehaviour
{
    Action testAction;
    Action<int> testAction2;

    public void _TestAction()
    {
        Debug.Log(1);
    }

    public void _TestAction2(int k)
    {
        Debug.Log(2 + k);
    }

    private void Start()
    {
        /*
        testAction += _TestAction;
        testAction += _TestAction;
        testAction += () => _TestAction2(2);

        testAction();

        testAction += _TestAction;
        testAction -= _TestAction;
        testAction -= _TestAction;
        testAction -= _TestAction;
        testAction += () => _TestAction2(2);
        testAction += () => _TestAction2(2);
        
        testAction();
        */
        //testAction2(3);
        testAction2 = (int a) => Debug.Log(testAction2); ;
        Debug.Log("³¡");
    }
}
