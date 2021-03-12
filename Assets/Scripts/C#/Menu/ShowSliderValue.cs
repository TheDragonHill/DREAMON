using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <author>Christian</author>
/// <summary>
/// Show Value of Slider in a Textfield as a percentage Number
/// </summary>
[RequireComponent(typeof(Slider))]
public class ShowSliderValue : MonoBehaviour
{
    /// <summary>
    /// Textfield where the Value is shown
    /// </summary>
    [SerializeField] TextMeshProUGUI sliderTextfield;

    Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();

        slider.onValueChanged.AddListener((float value) => ChangeSliderValue(value));
        ChangeSliderValue(slider.value);
    }

    /// <summary>
    /// Change the value of the Textfield
    /// </summary>
    /// <param name="value">Changed Value</param>
    void ChangeSliderValue(float value)
    {
        sliderTextfield.text = Mathf.RoundToInt(CalculateValueToPercentage(value)).ToString();
    }

    /// <summary>
    /// Calcute value to percentage by Min- and Maxvalue of Slider
    /// </summary>
    float CalculateValueToPercentage(float value)
    {
        return ((value - slider.minValue) * 100) / (slider.maxValue - slider.minValue);
    }
}
