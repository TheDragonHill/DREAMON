using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class PathBlocker : MonoBehaviour
{
    [SerializeField]
    Transform pathBlockerEndPosition;

    AudioSource audioSource;
    DialogueTrigger dialogueTrigger;

    [SerializeField]
    GameObject[] nextObjects;

    [SerializeField]
    bool deactivateNextObjectsOnStart = false;


    private void Start()
    {
        LoadState();


        if (deactivateNextObjectsOnStart)
        {
            ActivateNextObjects(false);
        }
    }


    public void ClearPathBlocker()
    {
        PlayAudio();
        EndState();
        SaveState();
    }

    public void EndState()
    {
        if (dialogueTrigger = GetComponent<DialogueTrigger>())
        {
            dialogueTrigger.enabled = false;
            dialogueTrigger.hasInteracted = true;
            dialogueTrigger.TheEnd(false);
        }
        transform.position = pathBlockerEndPosition.position;
        transform.rotation = pathBlockerEndPosition.rotation;
        ActivateNextObjects(true);
    }

    void ActivateNextObjects(bool setActive = true)
    {
        for (int i = 0; i < nextObjects.Length; i++)
        {
            nextObjects[i].SetActive(setActive);
        }
    }

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
    /// Load the state of this Trigger from savegame
    /// </summary>
    protected virtual void LoadState()
    {
        if(SaveManager.instance)
        if (SaveManager.instance.HasInteracted(gameObject.name))
        {
            EndState();
        }
    }


    void PlayAudio()
    {
        if(!audioSource)
            audioSource = GetComponent<AudioSource>();
        
        if (audioSource.clip)
            audioSource.Play();
    }
}
