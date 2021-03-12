using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Objects that the player can interact with
/// </summary>
public class Interactable : OutlineObject
{
	public float radius = 3f;
	public Transform interactionTransform;

	protected bool isFocus = false;
	protected Transform playerTransform;

	public bool hasFocused = true;

	protected override void InitValues()
	{
		base.InitValues();
		if (!interactionTransform)
			interactionTransform = transform;
	}

	/// <summary>
	/// Allows Interaction with interactable Objects
	/// </summary>
	public virtual void Interact()
	{
		// This method is meant to be overwritten
	}

	

	/// <summary>
	/// Interactable is focused by the player 
	/// </summary>
	public virtual void OnFocused(Transform playerTransform)
	{
		isFocus = true;
		this.playerTransform = playerTransform;
		hasFocused = false;
	}

	/// <summary>
	/// Interactable is no longer focused by the player
	/// </summary>
	public virtual void OnDefocused()
	{
		isFocus = false;
		playerTransform = null;
		hasFocused = false;
	}

	private void OnDrawGizmosSelected()
	{
		// Draw Interactionradius
		if(interactionTransform)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(interactionTransform.position, radius);
		}
	}
}
