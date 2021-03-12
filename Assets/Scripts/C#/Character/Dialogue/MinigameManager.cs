using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueTrigger))]
public class MinigameManager : MonoBehaviour
{
	[SerializeField]
	Transform camTarget;

	public GameObject mainCamera;
	public Transform cameraPosition;
	public MiniGame minigame;

	public CameraController cameraController;

	public int winRounds;
	public int loseRounds;
	public int[] nextWinDialog;
	public int[] nextLoseDialog;

	public PlayerController player;
	DialogueTrigger dialogTrigger;

	private void Start()
	{
		cameraController = mainCamera.GetComponent<CameraController>();
		player = GameObject.FindObjectOfType<PlayerController>();
		dialogTrigger = GetComponent<DialogueTrigger>();
		if (!camTarget)
			camTarget = minigame.transform;
	}

	/// <summary>
	/// Activates the minigame and point the camera at the minigame
	/// </summary>
	public virtual void StartNewMinigame()
	{
		player.motor.StopAgent();
		cameraController.MoveToFixedPosition(cameraPosition.position, camTarget);
		Invoke(nameof(SetMinigameActive), cameraController.drivingTime);
	}

	protected void SetMinigameActive()
	{
		minigame.StartMiniGame();
	}

	/// <summary>
	/// Deactivate the mini-game and point the camera back at the player
	/// </summary>
	public virtual void EndMinigame()
	{
		minigame.gameObject.SetActive(false);

		//Focusing the demon
		player.SetFocus(this.GetComponent<Interactable>());
		player.motor.ResumeAgent();


		cameraController.MoveToFixedPosition(Vector3.Lerp(player.facePoint.position, Vector3.Lerp(transform.position, cameraController.transform.position, 0.5f), 0.5f), dialogTrigger.transform);
	}

	public void StartNextDialog(bool isWin)
	{
		if (isWin == true)
		{
			for (int i = 0; i < nextWinDialog.Length; i++)
			{
				if (winRounds == i)
				{
					//Stop losing round
					EndMinigame();
					dialogTrigger.currentDialogue = nextWinDialog[i];
					dialogTrigger.hasInteracted = false;
					dialogTrigger.TriggerDialogue();
				}
			}
			winRounds++;

		}
		else if (isWin == false)
		{
			for (int i = 0; i < nextLoseDialog.Length; i++)
			{
				if (loseRounds == i)
				{
					//Stop losing round
					EndMinigame();
					dialogTrigger.currentDialogue = nextLoseDialog[i];
					dialogTrigger.hasInteracted = false;
					dialogTrigger.TriggerDialogue();
				}
			}
			loseRounds++;
		}

		if (nextLoseDialog.Length == loseRounds)
		{
			dialogTrigger.isLost = true;
		}
	}
}
