using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextShowSlowly : MonoBehaviour
{
    [SerializeField]
    float duration = 6;

    TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

        ShowColorSlow();
    }

    void ShowColorSlow()
    {
        Color oldColor = text.color;

        text.color = Color.clear;

        text.DOColor(oldColor, duration);
    }
}
