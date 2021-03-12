using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

/// <summary>
/// Needs to be Switched
/// </summary>
public class Dementum : DialogueTrigger
{
    [SerializeField]
    GameObject[] objectsToDeactivate;

    [SerializeField]
    GameObject lostCandleLight;

    [SerializeField]
    float deathAnimationTime = 6;

    [SerializeField]
    PolterGeist[] tavernpoltergeister;

    [SerializeField]
    PathBlocker blocker;

    [SerializeField]
    Dialogue[] lostDialogue;

    [SerializeField]
    Transform lostCameraTransform;

    [SerializeField]
    Transform lostPlayerPos;

    [SerializeField]
    AudioClip backgroundMusicClip;

    [SerializeField]
    Animator blackScreen;

    [SerializeField]
    string blackScreenStateName;

    [SerializeField]
    GameObject activateObject;

    [SerializeField]
    GhostLightSpawn lightSpawn;

    [SerializeField]
    Animator animator;

    [SerializeField]
    string[] idleAnimations;

    int currentidle = 0;

    AudioClip oldBackgroundMusic;
    bool endCall = false; 

    protected override void InitValues()
    {
        base.InitValues();
    }

    protected override void Update()
    {
        base.Update();
        PlayIdleAnimation();
    }

    void PlayIdleAnimation()
    {
        if(!hasInteracted && !endCall)
        {
            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
            {
                animator.CrossFade(idleAnimations[currentidle], 0.3f);

                currentidle = currentidle + 1 >= idleAnimations.Length ? 0 : currentidle + 1;
            }
        }
    }


    protected override void PlaySound()
    {
        base.PlaySound();
        oldBackgroundMusic = AudioManager.Instance.GetSourceClip(0);

        AudioManager.Instance.SetSourceClip(backgroundMusicClip);

    }

    public override void TheEnd(bool isLose)
    {
        endCall = !endCall;
        // Lost the Game:
        if(isLose)
        {
            // Lost Dialogue
            LostDialogue();

        }

        // Both Endings:
        
        // Backgroundmusic off
        if (AudioManager.Instance)
            AudioManager.Instance.SetSourceClip(null, 0, AudioManager.Instance.GetSamples(0));
        // Ambience Music off
        if (AudioManager.Instance)
            AudioManager.Instance.SetSourceClip(null, 1, AudioManager.Instance.GetSamples(1));

        SetEndState(isLose);
        // Tür speichern
        blocker?.ClearPathBlocker();

        base.TheEnd(isLose);
    }

    public override void SetEndState(bool isLose)
    {
        base.SetEndState(isLose);

        if(isLose)
        {
            // Remove all Chairs, etc.
            // + Remove light sources
            DissapearObjects();
            // Except one Light
            // Activate this Light beside Elios
            lostCandleLight.SetActive(true);
        }
        else
        {
            // Won the Game:
            // Light near Dementum disappears
            // This Light is the first in the List
            objectsToDeactivate.First().SetActive(false);

            activateObject.SetActive(true);
            Invoke(nameof(MakeLight), deathAnimationTime);
        }

        // Both Endings

        // Tür geht auf
        blocker?.EndState();
        
        // Poltergeist off
        if (tavernpoltergeister != null)
        {
            for (int i = 0; i < tavernpoltergeister.Length; i++)
            {
                tavernpoltergeister[i].enabled = false;
            }

        }

        // Set Gameobject inactive
        Invoke(nameof(SetInactive), deathAnimationTime + 0.5f);
    }


    void MakeLight()
    {
        lightSpawn.Light(1);
    }


    void DissapearObjects()
    {
        if (objectsToDeactivate.Length > 0)
        {
            for (int i = 0; i < objectsToDeactivate.Length; i++)
            {
                objectsToDeactivate[i].SetActive(false);
            }
        }
    }

    void LostDialogue()
    {
        if (SaveManager.instance)
            if (SaveManager.instance.HasInteracted(gameObject.name))
                return;

        if (lostDialogue.Length > 0)
        {
            blackScreen.Play("BlackOut");
            dialogue = lostDialogue;

            // Reset Values
            dialogueManager.cameraController.CancelResetCameraToPlayer();
            hasInteracted = false;
            currentDialogue = 0;
            
            // Set Positions
            camPosition = lostCameraTransform;
            player.transform.position = lostPlayerPos.position;


            // Start new Dialogue
            TriggerDialogue();
        }
    }
}
