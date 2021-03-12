using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
    [SerializeField]
    float minRange = 0.4f;

    [SerializeField]
    float maxRange = 2;

    [SerializeField]
    float duration = 5;

    [SerializeField]
    float maxStartingTime = 1;

    float range;

    private void OnEnable()
    {
        range = Random.Range(minRange, maxRange);
        if (Random.Range(-1f, 1f) <= 0)
            range *= -1;

        duration = Random.Range(duration - 1, duration);
        InvokeRepeating(nameof(Move), Random.Range(0.4f, maxStartingTime), duration);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Move));
    }

    void Move()
    {
        transform.DOLocalMoveY(transform.localPosition.y + range, duration);
        range *= -1;
    }
}
