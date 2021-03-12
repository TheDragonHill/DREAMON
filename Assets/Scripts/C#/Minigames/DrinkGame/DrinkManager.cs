using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DrinkManager : MiniGame
{
	[SerializeField]
	Canvas MiniGameCanvas;
	
	public TextMeshProUGUI timerText;
	public TextMeshProUGUI drinkBottleText;
	public TextMeshProUGUI drunkBottleText;

	public int drinkBottles;
	public int maxBottles;

	public int drunkBottles;
	public int maxAlcBottles;

	public float drinkTime = 3;
	private float startDrinkTime;

	public float maxBottleTimer = 3;
	public float bottleTime;

	public Drink[] bottles;

	public GameObject assignedTarget;

	[SerializeField]
	Animator drinkAnimator;

	[SerializeField]
	string drinkAnimationState;

	[SerializeField]
	Animator dementumAnimator;

	[SerializeField]
	string drinkIdle;

    public override void StartMiniGame()
    {
		gameObject.SetActive(true);
		MiniGameCanvas.gameObject.SetActive(true);
		UnityEngine.Cursor.visible = true;
		DrinkAnimation();
		startDrinkTime = drinkTime;
		bottles = GetComponentsInChildren<Drink>();
		bottleTime = maxBottleTimer;
		RandomAlc();
        base.StartMiniGame();
	}

	private void OnDisable()
	{
		if(MiniGameCanvas)
			MiniGameCanvas.gameObject.SetActive(false);
	}

    protected override IEnumerator MiniGameUpdate()
    {
		do
        {
			if (MiniGameCanvas.gameObject.activeInHierarchy)
			{
				// TODO: Slider im Krug
				timerText.text = string.Join(":", (((drinkTime - drinkTime % 60) / 60)).ToString("00"), Mathf.Round(drinkTime % 60).ToString("00"));
				drinkBottleText.text = string.Concat("Water: ", drinkBottles.ToString(), " / ", maxBottles.ToString());
				drunkBottleText.text = string.Concat("<color=yellow>Alcohol: ", drunkBottles.ToString(), " / ", maxAlcBottles.ToString());
			}
			CheckGame();
			TimingBottle();

			yield return base.MiniGameUpdate();
        } while (gameObject.activeInHierarchy);

	}

	void TimingBottle()
	{
		bottleTime -= Time.deltaTime;

		if (bottleTime <= 0)
		{
			bottleTime = maxBottleTimer;
			RandomAlc();
		}
	}

	void CheckGame()
	{
		drinkTime -= Time.deltaTime;

		if (drinkTime >= 0)
		{
			if (drinkBottles == maxBottles)
			{
				Win();
			}
			if (drunkBottles == maxAlcBottles)
			{
				//Drink to much alcohole
				Lost();
			}
		}
		else
		{
			Lost();
		}
	}

	public void Win()
	{
		EndMiniGame();
		//Stop game
		assignedTarget.GetComponent<MinigameManager>().StartNextDialog(true);
	}

	public void Lost()
	{
		EndMiniGame();
		//Stop game
		assignedTarget.GetComponent<MinigameManager>().StartNextDialog(false);
	}

    protected override void EndMiniGame()
    {
        base.EndMiniGame();
		ResetGame();
	}

	public void DrinkAnimation()
    {
		if(dementumAnimator && !string.IsNullOrEmpty(drinkIdle))
        {
			dementumAnimator.CrossFade(drinkIdle, 0.3f);
        }

		if(drinkAnimator && !string.IsNullOrEmpty(drinkAnimationState))
        {
			if (drinkAnimator.GetCurrentAnimatorStateInfo(0).IsName(drinkAnimationState))
				drinkAnimator.CrossFade(0, 0.3f);
			else
				drinkAnimator.CrossFade(drinkAnimationState, 0.3f);
        }
    }

	void RandomAlc()
	{
		// Reset All Bottles
		for (int i = 0; i < bottles.Length; i++)
		{
			// Reset to Alc
			bottles[i].SetBottle();
		}

		// Set Non Alc Bottles
		// Can be double index
		for (int i = 0; i < Random.Range(1, bottles.Length); i++)
		{
			bottles[Random.Range(0, bottles.Length)].SetBottle(false);
		}
	}

	void ResetGame()
	{
		drinkBottles = 0;
		drunkBottles = 0;
		bottleTime = 0;
		drinkTime = startDrinkTime;
	}
}
