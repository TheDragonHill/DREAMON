using UnityEngine;

public class LoadSettingsInScene : MonoBehaviour
{
    void Start()
    {
        if(SaveManager.instance)
        {
            SaveManager.instance.IntegrateSettingsData();
        }
    }
}
