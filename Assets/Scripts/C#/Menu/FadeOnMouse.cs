using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;


[RequireComponent(typeof(TextMeshProUGUI))]
public class FadeOnMouse : MonoBehaviour
{
    [SerializeField]
    HideMouseTimer mouseTimer;

    [SerializeField]
    float fadeDuration = 1;

    TextMeshProUGUI text;
    float nextAlpha = 1;

    // Start is called before the first frame update
    void Start()
    {
        mouseTimer.OnMouseShow += FadeText;
        text = GetComponent<TextMeshProUGUI>();
        text.alpha = 0;
    }

    void FadeText()
    {
        if(Cursor.visible)
        {
            nextAlpha = 1;
        }
        else
        {
            nextAlpha = 0;
        }
        text.DOFade(nextAlpha, fadeDuration);
    }
}
