using System;
using UnityEngine;

/// <summary>
/// Door Mechanic for Cemetery
/// </summary>
[RequireComponent(typeof(Collider), typeof(AudioSource))]
public class DoorMechanic : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    string openDoor;

    [SerializeField]
    string closeDoor;

    [SerializeField]
    AnimationClip animClip;

    [SerializeField]
    AudioClip openDoorClip;

    [SerializeField]
    AudioClip closeDoorClip;

    [SerializeField]
    Transform exitPoint;

    [SerializeField]
    Transform camPosition;

    [SerializeField]
    GameObject[] objectsToActivate;

    DialogueManager dialogueManager;
    AudioSource source;
    bool closeForever;

    private void Start()
    {
        InitValues();
    }

    void InitValues()
    {
        if(!(closeForever = LoadState()))
        {
            source = GetComponent<AudioSource>();
            dialogueManager = FindObjectOfType<DialogueManager>();
            ActivateObjects(false);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerController>() && !closeForever)
        {
            MoveDoor(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() && !closeForever)
        {
            if(exitPoint && CheckExitSide(other.transform.position))
            {
                MoveDoor(false);
                ActivateObjects(true);
                closeForever = true;
                SaveState();
            }
        }
    }

    /// <summary>
    /// Check if the player is walking through the right exitpoint
    /// </summary>
    bool CheckExitSide(Vector3 other)
    {
        return Vector3.Distance(transform.position, other) > Vector3.Distance(exitPoint.position, other);
    }

    /// <summary>
    /// Open / Close the door
    /// </summary>
    void MoveDoor(bool open)
    {
        string buffer = open ? openDoor : closeDoor;

        // Check Null
        if(!string.IsNullOrEmpty(buffer) && animator)
        {
            // Check is same State
            if(!animator.GetCurrentAnimatorStateInfo(0).IsName(buffer))
            {
                animator.Play(buffer);
                PlaySound(open);

                SetCutScene(open);
            }
        }
    }

    /// <summary>
    /// Activate / Deactivate Objects
    /// </summary>
    /// <param name="setActive">Should the Objects be active</param>
    void ActivateObjects(bool setActive)
    {
        if(objectsToActivate != null && objectsToActivate.Length > 0)
        {
            for (int i = 0; i < objectsToActivate.Length; i++)
            {
                objectsToActivate[i].SetActive(setActive);
            }
        }
    }

    /// <summary>
    /// Set Cutscene for object
    /// </summary>
    void SetCutScene(bool open)
    {
        // Set player
        dialogueManager.player.motor.StopAgent();
            
        // Set camera
        if(camPosition && !open)
        {
            dialogueManager.cameraController.MoveToFixedPosition(camPosition.position, transform);
        }


        // Reset every Time
        Invoke(nameof(ResetCutScene), animClip.length / 2);
    }

    /// <summary>
    /// Reset Cutscene for object
    /// </summary>
    void ResetCutScene()
    {
        // Set player
        dialogueManager.player.motor.ResumeAgent();

        // Set camera
        if (camPosition)
        {
            dialogueManager.cameraController.MoveToOffset(dialogueManager.player.transform);
            dialogueManager.cameraController.StartResetCameraToPlayer();
        }
    }

    /// <summary>
    /// Play the right sound for the animation
    /// </summary>
    void PlaySound(bool open)
    {
        source.clip = open ? openDoorClip : closeDoorClip;

        if (source.clip)
        {
            source.Play();
        }
    }

    /// <summary>
    /// Save the current state of an Object
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
    bool LoadState()
    {
        if (SaveManager.instance)
        {
            return SaveManager.instance.HasInteracted(gameObject.name);
        }

        return false;
    }
}
