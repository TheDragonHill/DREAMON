using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HOTrigger : DialogueTrigger
{
	public Camera mainCamera;

	public override void SetEndState(bool isLose)
	{
		base.SetEndState(isLose);

		HODestroy();
	}

	public override void TheEnd(bool isLose)
	{
		base.TheEnd(isLose);

		SetEndState(isLose);
	}

	//Destroys the object when you pick it up
	public void HODestroy()
	{
		GetComponentInParent<HOManager>().foundObjects++;

		//transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - 2, mainCamera.transform.position.z + 2);

		gameObject.SetActive(false);
	}
}
