using DG.Tweening;
using UnityEngine;

public class ElevatorScroll : MonoBehaviour
{
    [SerializeField]
    RectTransform canvasRect;

    [SerializeField]
    RectTransform transformToElevate;

    [SerializeField]
    float time = 20;

    [SerializeField]
    float delay = 0;

    private void Start()
    {
        Invoke(nameof(Elevate), delay);
    }

    void Elevate()
    {
        transformToElevate.DOAnchorPosY((canvasRect.rect.height + transformToElevate.rect.height) / 2, time);
    }
}
