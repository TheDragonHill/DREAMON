using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DecisionButtons : MonoBehaviour
{
	public int decisionNumber;

	DialogueManager dialogueManager;


	private void Start()
	{
		GetComponent<Button>().onClick.AddListener(() => SetDecisionNumber());

		// Set DialogManager only once
		dialogueManager = GetComponentInParent<DialogueManager>();
	}

	public void SetDecisionNumber()
	{
		dialogueManager.selectedOpinion = decisionNumber;
	}
}
