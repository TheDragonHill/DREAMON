using UnityEngine;

/// <summary>
/// Just rotate this object around y axis
/// </summary>
public class TurnAround : MonoBehaviour
{
    [SerializeField]
    float speed = 10;

    [SerializeField]
    Vector3 rotateAroundThisVector = new Vector3(0, 1, 0);

    private void Update()
    {
        transform.Rotate(rotateAroundThisVector * speed * Time.deltaTime, Space.Self);
    }
}
