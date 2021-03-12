using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Image))]
public class MoveUpOnMouseButton : OutlineObject
{
    [SerializeField]
    float positionSpeed = 0.18f;

    [SerializeField]
    Vector2 addedVector = new Vector2(0, 5f);

    [SerializeField]
    RectTransform recT;

    [SerializeField]
    GameObject placeablePrefab;

    [SerializeField]
    GameObject toSetActiv;

    [SerializeField]
    Transform parentObject;

    [SerializeField]
    TextMeshProUGUI infoText;

    [SerializeField]
    TextMeshProUGUI secondinfoText;

    [TextArea(6, 12)]
    [SerializeField]
    string text;

    [TextArea(6, 12)]
    [SerializeField]
    string secondText;

    Canvas myCanvas;

    Vector2 targetVector;

    Vector2 previousPosition;

    RectTransform ownRectTransform;
    Vector2 ownPreviosPosition;
    bool isDragging = false;



    void Start()
    {
        GetComponent<Image>().color = new Color(0, 0, 0, 0);
        previousPosition = recT.anchoredPosition;
        targetVector = recT.anchoredPosition + addedVector;
        ownRectTransform = GetComponent<RectTransform>();
        ownPreviosPosition = ownRectTransform.anchoredPosition;
        myCanvas = GetComponentInParent<Canvas>();

    }

    public void OnMouseEnter(PointerEventData eventData)
    {
        StartCoroutine(LerpVector(recT, targetVector, positionSpeed));
    }

    public void OnMouseExit(PointerEventData eventData)
    {
        StartCoroutine(LerpVector(recT, previousPosition, positionSpeed));
    }

    public void OnMouseDown(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnMouseUp(PointerEventData eventData)
    {
        //if (isDragging && GameValueManager.instance.playerController != null)
        //{
        //    GameValueManager.instance.playerController.dragging = false;
        //    GameValueManager.instance.playerController.unit = null;
        //}
        isDragging = false;
    }

    /// <summary>
    /// Lerp the localScale to targetScale
    /// </summary>
    IEnumerator LerpVector(RectTransform currentRect, Vector3 targetPosition, float time)
    {
        float lerpvalue = 0;
        Vector3 lerpPosition = currentRect.anchoredPosition;

        while (lerpvalue < 1)
        {
            lerpvalue += Time.deltaTime / time;
            currentRect.anchoredPosition = Vector3.Lerp(lerpPosition, targetPosition, lerpvalue);
            yield return new WaitForEndOfFrame();
        }
    }
}
