using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEgoDialog : DialogueTrigger
{
	[SerializeField]
	TimedTalk[] chessTalk;

	[SerializeField]
	TimedTalk[] tutorialTalk;


	protected override void InitValues()
	{
		base.InitValues();

		//Starts interacting with the player
		Invoke(nameof(StartDialogAfterTime), .1f);
	}

	void StartDialogAfterTime()
	{
		TriggerDialogue();
		dialogueManager.minigameManager = minigameManager;
		dialogueManager.player.callBetween.CallBetween(chessTalk);
	}

	public override void TheEnd(bool isLose)
	{
		base.TheEnd(isLose);
		dialogueManager.player.callBetween.EndCall();
		if(tutorialTalk.Length > 0)
			dialogueManager.player.callBetween.CallBetween(tutorialTalk);

		SetEndState(isLose);
	}

    public override void SetEndState(bool isLose)
    {
        base.SetEndState(isLose);
		SetInactive();
	}
}
