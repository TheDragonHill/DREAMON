using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearDelayAction : AppearAction
{
    [SerializeField]
    float delay = 1;

    public override void DoAction()
    {
        Invoke(nameof(DelayAction), delay);
    }

    void DelayAction()
    {
        base.DoAction();
    }

}
