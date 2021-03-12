using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HOManager : MiniGame
{
	public GameObject[] hiddenObjects;
	public int foundObjects;

	public GameObject assignedTarget;

	public float searchTime = 60;

    public override void StartMiniGame()
    {
		gameObject.SetActive(true);
		base.StartMiniGame();
    }

    protected override void EndMiniGame()
    {
        base.EndMiniGame();
	}

	private void Update()
	{
		searchTime -= Time.deltaTime;

		if (searchTime < 0)
		{
			assignedTarget.GetComponent<MinigameManager>().StartNextDialog(false);
			EndMiniGame();
		}
		else if (hiddenObjects.Length == foundObjects)
		{
			assignedTarget.GetComponent<MinigameManager>().StartNextDialog(true);
			EndMiniGame();
		}
	}

	//protected override IEnumerator MiniGameUpdate()
 //   {
	//	do
	//	{
	//		searchTime -= Time.deltaTime;

	//		if (searchTime < 0)
	//		{
	//			assignedTarget.GetComponent<MinigameManager>().StartNextDialog(false);
	//			EndMiniGame();
	//		}
	//		else if (hiddenObjects.Length == foundObjects)
	//		{
	//			assignedTarget.GetComponent<MinigameManager>().StartNextDialog(true);
	//			EndMiniGame();
	//		}
	//	} while (gameObject.activeInHierarchy);

	//	return base.MiniGameUpdate();
	//}
}
