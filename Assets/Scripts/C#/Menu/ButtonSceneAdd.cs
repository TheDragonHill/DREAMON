using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSceneAdd : ButtonSceneChange
{
    protected override void SceneLoading()
    {
        SceneManager.LoadScene(nextScene, LoadSceneMode.Additive);
    }
}
