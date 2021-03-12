using UnityEngine;

/// <summary>
/// Set Credits outside of the Canvas View
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class CreditText : MonoBehaviour
{
    [SerializeField]
    RectTransform canvasRect;

    [SerializeField]
    bool validate = false;

    RectTransform rect;

    void Start()
    {
        SetUpCreditText();
    }

    private void OnValidate()
    {
        SetUpCreditText();
    }

    void SetUpCreditText()
    {
        if(rect == null)
            rect = GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -(canvasRect.rect.height + rect.rect.height) / 2);
    }
}
