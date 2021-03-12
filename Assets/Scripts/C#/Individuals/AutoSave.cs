using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSave : MonoBehaviour
{
    public delegate void Save();
    public event Save OnAutoSave;

    [SerializeField]
    float autoSaveSeconds = 5;

    float currentTimer;

    // Start is called before the first frame update
    void Start()
    {
        InitValues();
    }

    void InitValues()
    {
        if(SaveManager.instance)
        {
            SaveManager.instance.currentAutoSave = this;
            StartCoroutine(LateStartLoad());
        }
        currentTimer = Time.time;
    }

    public void ForceSave()
    {
        if(OnAutoSave != null)
        {
            OnAutoSave();
            currentTimer = Time.time;
        }
    }

    IEnumerator LateStartLoad()
    {
        yield return new WaitForFixedUpdate();
        SaveManager.instance.LoadSaveInScene();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if(OnAutoSave != null)
        if(Time.time - currentTimer >= autoSaveSeconds)
        {
            OnAutoSave();
            currentTimer = Time.time;
        }
    }
}
