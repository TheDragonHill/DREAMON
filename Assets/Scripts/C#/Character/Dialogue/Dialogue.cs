using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
	[System.Serializable]
	public class Talk
	{
		public DialogAction dialogAction;
		public AnimationObject animation;
		public AudioObject audio;
		public Transform cameraTarget;
		public string name;

		[TextArea(3, 10)]
		public string sentence;
	}


	[System.Serializable]
	public class Option
	{
		public bool endSentence;

		public bool nextMinigame;

		public Talk[] talks;
		

		public string[] decisions;

		public int[] nextDecisions;
	}

	public Option[] option;
}

[System.Serializable]
public class AnimationObject
{
	public Animator animator;
	public string AnimationStateName;
	public Animator secondAnimator;
	public string SecondAnimationStateName;
}

[System.Serializable]
public class AudioObject
{
	public AudioSource source;
	public AudioClip clip;
}
