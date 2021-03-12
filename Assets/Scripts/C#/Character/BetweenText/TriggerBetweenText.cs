using UnityEngine;

/// <summary>
/// Trigger BetweenText on player enter
/// </summary>
[RequireComponent(typeof(Collider))]
public class TriggerBetweenText : MonoBehaviour
{
    [SerializeField]
    TimedTalk[] timedTalks;
   

    bool triggered = false;

    void Start()
    {
        InitValues();
    }

    void InitValues()
    {
        GetComponent<Collider>().isTrigger = true;

        //Subscribe event for loading Savefile
        if (SaveManager.instance)
            SaveManager.instance.OnLoadSave += LoadState;
    }

    /// <summary>
    /// If not already triggerd Callbetweentext and save
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if(!triggered)
            if (other.GetComponent<CallBetweenText>())
            {
                other.GetComponent<CallBetweenText>().CallBetween(timedTalks);
                triggered = true;
                SaveState();
            }
    }

    /// <summary>
    /// Save state of this Trigger
    /// </summary>
    void SaveState()
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
	/// Load the state of this Trigger from savegame
	/// </summary>
	void LoadState()
    {
        if (triggered = SaveManager.instance.HasInteracted(gameObject.name))
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Unsubscribe event for loading savedata
    /// </summary>
    private void OnDisable()
    {
        if (SaveManager.instance)
            SaveManager.instance.OnLoadSave -= LoadState;
    }
}
