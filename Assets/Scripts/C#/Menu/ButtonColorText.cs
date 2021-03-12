using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonColorText : OverButton
{
    [SerializeField]
    TextMeshProUGUI text;

    [SerializeField]
    float time = 0.2f;

    Button button;

    protected override void Initialise()
    {
        base.Initialise();
        button = GetComponent<Button>();
    }

    protected override void OnButton()
    {
        base.OnButton();
        text.DOColor(button.colors.highlightedColor, time).SetUpdate(true);
    }

    protected override void ButtonClicked()
    {
        base.ButtonClicked();
        text.color = button.colors.pressedColor;
    }

    protected override void ButtonExit()
    {
        base.ButtonExit();
        text.DOColor(button.colors.normalColor, time).SetUpdate(true);
    }

    private void OnDisable()
    {
        if(button)
            text.DOColor(button.colors.normalColor, time).SetUpdate(true);
    }
}