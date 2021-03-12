using UnityEngine;


/// <summary>
/// Creates a Zone where the Player can 
/// hear 2D Backgroundmusic only in this Zone
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class AudioZone : MonoBehaviour
{
    [SerializeField]
    AudioClip BackGroundMusicClip;

    [SerializeField]
    AudioClip FXSoundClip;

    [Range(0, 1)]
    [SerializeField]
    float volumeFactor = 0.8f;

    int currentBackTimeSamples;
    int currentFXTimeSamples;

    Transform player;
    float radius;

    void Start()
    {
        GetComponent<Collider>().isTrigger = true;
        radius = GetComponent<SphereCollider>().radius * 0.01f;
    }


    /// <summary>
    /// Set Background & FXclip to this Clip if not already set
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (AudioManager.Instance)
        {
            if(other.GetComponent<PlayerController>())
            {
                player = other.transform;
                if (BackGroundMusicClip && !AudioManager.Instance.CompareClip(BackGroundMusicClip))
                {
                    AudioManager.Instance.SetSourceClip(BackGroundMusicClip, 0, currentBackTimeSamples);
                }
                if (FXSoundClip && !AudioManager.Instance.CompareClip(FXSoundClip))
                {
                    AudioManager.Instance.SetSourceClip(FXSoundClip, 1, currentFXTimeSamples);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (AudioManager.Instance && player)
        {
            float newVolume = Mathf.Clamp01(volumeFactor / ((Vector3.Distance(transform.position, player.position) / radius) * Time.deltaTime));

            AudioManager.Instance.SetSourceVome(0, newVolume);
            AudioManager.Instance.SetSourceVome(1, newVolume);
        }
    }

    /// <summary>
    /// Set clips to no clip on exit zone
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (AudioManager.Instance)
        {
            if (other.GetComponent<PlayerController>())
            {
                currentBackTimeSamples = AudioManager.Instance.GetSamples(0);
                currentFXTimeSamples = AudioManager.Instance.GetSamples(1);

                if (AudioManager.Instance.CompareClip(BackGroundMusicClip))
                    AudioManager.Instance.SetSourceClip(null);

                if (AudioManager.Instance.CompareClip(FXSoundClip))
                    AudioManager.Instance.SetSourceClip(null, 1);

                AudioManager.Instance.SetSourceVome(0, 1);
                AudioManager.Instance.SetSourceVome(1, 1);

                player = null;
            }
        }
    }

    /// <summary>
    /// On Disable set Ambience off
    /// </summary>
    private void OnDisable()
    {
        if(AudioManager.Instance)
        {
            AudioManager.Instance.SetSourceClip(null, 1);
        }
    }
}