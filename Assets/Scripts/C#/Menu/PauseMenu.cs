using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PauseMenu : MonoBehaviour
{
    bool pauseMenuOpen = false;

    [SerializeField]
    Canvas pauseMenu;

    KeyCode openPauseMenu;
    bool cursorWasVisible;

    void Start()
    {
        #if UNITY_EDITOR
                openPauseMenu = KeyCode.Backspace;
        #else
                openPauseMenu = KeyCode.Escape;
        #endif

        pauseMenu.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(openPauseMenu))
        {
            if (pauseMenuOpen)
            {
                CloseMenu();
            }
            else
            {
                ShowMenu();
            }
        }
    }

    public void CloseMenu()
    {
        if (SceneManager.sceneCount > 1)
            return;

        pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1;
        AudioManager.Instance?.PitchManual(1, 0, 1);

        // Set Cursor to 3D Particle Cursor
        Cursor.visible = cursorWasVisible;

        pauseMenuOpen = false;
    }

    void ShowMenu()
    {
        pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0f;
        pauseMenuOpen = true;

        AudioManager.Instance?.PitchManual(0.7f, 0, 1);

        cursorWasVisible = Cursor.visible;
        // Set Cursor to MenuCursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SaveManager.instance?.currentAutoSave.ForceSave();
    }

    private void OnDisable()
    {
        Cursor.visible = true;
        Time.timeScale = 1;

        AudioManager.Instance?.PitchManual(1, 0, 1);

        // Set Cursor to 3D Particle Cursor
        Cursor.visible = true;
    }
}
