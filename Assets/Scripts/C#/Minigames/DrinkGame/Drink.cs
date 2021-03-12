using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Drink : MonoBehaviour
{
	public bool isAlc;

	DrinkManager drinkManager;

	[SerializeField]
	AudioClip drinkAlc;
	
	[SerializeField]
	AudioClip drinkWater;

	[SerializeField]
	MeshRenderer changeColorOnThis;

	[SerializeField]
	Color alcColor = Color.yellow;

	[SerializeField]
	Color normalColor = Color.white;

	MeshRenderer[] allRenderer;
	AudioSource source;
	private void Awake()
	{
		drinkManager = GetComponentInParent<DrinkManager>();
		source = GetComponent<AudioSource>();
		allRenderer = GetComponentsInChildren<MeshRenderer>();
	}

	//Drink the bottle by deactivating the MeshRenderer
	public void DrinkBottle()
	{
		if (isAlc == true)
		{
			drinkManager.drunkBottles++;
			source.clip = drinkAlc;
		}
		else
		{
			drinkManager.drinkBottles++;
			source.clip = drinkWater;
		}
		
		source.Play();


		SetAllRenderer(false);
	}

	public void SetBottle(bool isAlcoholic = true)
	{
		isAlc = isAlcoholic;

		if(isAlcoholic)
		{

			changeColorOnThis.material.SetColor("_Color", alcColor);
			SetAllRenderer(true);
		}
		else
		{
			changeColorOnThis.material.SetColor("_Color", normalColor);
		}
	}

	void SetAllRenderer(bool enable)
	{
		for (int i = 0; i < allRenderer.Length; i++)
		{
			allRenderer[i].enabled = enable;
		}
	}
}
