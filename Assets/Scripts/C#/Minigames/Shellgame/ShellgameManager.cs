using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Net.Security;
using System.Diagnostics.Contracts;

public class ShellgameManager : MiniGame
{
    [SerializeField]
    MinigameManager minigameManager;

    [SerializeField]
    ShellTrigger[] shells;

    [SerializeField]
    TextMeshProUGUI roundText;

    [SerializeField]
    Transform ring;

    [SerializeField]
    float animduration = 1;

    [SerializeField]
    float substractDurationPerRound = 2;

    ShellTrigger rndShell;
    int currentRound = 0;

    bool isLost = true;

    public override void StartMiniGame()
    {
        InitValues();

        ShowRounds();
        ShowRing();
        ResetShells();
        StartAnimation();
        Invoke(nameof(ShellAnimation), 2);
        base.StartMiniGame();
    }

    void InitValues()
    {
        gameObject.SetActive(true);
        currentRound++;
        rndShell = shells[Random.Range(0, shells.Length)];

        if(currentRound > 1)
            animduration -= substractDurationPerRound;
    }

    void StartAnimation()
    {
        SetCollider(false);

        for (int i = 0; i < shells.Length; i++)
        {
            Sequence s = DOTween.Sequence();
            s.Append(shells[i].transform.DOLocalMoveY(1, 1));
            s.Append(shells[i].transform.DOLocalMoveY(0, 1));
            s.Play();
        }
    }

    void ShellAnimation()
    {
        ring.GetComponent<Renderer>().enabled = false;


        StartCoroutine(AnimateShells());
    }

    void SetCollider(bool enabled)
    {
        for (int i = 0; i < shells.Length; i++)
        {
            shells[i].GetComponent<Collider>().enabled = enabled;
        }
    }

    IEnumerator AnimateShells()
    {
        int rndI = 0;
        int rndL = 0;
        Cursor.lockState = CursorLockMode.Locked;

        int moveCount = Random.Range(6, 10);
        float singleduration = animduration / moveCount;


        for (int i = 0; i < moveCount; i++)
        {
            rndI = Random.Range(0, shells.Length);
            rndL = Random.Range(0, shells.Length);

            if(rndI == rndL)
            {
                if (rndL == 0)
                    rndL++;
                else
                    rndL--;
            }

            Sequence s = DOTween.Sequence();
            s.Append(shells[rndI].transform.DOLocalMove(shells[rndI].transform.localPosition + shells[rndI].transform.forward.normalized * 0.4f, singleduration / 2));
            s.Append(shells[rndI].transform.DOLocalMove(shells[rndL].transform.localPosition, singleduration / 2));
            s.Play();

            Sequence o = DOTween.Sequence();
            o.Append(shells[rndL].transform.DOLocalMove(shells[rndL].transform.localPosition - shells[rndL].transform.forward.normalized * 0.4f, singleduration / 2));
            o.Append(shells[rndL].transform.DOLocalMove(shells[rndI].transform.localPosition, singleduration / 2));
            o.Play();

            yield return new WaitForSeconds(singleduration);
        }

        Cursor.lockState = CursorLockMode.None;
        SetCollider(true);
    }


    void ShowRing()
    {
        ring.GetComponent<Renderer>().enabled = true;

        ring.localPosition = new Vector3(rndShell.transform.localPosition.x, ring.localPosition.y, rndShell.transform.localPosition.z);
    }


    public void RevealShell(ShellTrigger shell)
    {
        SetCollider(false);
        ShowRing();

        if(isLost = !rndShell.Equals(shell))
        {
            rndShell.transform.DOLocalMoveY(1, 1);
        }
        shell.transform.DOLocalMoveY(1, 1);

        Invoke(nameof(EndMiniGame), 1);
    }


    protected override void EndMiniGame()
    {
        ResetShells();
        roundText.SetText(string.Empty);
        minigameManager.StartNextDialog(!isLost);
        base.EndMiniGame();
    }

    void ResetShells()
    {
        for (int i = 0; i < shells.Length; i++)
        {
            shells[i].transform.localPosition = new Vector3(shells[i].transform.localPosition.x, 0, shells[i].transform.localPosition.z);
        }
    }


    void ShowRounds()
    {
        if (currentRound <= 3)
            roundText.SetText(string.Concat("Round\n", currentRound));
        else
            roundText.SetText(string.Empty);
    }
}
