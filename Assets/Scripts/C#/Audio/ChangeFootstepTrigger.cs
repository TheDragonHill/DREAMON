using UnityEngine;

/// <summary>
/// Change Footstepssound on Trigger
/// </summary>
[RequireComponent(typeof(Collider))]
public class ChangeFootstepTrigger : MonoBehaviour
{
    /// <summary>
    /// Set Audioindex of new Footstep, found in Inspector of PlayerMotor
    /// </summary>
    [SerializeField]
    int newFootstepsIndex = -1;

    /// <summary>
    /// Set Audioindex of current FootstepsIndex, found in Inspector of PlayerMotor
    /// </summary>
    [SerializeField]
    int normalFootstepsIndex = -1;

    PlayerMotor currentPlayermotor;

    void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if indices are set
        if (newFootstepsIndex >= 0 && normalFootstepsIndex >= 0)
        {
            // set currentPlayermotor
            if (currentPlayermotor = other.GetComponent<PlayerMotor>()) 
            {
                // set footstepindex to not the current one
                if (currentPlayermotor.GetFootstepIndex() == newFootstepsIndex)
                {
                    currentPlayermotor.ChangeFootstepsSound(normalFootstepsIndex);
                }
                else
                {
                    currentPlayermotor.ChangeFootstepsSound(newFootstepsIndex);
                }
            }
        }
    }
}
