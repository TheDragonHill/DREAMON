using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class BoardManager : MiniGame
{
	public static BoardManager Instance { set; get; }
	private bool[,] allowedMoves { set; get; }

	public Chessman[,] Chessmans { set; get; }
	private Chessman selectedChessman;

	private int selectionX = -1;
	private int selectionY = -1;

	public List<GameObject> chessmanPrefabs;
	private List<GameObject> activeChessman = new List<GameObject>();

	public GameObject selectedHighlight;

	int currentChessmans = 3;

	public GameObject assignedTarget;

	int rounds;
	int winRounds;
	int loseRounds;

	[SerializeField]
	AudioClip[] movePiece;

	[SerializeField]
	AudioClip[] hitPiece;

	[SerializeField]
	TextMeshProUGUI roundText;

	AudioSource source;

	bool mouseDown = false;

	private void Start()
	{
		source = GetComponent<AudioSource>();
		Instance = this;
		SpawnAllChessmans();
		Cursor.visible = true;
		selectedHighlight = Instantiate(selectedHighlight, transform);
	}

    public override void StartMiniGame()
    {
		gameObject.SetActive(true);
		rounds = 0;
        base.StartMiniGame();
    }

    private void Update()
    {
		mouseDown = Input.GetMouseButtonDown(0);
    }

    protected override IEnumerator MiniGameUpdate()
	{
		do
		{
			//If the player lose the game
			if (currentChessmans == 0)
			{
				loseRounds++;

				CheckForWinLose();

				SpawnAllChessmans();
			}

			UpdateSelection();
			DrawChessboard();

			if (mouseDown)
			{
				if (selectionX >= 0 && selectionY >= 0)
				{
					if (Chessmans[selectionX, selectionY] && !Chessmans[selectionX, selectionY].isEnemy)
					{
						//Select the chessman
						SelectChessman(selectionX, selectionY);
					}
					else
					{
						// Move Chessman and check if Chessman is standing in the last row
						if (MoveChessman(selectionX, selectionY))
						{
							winRounds++;

							Invoke(nameof(CheckForWinLose), 0.3f);

							Invoke(nameof(SpawnAllChessmans), 0.5f);
						}

					}
				}
			}

			yield return base.MiniGameUpdate();

		} while (gameObject.activeInHierarchy);

    }

	void CheckForWinLose()
    {
		if (rounds == 3)
		{
			if (winRounds < loseRounds)
			{
				EndMiniGame();
				assignedTarget.GetComponent<MinigameManager>().StartNextDialog(false);
			}
			else if (winRounds > loseRounds)
			{
				EndMiniGame();
				assignedTarget.GetComponent<MinigameManager>().StartNextDialog(true);
			}
		}
	}

	private void SelectChessman(int x, int z)
	{
		if (Chessmans[x,z] == null)
		{
			return;
		}
		BoardHighlights.Instance.Hidehighlights();

		allowedMoves = Chessmans[x, z].PossibleMove();
		selectedChessman = Chessmans[x, z];
		BoardHighlights.Instance.HighlightAllowedMoves(allowedMoves);
	}

	private bool MoveChessman(int x, int z)
	{
		bool winningMove = false;

		if (selectedChessman != null && allowedMoves != null && allowedMoves[x,z])
		{
			Chessman c = Chessmans[x, z];

			Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;

			if (c != null)
			{
				//capture a piece
				activeChessman.Remove(c.gameObject);

				c.transform.DOLocalJump(c.transform.localPosition + new Vector3(0, 0, 1), 0.1f, 1, 0.2f);
				Destroy(c.gameObject, 0.2f);

				MoveLocalTransform(selectedChessman.transform, new Vector3(x + 0.5f, selectedChessman.transform.localScale.y / 2, z + 0.5f), 0.3f);
				Destroy(selectedChessman.gameObject, 0.3f);

				currentChessmans--;

				source.clip = hitPiece[Random.Range(0, hitPiece.Length)];
			}
			else
			{
				source.clip = movePiece[Random.Range(0, movePiece.Length)];
				selectedChessman.SetPosition(x, z);
				Chessmans[x, z] = selectedChessman;
			}


			Vector3 moveVector = new Vector3(x + 0.5f, selectedChessman.transform.localScale.y / 2, z + 0.5f);

			MoveLocalTransform(selectedChessman.transform, moveVector, 0.3f);

			source.Play();

			winningMove = moveVector.z > 7;
		}

		BoardHighlights.Instance.Hidehighlights();
		selectedChessman = null;

		return winningMove;
	}

	private void UpdateSelection()
	{
		if (!Camera.main)
		{
			return;
		}

		RaycastHit hit;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ChessPlane")))
		{
			
			selectionX = (int)transform.InverseTransformPoint(hit.point).x;
			selectionY = (int)transform.InverseTransformPoint(hit.point).z;

		}
		else
		{
			selectionX = -1;
			selectionY = -1;
		}
	}

	private void SpawnChessman(int index, int x, int z)
	{

		GameObject go = Instantiate(chessmanPrefabs[index]) as GameObject;
		go.transform.SetParent(transform);
		go.transform.rotation = transform.rotation;

		MoveLocalTransform(go.transform, new Vector3(x + 0.5f, go.transform.localScale.y / 2, z + 0.5f), 0.5f);

		Chessmans[x, z] = go.GetComponent<Chessman>();
		Chessmans[x, z].SetPosition(x, z);
		activeChessman.Add(go);
	}

	void MoveLocalTransform(Transform target,Vector3 newPosition, float time)
    {
		target.DOLocalJump(newPosition, 0.1f, 1, time);
    }

	private void SpawnAllChessmans()
	{
		Chessmans = new Chessman[8, 8];

		if (rounds > 0)
		{
			for (int i = 0; i < activeChessman.Count; i++)
			{
				Destroy(activeChessman[i]);
			}

			activeChessman.Clear();
		}

		if (rounds == 0)
		{
			//Spawn the players pieces
			SpawnChessman(0, 7, 1);
			SpawnChessman(0, 6, 0);

			//Spawn the enemys pieces
			for (int i = 0; i < 8; i++)
			{
				SpawnChessman(1, i, 6);
			}
		}
		else if (rounds == 1)
		{
			//Spawn the players pieces
			SpawnChessman(0, 0, 2);
			SpawnChessman(0, 7, 1);
			SpawnChessman(0, 6, 0);
			SpawnChessman(0, 2, 0);

			//Spawn the enemys pieces
			SpawnChessman(1, 3, 2);
			SpawnChessman(1, 4, 2);
			SpawnChessman(1, 5, 3);
			SpawnChessman(1, 5, 4);
			SpawnChessman(1, 2, 3);
			SpawnChessman(1, 2, 4);

			for (int i = 0; i < 8; i++)
			{
				SpawnChessman(1, i, 6);
			}
		}
		else if (rounds == 2)
		{
			//Spawn the players pieces
			SpawnChessman(0, 4, 0);
			SpawnChessman(0, 2, 2);

			SpawnChessman(1, 2, 5);
			SpawnChessman(1, 2, 4);
			SpawnChessman(1, 5, 5);
			SpawnChessman(1, 5, 4);
			SpawnChessman(1, 0, 2);
			SpawnChessman(1, 7, 2);

			//Spawn the enemys pieces
			for (int i = 0; i < 8; i++)
			{
				SpawnChessman(1, i, 6);
			}
		}

		ResetChess();
	}

	private void ResetChess()
	{
		rounds++;
		currentChessmans = activeChessman.Count(c => c.GetComponent<Pieces>() == true);
		selectionX = 0;
		selectionY = 0;
		ShowRounds();
	}

	private void DrawChessboard()
	{
		//Draw the selection
		if (selectionX >= 0 && selectionY >= 0)
		{
			selectedHighlight.transform.localPosition = Vector3.forward * (selectionY + 0.5f) + Vector3.right * (selectionX + 0.5f) + Vector3.up * 0.002f;
			selectedHighlight.transform.localRotation *= Quaternion.Euler(0, selectedHighlight.transform.localRotation.y + 2 * Mathf.Sin(Time.time / 2), 0);
		}
	}

	void ShowRounds()
    {
		if(rounds <= 3)
			roundText.SetText(string.Concat("Round\n", rounds));
		else
			roundText.SetText(string.Empty);
	}
}
