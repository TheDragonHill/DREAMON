using DG.Tweening;
using UnityEngine;


/// <summary>
/// Switch roof between states
/// </summary>
[RequireComponent(typeof(Collider))]
public class LightLogic : MonoBehaviour
{
    [SerializeField]
    Light lightToActivate;

    [SerializeField]
    bool lightActive = false;

    float oldIntensity;

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
        oldIntensity = lightToActivate.intensity;
        if (!lightActive)
            lightToActivate.intensity = 0;
    }


    private void OnTriggerEnter(Collider other)
    {
        // Check Player
        if (other.GetComponent<PlayerController>())
            SwitchLight();
    }

    private void OnTriggerExit(Collider other)
    {
        //Check Player
        if (other.GetComponent<PlayerController>())
            SwitchLight();
    }


    /// <summary>
    /// Switch Roof
    /// </summary>
    /// <param name="isShown">Does Roof exist or not</param>
    void SwitchLight()
    {
        if (lightToActivate)
        {
            if (lightActive)
                lightToActivate.DOIntensity(0, 3);
            else
                lightToActivate.DOIntensity(oldIntensity, 3);

            lightActive = !lightActive;
        }
    }
}
