using UnityEngine;


/// <summary>
/// Control BetweenText
/// </summary>
public class CallBetweenText : MonoBehaviour
{
    [SerializeField]
    BetweenText betweenText;

    public void CallBetween(TimedTalk[] timedTalk)
    {
        betweenText.SetText(timedTalk);
    }

    public void EndCall()
    {
        betweenText.ClearText();
    }
}
