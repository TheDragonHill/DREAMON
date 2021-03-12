using UnityEngine;

/// <summary>
/// Set new Backgroundmusic on Sceneload
/// </summary>
public class ChangeBackgroundMusic : MonoBehaviour
{
    [SerializeField]
    AudioClip newMusic;

    [SerializeField]
    AudioClip FXSoundClip;

    void Start()
    {
        SetClip(newMusic);
        SetClip(FXSoundClip, 1);
    }

    /// <summary>
    /// Set Audioclip in Audiosources of AudioManager
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="index"></param>
    void SetClip(AudioClip clip, int index = 0)
    {
        if(AudioManager.Instance)
            if (clip)
                if(!AudioManager.Instance.CompareClip(clip))
                    AudioManager.Instance.SetSourceClip(clip, index);
    }
}
