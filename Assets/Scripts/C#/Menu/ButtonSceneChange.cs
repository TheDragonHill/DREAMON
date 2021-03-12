using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Change Scene on Button Click
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonSceneChange : MonoBehaviour
{
    [SerializeField]
    protected string nextScene;

    protected void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => ChangeScene());
    }

    protected void ChangeScene()
    {
        if(!string.IsNullOrEmpty(nextScene))
        {
            SceneLoading();
        }
        else
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    protected virtual void SceneLoading()
    {
        SceneManager.LoadScene(nextScene);
    }
}
