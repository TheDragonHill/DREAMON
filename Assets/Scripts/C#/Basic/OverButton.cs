using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// Parentclass to check if mousecursor is hovering over a Button
/// </summary>
[RequireComponent(typeof(Button))]
public class OverButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IPointerExitHandler
{
    protected void Start()
    {
        Initialise();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnButton();
    }

    public void OnSelect(BaseEventData eventData)
    {
        OnButton();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ButtonExit();
    }


    protected virtual void Initialise()
    {
        GetComponent<Button>().onClick.AddListener(() => ButtonClicked());
    }

    /// <summary>
    /// Button is clicked
    /// </summary>
    protected virtual void ButtonClicked()
    {

    }

    /// <summary>
    /// Mouse is over button
    /// </summary>
    protected virtual void OnButton()
    {

    }

    /// <summary>
    /// Mouse is no longer on button
    /// </summary>
    protected virtual void ButtonExit()
    {

    }
}