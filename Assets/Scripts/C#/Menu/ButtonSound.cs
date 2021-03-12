using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Play Sound on Button Mouse over and Click
/// </summary>
[RequireComponent(typeof(AudioSource), typeof(Button))]
public class ButtonSound : OverButton
{
    AudioSource source;

    [SerializeField]
    AudioClip clickAudio, hoverAudio;

    protected override void Initialise()
    {
        source = GetComponent<AudioSource>();
        GetComponent<Button>().onClick.AddListener(() => ButtonClicked());

        source.playOnAwake = false;
    }


    protected override void ButtonClicked()
    {
        AudioManager.Instance?.TakeAudioToNextScene(clickAudio);
    }


    protected override void OnButton()
    {
        source.clip = hoverAudio;
        source.Play();
    }
}
