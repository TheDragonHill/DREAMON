using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonNewGame : MonoBehaviour
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
                GetComponent<Button>().onClick.AddListener(() => DeleteGame());
            }
        }
    }


    void DeleteGame()
    {
        SaveManager.instance.NewGame();
        SceneManager.LoadScene(1);
    }
}
