using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartCredits : MonoBehaviour
{
	public Animator transition;


	private void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<PlayerController>())
			StartCoroutine(LoadLevel());

	}

	IEnumerator LoadLevel()
	{
		//Play animation
		transition.SetTrigger("Start");

		//Wait
		yield return new WaitForSeconds(4);

		//Load scene
		SceneManager.LoadScene("Credits");
	}

}
