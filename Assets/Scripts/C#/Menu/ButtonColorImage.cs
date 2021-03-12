using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorImage : OverButton
{
    [SerializeField]
    Image image;

    [SerializeField]
    float time = 3.7f;

    Button button;
    Color normalColor;

    protected override void Initialise()
    {
        base.Initialise();
        button = GetComponent<Button>();
        normalColor = image.color;
    }

    protected override void OnButton()
    {
        base.OnButton();
        image.DOColor(button.colors.highlightedColor, time).SetUpdate(true);
    }

    protected override void ButtonClicked()
    {
        base.ButtonClicked();
        image.color = button.colors.pressedColor;
    }

    protected override void ButtonExit()
    {
        base.ButtonExit();
        image.DOColor(normalColor, time).SetUpdate(true);
    }
}
