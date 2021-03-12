using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchObjectAction : SavedDialogAction
{
    [SerializeField]
    GameObject objectToSetInactive;

    [SerializeField]
    GameObject objectToSetActive;

    protected override void InitValues()
    {
        base.InitValues();
    }


    public override void DoAction()
    {
        base.DoAction();
        EndState();
        SaveState();
    }

    protected override void EndState()
    {
        base.EndState();
        objectToSetInactive.SetActive(false);
        objectToSetActive.SetActive(true);

    }

}
