using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Dayak : DialogueTrigger
{
    [SerializeField]
    GameObject[] objectsToDeactivate;

    [SerializeField]
    float deathAnimationTime = 6;

    [SerializeField]
    AudioClip dayakTheme;

    AudioClip oldBackgroundMusic;

    [SerializeField]
    Animator doorAnimator;

    [SerializeField]
    string doorAnimationState;

    [SerializeField]
    AudioSource doorSource;

    [SerializeField]
    AudioClip deathSound;

    [SerializeField]
    string deathScene;

    protected override void PlaySound()
    {
        base.PlaySound();

        if(AudioManager.Instance && dayakTheme)
        {
            oldBackgroundMusic = AudioManager.Instance.GetSourceClip();
            AudioManager.Instance.SetSourceClip(dayakTheme);
        }
    }

    public override void TheEnd(bool isLose)
    {

        // Lost:
        if(isLose)
        {
            // Musik und Ambient geht aus
            SetMusicOff();
            // Deathsound wird gespielt
            PlayDeathSound();
            // Deathscreen kommt
            // Deathscreen führt zu Credits
            LoadDeathScreen();
        }
        else
        {
            // Win:
            // Dayak verschwindet
            // Kerzen gehen mit ihm aus
            Disappear();
            // Tor geht auf
            OpenDoors();
            // Musik geht aus
            SetMusicOff();
            base.TheEnd(isLose);
        }

        if (objectsToDeactivate.Length > 0)
        {
            for (int i = 0; i < objectsToDeactivate.Length; i++)
            {
                objectsToDeactivate[i].SetActive(false);
            }
        }
        Invoke(nameof(SetInactive), deathAnimationTime + 0.5f);
    }

    private void LoadDeathScreen()
    {
        SceneManager.LoadScene(deathScene, LoadSceneMode.Additive);
    }

    void Disappear()
    {
        Sequence s = DOTween.Sequence();

        // Fly Away
        s.Append(transform.DOScale(Vector3.zero, deathAnimationTime));
        s.Play();
    }

    void OpenDoors()
    {
        doorAnimator.Play(doorAnimationState);
        doorSource.Play();
    }

    void SetMusicOff()
    {
        if (AudioManager.Instance)
        {
            AudioManager.Instance.SetSourceClip(null);
            AudioManager.Instance.SetSourceClip(null, 1);
        }
    }

    void PlayDeathSound()
    {
        if(AudioManager.Instance)
        {
            AudioManager.Instance.TakeAudioToNextScene(deathSound);
        }
    }

    public override void SetEndState(bool isLose)
    {
        base.SetEndState(isLose);
        Disappear();
        OpenDoors();
    }
}
