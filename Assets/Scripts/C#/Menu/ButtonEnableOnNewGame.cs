using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonEnableOnNewGame : MonoBehaviour
{

    private void OnEnable()
    {
        CheckNewGame();
    }

    /// <summary>
    /// Check if a Game is available
    /// </summary>
    void CheckNewGame()
    {
        if (SaveManager.instance)
        {
            if (SaveManager.instance.SaveExists())
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
