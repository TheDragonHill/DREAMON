using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Change Button if a Game is loadable
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonLoadGame : MonoBehaviour
{
    [SerializeField]
    string loadText = "continue";

    void Start()
    {
        CheckContinueGame();
    }

    /// <summary>
    /// Check if a Game is available
    /// </summary>
    void CheckContinueGame()
    {
        if (SaveManager.instance)
        {
            if (SaveManager.instance.SaveExists())
            {
                GetComponent<Button>().onClick.AddListener(() => SaveManager.instance.LoadSavedGame());
                GetComponentInChildren<TextMeshProUGUI>().text = loadText;
                SaveManager.instance.isNewGame = false;
            }
            else
            {
                SaveManager.instance.isNewGame = true;
            }
        }
    }
}
