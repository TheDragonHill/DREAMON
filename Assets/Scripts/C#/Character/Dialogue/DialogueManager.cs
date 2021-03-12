using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages the current Dialogue and Minigame
/// </summary>
public class DialogueManager : MonoBehaviour
{
    /// <summary>
    /// Time to wait between letters
    /// </summary>
    [SerializeField]
    float timeForTextInSeconds = 0.5f;

    /// <summary>
    /// Textfield for the Name of the Character
    /// </summary>
    public TextMeshProUGUI nameText;

    /// <summary>
    /// Textfield for the spoken Words of the Character
    /// </summary>
    public TextMeshProUGUI dialogueText;


    /// <summary>
    /// Button to press to continue the Dialogue
    /// </summary>
    public GameObject continueButton;

    /// <summary>
    /// Button to press to end the Dialogue
    /// </summary>
    public GameObject endButton;

    /// <summary>
    /// Parentobject of decisionsButtons
    /// </summary>
    public GameObject decisions;

    /// <summary>
    /// Buttons to select a different Option
    /// </summary>
    public UnityEngine.UI.Button[] decisionsButtons = new UnityEngine.UI.Button[3];

    /// <summary>
    /// Set the selected Decision of the decisionButton
    /// </summary>
    public int selectedOpinion = 0;

    /// <summary>
    /// Current running dialogue
    /// </summary>
    public DialogueTrigger currentDialogObject;

    /// <summary>
    /// Current minigame
    /// </summary>
    public MinigameManager minigameManager;
    /// <summary>
    /// Should there play a minigame after dialogue?
    /// </summary>
    public bool selectMinigame;

    /// <summary>
    /// Animator to open and close the dialogue
    /// </summary>
    public Animator dialogueAnimator;

    /// <summary>
    /// PlayerController of player
    /// </summary>
    public PlayerController player;

    /// <summary>
    /// Controller of Camera
    /// </summary>
    public CameraController cameraController;

    /// <summary>
    /// Is a Dialogue currently typing in dialoguefield?
    /// </summary>
    public bool isTyping = false;

    /// <summary>
    /// Is the Dialogue at the end?
    /// </summary>
    bool end = false;

    // Queues for dialogue object
    Queue<string> sentences;
    Queue<Transform> positions;
    Queue<string> names;
    Queue<AnimationObject> animations;
    Queue<AudioObject> audios;
    Queue<DialogAction> actions;

    /// <summary>
    /// First Animator in Dialogue to play Animations while speaking
    /// </summary>
    Animator currentAnimator;

    /// <summary>
    /// Second Animator in Dialogue to play Animations while speaking
    /// </summary>
    Animator secondCurrentAnimator;

    /// <summary>
    /// Save the size of one TextLine
    /// </summary>
    float oneTextLineSizeY = 0;

    /// <summary>
    /// Init Values
    /// </summary>
    void Awake()
    {
        sentences = new Queue<string>();
        positions = new Queue<Transform>();
        names = new Queue<string>();
        animations = new Queue<AnimationObject>();
        audios = new Queue<AudioObject>();
        actions = new Queue<DialogAction>();
        oneTextLineSizeY = nameText.GetPreferredValues("0").y;
        cameraController = Camera.main.GetComponent<CameraController>();
        player = FindObjectOfType<PlayerController>();
    }


    /// <summary>
    /// Starts the dialog by triggering it from the DialogueTrigger
    /// </summary>
    /// <param name="currentTrigger"></param>
    public void StartDialogue (DialogueTrigger currentTrigger)
    {
        currentDialogObject = currentTrigger;

        // Setup Values for Dialogue
        SetupValues();
        SetupCamera();
        RotatePlayer();
        currentDialogObject.SetPlayerState();


        //Opens the first dialog option
        Dialogue.Option option = currentTrigger.dialogue[currentTrigger.currentDialogue].option[0];

        // Set Decisions
        ActivateDecisions(ref option);
        SetDecisions(ref option);

        //Opens the dialog box
        dialogueAnimator.SetBool("IsOpen", true);

        FillQueues(option);

        end = option.endSentence;

        ResetDecisionButtons();
        SetupMinigame(ref option);
        
        DisplayNextSentence();
    }

    #region StartDialog() Methods

    /// <summary>
    /// Set Values on Start Dialogue
    /// </summary>
    private void SetupValues()
    {
        UnityEngine.Cursor.visible = true;

        //Reset the Buttons
        DisableButtons(true);

        continueButton.SetActive(true);
        decisions.SetActive(false);
    }

    /// <summary>
    /// Change Cameraposition to fit the Dialogue needs
    /// </summary>
    private void SetupCamera()
    {
        if (currentDialogObject.camPosition != null)
        {
            cameraController.MoveToFixedPosition(currentDialogObject.camPosition.position, currentDialogObject.transform);
        }
        else
        {
            // Set up Camera between Dialogtrigger, Camera and Player
            cameraController.MoveToFixedPosition(Vector3.Lerp(player.facePoint.position, Vector3.Lerp(currentDialogObject.transform.position, cameraController.transform.position, 0.1f), 0.2f), currentDialogObject.transform);
        }
    }

    /// <summary>
    /// Rotate Player to interactionTransform
    /// </summary>
    private void RotatePlayer()
    {
        if (currentDialogObject.interactionTransform != currentDialogObject.transform)
        {
            // Rotate Player
            player.motor.RotatePlayerTo(currentDialogObject.transform);
        }
    }

    /// <summary>
    /// Checks the number of options and then activates the corresponding buttons
    /// </summary>
    private void ActivateDecisions(ref Dialogue.Option option)
    {
        if (option.decisions.Length != decisionsButtons.Length && option.decisions.Length != 0)
        {
            if (option.decisions.Length == 2)
            {
                decisionsButtons[1].gameObject.SetActive(true);
                decisionsButtons[2].gameObject.SetActive(false);
            }
            else if (option.decisions.Length == 1)
            {
                decisionsButtons[1].gameObject.SetActive(false);
                decisionsButtons[2].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Changes the corresponding values of the buttons
    /// </summary>
    private void SetDecisions(ref Dialogue.Option option)
    {
        for (int i = 0; i < option.decisions.Length; i++)
        {
            //Set text
            decisionsButtons[i].GetComponentInChildren<TextMeshProUGUI>().SetText(option.decisions[i].ToString());

            //Set decision number
            decisionsButtons[i].GetComponent<DecisionButtons>().decisionNumber = option.nextDecisions[i];
        }
    }

    /// <summary>
    /// SetupButtons to select new options
    /// </summary>
    private void ResetDecisionButtons()
    {
        for (int i = 0; i < decisionsButtons.Length; i++)
        {
            decisionsButtons[i].onClick.RemoveAllListeners();
            decisionsButtons[i].onClick.AddListener(() => SelectOption(currentDialogObject.dialogue[currentDialogObject.currentDialogue]));
        }
    }

    /// <summary>
    /// Set Minigame
    /// </summary>
    private void SetupMinigame(ref Dialogue.Option option)
    {
        if (end == true && option.nextMinigame)
        {
            selectMinigame = option.nextMinigame;
            minigameManager = currentDialogObject.minigameManager;
        }
    }

    #endregion

    /// <summary>
    /// Opens the further dialogues
    /// </summary>
    /// <param name="dialogue"></param>
    /// <param name="currentTrigger"></param>
    public void SelectOption(Dialogue dialogue)
    {
        //Reset the Buttons
        continueButton.SetActive(true);
        decisions.SetActive(false);

        //Opens the next correct dialog option
        Dialogue.Option option = dialogue.option[selectedOpinion];


        ActivateDecisions(ref option);

        SetDecisions(ref option);

        //Opens the dialog box
        dialogueAnimator.SetBool("IsOpen", true);

        FillQueues(option);

        end = option.endSentence;

        //Checks whether the dialog is over to start a mini-game
        SetupMinigame(ref option);

        DisplayNextSentence();
    }

    /// <summary>
    /// Displays the next sentence
    /// </summary>
    public void DisplayNextSentence()
    {
        DisableButtons();
        //ActivateEndButtons();

        // Camera
        SetLine(positions.Dequeue());

        // Animation
        SetLine(animations.Dequeue());

        StopCoroutine(WaitForCam());
        StartCoroutine(WaitForCam());
        
    }

    IEnumerator WaitForCam()
    {
        yield return new WaitWhile(() => cameraController.onLookAtLerp);


        // Set Charactername
        SetLine(names.Dequeue());
        // Audio
        SetLine(audios.Dequeue());
        // Action
        SetLine(actions.Dequeue());

        // Stop Typing
        StopCoroutine(nameof(TypeSentence));
        // Start typing
        StartCoroutine(TypeSentence(sentences.Dequeue()));

        ActivateEndButtons();
    }

    #region SetLine() Methods

    void SetLine(string name)
    {
        if (!nameText.text.Equals(name))
        {
            nameText.SetText(name);
        }
    }

    void SetLine(Transform cameraTarget)
    {
        if (cameraTarget && !cameraTarget.Equals(cameraController.target))
        {
            cameraController.LerpLookAt(cameraTarget);
        }
    }

    void SetLine(AudioObject audio)
    {
        if (audio.clip && audio.source)
        {
            // Clip Audio for Dialogue
            audio.source.clip = audio.clip;
            // Play Audio
            audio.source.Play();
        }
    }

    void SetLine(AnimationObject animation)
    {
        if (!string.IsNullOrEmpty(animation.AnimationStateName))
        {
            if (animation.animator)
                currentAnimator = animation.animator;

            if (currentAnimator)
                currentAnimator.CrossFade(animation.AnimationStateName, 0.3f);
        }

        // Second Animation
        if (!string.IsNullOrEmpty(animation.SecondAnimationStateName))
        {
            if (animation.secondAnimator)
                secondCurrentAnimator = animation.secondAnimator;

            if (secondCurrentAnimator)
                secondCurrentAnimator.CrossFade(animation.SecondAnimationStateName, 0.3f);
        }
    }

    void SetLine(DialogAction action)
    {
        if(action)
        {
            action.DoAction();
        }
    }

    private void ActivateEndButtons()
    {
        if (end == true && sentences.Count <= 1)
        {
            continueButton.SetActive(false);
            decisions.SetActive(false);
            endButton.SetActive(true);
        }
        else if (end == false && sentences.Count <= 1)
        {
            continueButton.SetActive(false);
            decisions.SetActive(true);
        }
        else
        {
            continueButton.SetActive(true);
            decisions.SetActive(false);
        }
    }

    #endregion

    /// <summary>
    /// Animates the words
    /// </summary>
    /// <param name="sentence"></param>
    /// <returns></returns>
    IEnumerator TypeSentence(string sentence)
    {
        int lastlineCount = 1;
        isTyping = true;
        dialogueText.text = string.Empty;

        for (int i = 0; i < sentence.ToCharArray().Length; i++)
        {

            if (!CheckIsOpen() && !isTyping)
                break;

            dialogueText.text += sentence.ToCharArray()[i];

            // Check for new lines
            if (lastlineCount < dialogueText.textInfo.lineCount)
            {
                dialogueText.ForceMeshUpdate();

                // Resize Dialog rect to match the lines in Text
                dialogueText.rectTransform.sizeDelta = new Vector2(dialogueText.rectTransform.sizeDelta.x, oneTextLineSizeY * (dialogueText.textInfo.lineCount + 1));
                lastlineCount = dialogueText.textInfo.lineCount;
            }

            yield return new WaitForSeconds(timeForTextInSeconds);
        }

        isTyping = false;
    }

    /// <summary>
    /// Disable all Buttons except continue
    /// </summary>
    /// <param name="enabled"></param>
    public void DisableButtons(bool enabled = false)
    {
        decisions.SetActive(enabled);
        continueButton.gameObject.SetActive(enabled);
        endButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Ends the dialog
    /// </summary>
    public void EndDialogue()
    {
        StopCoroutine(nameof(TypeSentence));
        dialogueAnimator.SetBool("IsOpen", false);

        //Stop focusing any objects
        player.RemoveFocus();

        //Reset the Buttons
        DisableButtons();

        SetLine("");
        StartCoroutine(TypeSentence(""));

        //Starts Minigame
        if (selectMinigame)
        {
            StartMinigame();
        }
        else
        {
            ResetDialogueValues();
        }
    }

   
    /// <summary>
    /// Start Minigame
    /// </summary>
    private void StartMinigame()
    {
        player.motor.StopAgent();
        selectMinigame = false;
        minigameManager.StartNewMinigame();
    }

    /// <summary>
    /// Reset all Values to let the Player play
    /// </summary>
    private void ResetDialogueValues()
    {
        UnityEngine.Cursor.visible = false;
        cameraController.MoveToOffset(player.transform);
        cameraController.StartResetCameraToPlayer();
        player.motor.ResumeAgent();
        nameText.text = string.Empty;
        

        // Call End of Dialogue
        SetDialogueEnd();

        currentDialogObject = null;
    }

    /// <summary>
    /// Call End of the current Dialogue
    /// </summary>
    void SetDialogueEnd()
    {
        if (currentDialogObject)
        {
            if (currentDialogObject.isLost == true)
            {
                currentDialogObject.TheEnd(true);
            }
            else
            {
                currentDialogObject.TheEnd(false);
            }
        }
    }



    /// <summary>
    /// Check if Dialogue is currently visible
    /// </summary>
    public bool CheckIsOpen()
    {
        return dialogueAnimator.GetBool("IsOpen");
    }

    /// <summary>
    /// Check for running Dialogue
    /// </summary>
    /// <returns>Returns true if current a dialogue is running</returns>
    public bool CheckIsDialogue()
    {
        return currentDialogObject;
    }


    /// <summary>
    /// Fill Queues to start the Talk
    /// </summary>
    void FillQueues(Dialogue.Option option)
    {
        // Clean up
        ClearQueues();

        // Go through Talks and line up queues
        for (int i = 0; i < option.talks.Length; i++)
        {
            actions.Enqueue(option.talks[i].dialogAction);
            audios.Enqueue(option.talks[i].audio);
            animations.Enqueue(option.talks[i].animation);
            names.Enqueue(option.talks[i].name);
            positions.Enqueue(option.talks[i].cameraTarget);
            sentences.Enqueue(option.talks[i].sentence);
        }
    }

    /// <summary>
    /// Clear all Queues
    /// </summary>
    void ClearQueues()
    {
        sentences.Clear();
        names.Clear();
        positions.Clear();
        animations.Clear();
        audios.Clear();
        actions.Clear();
    }
}
