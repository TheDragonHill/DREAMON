using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LODGroup), typeof(Renderer))]
public class LODSet : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField]
    float culledPercentage = 0.08f;

    LODGroup lod;
    private void Start()
    {
        SetUpLod();
    }

    void SetUpLod()
    {
        lod = GetComponent<LODGroup>();
        lod.SetLODs(new LOD[] { new LOD(culledPercentage, GetComponents<Renderer>()) });
    }

    private void OnValidate()
    {
        SetUpLod();
    }

}
