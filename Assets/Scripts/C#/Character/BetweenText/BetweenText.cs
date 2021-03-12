using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;


/// <summary>
/// Talk with a delay for longer readingtime
/// </summary>
[System.Serializable]
public class TimedTalk : Dialogue.Talk
{
    /// <summary>
    /// Is added to readingtime
    /// </summary>
    public float delay = 0;
}


/// <summary>
/// Shows timed Text between Dialogues
/// </summary>
public class BetweenText : MonoBehaviour
{
    [SerializeField]
    DialogueManager dialogueManager;

    [SerializeField]
    TextMeshProUGUI nameField, textField;

    [SerializeField]
    float timeToReadPerWord = 0.3f;

    [SerializeField]
    float defaultTime = 1.4f;

    List<TimedTalk> currentTalks;

    float oneTextLineSizeY = 0;

    int currentCount = 0;
    string currentName;
    Animator currentAnimator;
    AudioSource currentAudioSource;


    void Awake()
    {
        InitValues();
    }

    void InitValues()
    {
        oneTextLineSizeY = nameField.GetPreferredValues("0").y;
        currentTalks = new List<TimedTalk>();
    }

    /// <summary>
    /// Set text to show on UI
    /// </summary>
    /// <param name="name">Name of Character</param>
    /// <param name="text">Text of Character</param>
    public void SetText(TimedTalk[] talks)
    {
        currentTalks.AddRange(talks);

        if(currentCount <= 0)
        {
            currentCount = currentTalks.Count;
            StopCoroutine(GoThroughDialogues());
            StartCoroutine(GoThroughDialogues());
        }
    }

    /// <summary>
    /// Clear Text after Call ended
    /// </summary>
    public void ClearText()
    {
        StopCoroutine(GoThroughDialogues());
        currentCount = 0;
        currentTalks.Clear();
        dialogueManager.dialogueAnimator.SetBool("IsOpen", false);
    }

    /// <summary>
    /// Iterate through all Dialogues and play at the right time
    /// </summary>
    IEnumerator GoThroughDialogues()
    {
        int i = 0;
        float currentdelay = 0;

        while(i < currentCount)
        {
            if (currentTalks.Count <= 0)
                break;

            // Delay before Betweentext
            yield return new WaitForSeconds(0.5f);


            if (!dialogueManager.CheckIsOpen())
            {
                //currentdelay = timeToRead + currentTalks[i].delay;


                // Play Text
                if (!string.IsNullOrEmpty(currentTalks[i].sentence))
                {
                    // Autocalc delay
                    currentdelay = Regex.Matches(currentTalks[i].sentence, @"\s").Count * timeToReadPerWord;

                    // Add manual delay
                    currentdelay += currentTalks[i].delay;

                    ShowDialogAction(currentTalks[i].dialogAction);
                    ShowText(currentTalks[i].name, currentTalks[i].sentence);
                    StopCoroutine(nameof(CloseDialogue));
                    StartCoroutine(CloseDialogue(currentTalks[i].sentence, currentdelay > 1f ? currentdelay : defaultTime));
                }
                else
                {
                    currentdelay = defaultTime;
                }

                // Play Animation
                if (!string.IsNullOrEmpty(currentTalks[i].animation.AnimationStateName))
                    ShowAnimation(currentTalks[i].animation.AnimationStateName, currentTalks[i].animation.animator);

                // Play Audio
                if (currentTalks[i].audio.clip)
                    PlayAudio(currentTalks[i].audio.source, currentTalks[i].audio.clip);

                
                // Remove Talk to play next one
                currentTalks.RemoveAt(i);



                yield return new WaitForSeconds(currentdelay);
                
                // Set new Count
                currentCount = currentTalks.Count;
            }
            else
            {
                // Check for Dialog is closed and ready for waiting Talks
                yield return new WaitForSeconds(1);
            }
        }

        currentTalks.Clear();
    }

    void ShowDialogAction(DialogAction action)
    {
        if (action)
            action.DoAction();
    }

    /// <summary>
    /// Show Text on Textfield
    /// </summary>
    void ShowText(string name, string text)
    {
        StopCoroutine(TypeSentence(string.Empty));

        dialogueManager.DisableButtons();
        dialogueManager.dialogueAnimator.SetBool("IsOpen", true);
        
        if (!string.IsNullOrEmpty(name))
            currentName = name;

        if(!string.IsNullOrEmpty(currentName))
            nameField.SetText(currentName);

        if(!string.IsNullOrEmpty(text))
            StartCoroutine(TypeSentence(text));

        //StartCoroutine(CloseDialogue(text));
    }

    /// <summary>
    /// Show an Animation
    /// </summary>
    /// <param name="animationState">State of the animated Object</param>
    /// <param name="animator">Animator</param>
    void ShowAnimation(string animationState, Animator animator)
    {
        if (animator)
            currentAnimator = animator;

        if (currentAnimator)
            currentAnimator.CrossFade(animationState, 0.3f);
    }

    /// <summary>
    /// Play audio at a specific Source
    /// </summary>
    /// <param name="source">Audiosource</param>
    /// <param name="clip">Audioclip</param>
    void PlayAudio(AudioSource source, AudioClip clip)
    {
        if (source)
            currentAudioSource = source;

        if (currentAudioSource)
        {
            currentAudioSource.clip = clip;
            currentAudioSource.Play();
        }
    }

    /// <summary>
    /// Animates the words
    /// </summary>
    /// <param name="sentence"></param>
    /// <returns></returns>
    IEnumerator TypeSentence(string sentence)
    {
        textField.text = string.Empty;

        int lastlineCount = 1;

        for (int i = 0; i < sentence.ToCharArray().Length; i++)
        {
            if (dialogueManager.isTyping || !dialogueManager.CheckIsOpen())
                break;

            textField.text += sentence.ToCharArray()[i];


            // Check for new lines
            if (lastlineCount < textField.textInfo.lineCount)
            {
                textField.ForceMeshUpdate();

                // Resize Dialog rect to match the lines in Text
                textField.rectTransform.sizeDelta = new Vector2(textField.rectTransform.sizeDelta.x, oneTextLineSizeY * (textField.textInfo.lineCount + 1));
                lastlineCount = textField.textInfo.lineCount;
            }

            yield return new WaitForSeconds(0.02f);
        }
    }

    /// <summary>
    /// Clear text after a specific amount of time
    /// </summary>
    IEnumerator CloseDialogue(string text, float readingTime)
    {
        yield return new WaitForSeconds(readingTime);


        if (textField.text.Equals(text))
            dialogueManager.dialogueAnimator.SetBool("IsOpen", false);
    }
}
