/// <summary>
/// Save a Dialog Action after using it
/// May be used at the end of an dialog
/// </summary>
public class SavedDialogAction : DialogAction
{

    protected override void InitValues()
    {
        base.InitValues();
        LoadState();
    }

    /// <summary>
    /// Save the Sate of the Action
    /// </summary>
    protected virtual void SaveState()
    {
        if (SaveManager.instance)
        {
            if (!SaveManager.instance.HasInteracted(gameObject.name))
            {
                SaveManager.instance.Save(gameObject.name);
            }
        }
    }


    /// <summary>
    /// Load the state of this Action from savegame
    /// </summary>
    protected virtual void LoadState()
    {
        if (SaveManager.instance)
        if (SaveManager.instance.HasInteracted(gameObject.name))
        {
            EndState();
        }
    }

    /// <summary>
    /// End State of Object, this should have it after loading
    /// </summary>
    protected virtual void EndState()
    {

    }
}
