using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverrideIsLostAction : DialogAction
{
    [SerializeField]
    DialogueTrigger trigger;

    [SerializeField]
    bool newIsLostValue = true;

    protected override void InitValues()
    {
        base.InitValues();
    }

    public override void DoAction()
    {
        base.DoAction();

        trigger.isLost = newIsLostValue;
    }
}
