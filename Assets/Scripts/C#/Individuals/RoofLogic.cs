using UnityEngine;


/// <summary>
/// Switch roof between states
/// </summary>
[RequireComponent(typeof(Collider))]
public class RoofLogic : MonoBehaviour
{
    [SerializeField]
    GameObject roofToDeactivate;

    [SerializeField]
    bool switchLogic = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check Player
        if(other.GetComponent<PlayerController>())
            SwitchRoofView(false);
    }

    private void OnTriggerExit(Collider other)
    {
        //Check Player
        if (other.GetComponent<PlayerController>())
            SwitchRoofView(true);
    }


    /// <summary>
    /// Switch Roof
    /// </summary>
    /// <param name="isShown">Does Roof exist or not</param>
    void SwitchRoofView(bool isShown)
    {
        if(roofToDeactivate)
        {
            if (switchLogic)
                roofToDeactivate.SetActive(!isShown);
            else
                roofToDeactivate.SetActive(isShown);
        }
    }
}
