using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SetVersion : MonoBehaviour
{
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text += Application.version;
    }
}