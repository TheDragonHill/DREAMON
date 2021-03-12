using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Needs to be Switched
/// </summary>
public class Belphe : DialogueTrigger
{
    [SerializeField]
    GameObject[] objectsToDeactivate;

    [SerializeField]
    float deathAnimationTime = 6;

    public override void SetPlayerState()
    {
        if(!(minigameManager as HOMinigameManager).isEnd)
            player.motor.StopAgent();
    }

    public override void TheEnd(bool isLose)
    {
        base.TheEnd(isLose);

        Sequence s = DOTween.Sequence();
        // Fly Away

        s.Append(transform.DOScale(Vector3.zero, deathAnimationTime));
        s.Join(transform.DOShakeRotation(deathAnimationTime));
        s.Play();

        if (objectsToDeactivate.Length > 0)
        {
            for (int i = 0; i < objectsToDeactivate.Length; i++)
            {
                objectsToDeactivate[i].SetActive(false);
            }
        }

        Invoke(nameof(SetInactive), deathAnimationTime + 0.5f);

        player.motor.ResumeAgent();
    }

    public override void SetEndState(bool isLose)
    {
        base.SetEndState(isLose);
        TheEnd(isLose);
    }
}
