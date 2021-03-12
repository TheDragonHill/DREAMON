using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorMultipleImages : OverButton
{
    [SerializeField]
    Image[] images;

    [SerializeField]
    float time = 3.7f;

    Button button;
    Color normalColor;

    protected override void Initialise()
    {
        base.Initialise();
        button = GetComponent<Button>();
        normalColor = images[0].color;
    }

    protected override void OnButton()
    {
        base.OnButton();
        DoColor(button.colors.highlightedColor);
    }

    protected override void ButtonClicked()
    {
        base.ButtonClicked();
        SetColor(button.colors.pressedColor);
    }

    protected override void ButtonExit()
    {
        base.ButtonExit();
        DoColor(normalColor);
    }

    private void OnDisable()
    {
        DoColor(normalColor);
    }

    void DoColor(Color color)
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].DOColor(color, time).SetUpdate(true);
        }
    }

    void SetColor(Color color)
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = color;
        }
    }
}
