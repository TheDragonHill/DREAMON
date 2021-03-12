using UnityEngine;


/// <summary>
/// Change a material while a dialog is running
/// </summary>
public class MaterialChangeAction : DialogAction
{
    [SerializeField]
    Renderer rendererToChange;

    [SerializeField]
    Material newMaterial;

    public override void DoAction()
    {
        ChangeMaterial();
    }

    /// <summary>
    /// Change the Material of the chosen Renderer
    /// </summary>
    void ChangeMaterial()
    {
        if (rendererToChange && newMaterial)
        {
            rendererToChange.sharedMaterial = newMaterial;
        }
    }
}
