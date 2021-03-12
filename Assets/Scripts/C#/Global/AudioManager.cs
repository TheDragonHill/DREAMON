using UnityEngine;
using UnityEngine.Audio;
using System.Linq;
using System.Collections;

/// <summary>
/// Manager of all Audio
/// </summary>
[RequireComponent(typeof(AudioSource), typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField]
    AudioMixer mainMixer;

    public string[] volumeStrings;

    AudioSource[] sources;

    private void Awake()
    {
        MakeSingelton();
        Initialise();
    }


    /// <summary>
    /// This Object should only be once in the Game
    /// </summary>
    void MakeSingelton()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Initialise()
    {
        sources = GetComponents<AudioSource>();
    }

    /// <summary>
    /// Play Clip on Audiosource that isn't going to destroy on Scenechange
    /// </summary>
    /// <param name="c">AudioClip that is going to be played</param>
    public void TakeAudioToNextScene(AudioClip c)
    {
        sources.Last().clip = c;
        sources.Last().Play();
    }

    /// <summary>
    /// Get the current volumesetting
    /// </summary>
    /// <param name="index">Index of Volume in volumeStrings</param>
    public float GetCurrentVolume(int index)
    {
        float currentVolume = 0;

        mainMixer.GetFloat(volumeStrings[index], out currentVolume);

        return currentVolume;
    }

    /// <summary>
    /// Set the current volumesetting
    /// </summary>
    /// <param name="index">Index of Volume in volumeStrings</param>
    /// <param name="volume">Volume its going to be</param>
    public void SetCurrentVolume(int index, float volume)
    {
        // Mute Volume, I could still hear it
        if (volume <= -40)
            volume = -80;

        mainMixer.SetFloat(volumeStrings[index], volume);
    }

    /// <summary>
    /// Set the current Volume of the source
    /// </summary>
    /// <param name="index">Index of volume in sources</param>
    /// <param name="volume">Volume to be set</param>
    public void SetSourceVome(int index, float volume)
    {
        sources[index].volume = volume;
    }

    /// <summary>
    /// Get the current volume of the source
    /// </summary>
    public float GetSourcVolume(int index)
    {
        return sources[index].volume;
    }

    /// <summary>
    /// Set the current Backgroundmusic
    /// </summary>
    /// <param name="clip">New Backgroundmusic</param>
    public void SetSourceClip(AudioClip clip, int index = 0, int timeSamples = 0)
    {
        if (!clip)
        {
            sources[index].Stop();
            return;
        }

        if (timeSamples > 0)
            sources[index].timeSamples = timeSamples;

        sources[index].clip = clip;
        sources[index].Play();
    }

    public AudioClip GetSourceClip(int index = 0)
    {
        if (!sources[index].clip)
            return null;

        return sources[index].clip;

    }

    public void PitchManual(float pitch, params int[] indices)
    {
        for (int i = 0; i < indices.Length; i++)
        {
            if(sources[i])
                sources[i].pitch = pitch;
        }
    }

    /// <summary>
    /// Compare clip with all clips in sources
    /// </summary>
    public bool CompareClip(AudioClip clip)
    {

        for (int i = 0; i < sources.Length; i++)
        {
            if (sources[i].clip && sources[i].clip.Equals(clip))
                return true;
        }

        return false;
    }

    public int GetSamples(int index)
    {
        if (!sources[index])
            return 0;

        return sources[index].timeSamples;
    }
}
