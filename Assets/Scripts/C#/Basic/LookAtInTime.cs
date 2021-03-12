using DG.Tweening;
using UnityEngine;

/// <summary>
/// Look at a specific transform in a set amount of time
/// </summary>
public class LookAtInTime : MonoBehaviour
{
    [SerializeField]
    Transform lookAtStart;

    [SerializeField]
    float timeAtStart = 3;

    void Start()
    {
        Look(lookAtStart.position, timeAtStart);
    }

    /// <summary>
    /// Look at an object after a set amount of time
    /// </summary>
    /// <param name="position">Position of object</param>
    /// <param name="time">Time until the object is in focus</param>
    public void Look(Vector3 position, float time)
    {
        transform.DOLookAt(position, time);
    }
}
