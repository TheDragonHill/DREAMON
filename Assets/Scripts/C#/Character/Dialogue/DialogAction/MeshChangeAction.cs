using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshChangeAction : SavedDialogAction
{
    [SerializeField]
    MeshFilter meshFilter;

    [SerializeField]
    Mesh newMesh;


    public override void DoAction()
    {
        base.DoAction();
        EndState();
        SaveState();
    }

    protected override void EndState()
    {
        base.EndState();

        if(meshFilter && newMesh)
            meshFilter.sharedMesh = newMesh;
    }
}
