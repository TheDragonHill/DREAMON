using UnityEngine;

/// <summary>
/// Parentclass for Dialogactions
/// </summary>
public class DialogAction : MonoBehaviour
{
    private void Start()
    {
        InitValues();
    }

    protected virtual void InitValues()
    {

    }

    public virtual void DoAction()
    {

    }
}
