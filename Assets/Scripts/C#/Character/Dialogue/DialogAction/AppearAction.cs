using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearAction : DialogAction
{
    [SerializeField]
    protected GameObject objectToAppear;

    [SerializeField]
    protected bool visible = false;

    protected override void InitValues()
    {
        base.InitValues();

        objectToAppear.SetActive(visible);
    }

    public override void DoAction()
    {
        base.DoAction();

        visible = !visible;

        objectToAppear.SetActive(visible);
    }
}
