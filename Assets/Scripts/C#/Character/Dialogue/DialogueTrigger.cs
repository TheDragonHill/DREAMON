using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DialogueTrigger : Interactable
{
	public Dialogue[] dialogue;

	public int currentDialogue;

	public MinigameManager minigameManager;

	public Transform camPosition;

	protected DialogueManager dialogueManager;

	public AudioSource source;

	public bool? isLost;

	protected PlayerController player;



	protected override void InitValues()
	{

		base.InitValues();
		dialogueManager = FindObjectOfType<DialogueManager>();
		player = dialogueManager.player;
		if (SaveManager.instance)
			SaveManager.instance.OnLoadSave += LoadState;
	}


	//Starts interacting with the player
	public override void Interact()
	{
		PlaySound();
		TriggerDialogue();
	}

	//Starts the dialog
	public void TriggerDialogue()
	{
		if (hasInteracted == false && !dialogueManager.currentDialogObject || !dialogueManager.selectMinigame)
		{
			dialogueManager.StartDialogue(this);
			hasInteracted = true;
		}
	}

	public virtual void SetPlayerState()
    {
		player.motor.StopAgent();
	}

	public virtual void TheEnd(bool isLose)
	{
		SaveState(!isLose);

		// Do Something
	}

	public virtual void SetEndState(bool isLose)
    {

    }

	protected virtual void PlaySound()
	{
		if (source)
			if (source.clip)
				source.Play();
	}

	protected virtual void SetInactive()
	{
		gameObject.SetActive(false);
	}

	protected virtual void SaveState(bool result)
	{
		if(SaveManager.instance)
		{
			if(!SaveManager.instance.HasInteracted(gameObject.name))
			{
				SaveManager.instance.Save(gameObject.name, result);
			}
		}
	}

	protected virtual void Update()
	{
		CheckForInteraction();
	}

	protected virtual void CheckForInteraction()
	{
		if (interactionTransform && isFocus && !hasFocused)
		{
			float distance = Vector2.Distance(new Vector2(playerTransform.position.x, playerTransform.position.z), new Vector2(interactionTransform.position.x, interactionTransform.position.z));
			if (distance < radius || distance < 0.2f)
			{
				Interact();
				hasFocused = true;
			}
		}
	}


	/// <summary>
	/// Load the state of this Trigger from savegame
	/// </summary>
	protected virtual void LoadState()
	{
		hasInteracted = SaveManager.instance.HasInteracted(gameObject.name);
		isLost = !SaveManager.instance.HasResult(gameObject.name);
		if (hasInteracted)
		{
			CancelInvoke();
			SetEndState((bool)isLost);
		}
	}

	private void OnDisable()
	{
		if (SaveManager.instance)
			SaveManager.instance.OnLoadSave -= LoadState;
	}
}
