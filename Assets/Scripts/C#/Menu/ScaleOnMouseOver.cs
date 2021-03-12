using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleOnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    [SerializeField]
    float scaleSpeed = 0.18f;

    [SerializeField]
    Vector3 scaleVector = new Vector3(1.07f, 1.07f, 1);

    RectTransform recT;

    bool scaled = false;

    void Start()
    {
        recT = GetComponent<RectTransform>();
    }

    /// <summary>
    /// For better Visualisation scaling UI Element on Mouse over
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        scaled = false;
        ScaleButton();

    }

    /// <summary>
    /// Rescaling UI Element on Mouse exit
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        scaled = true;
        ScaleButton();

    }

    void ScaleButton()
    {

        if(scaled)
        {
            recT.DOKill();
            recT.localScale = Vector3.one;
        }
        else
        {
            recT.DOKill();
            recT.DOScale(scaleVector, scaleSpeed).SetUpdate(true);
        }
    }

    void OnDisable()
    {
        if(recT)
        {
            recT.DOKill();
            recT.localScale = Vector3.one;
        }
    }
}
