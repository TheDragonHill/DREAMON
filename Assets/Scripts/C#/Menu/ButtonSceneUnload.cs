using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSceneUnload : ButtonSceneChange
{
    protected override void SceneLoading()
    {
        SceneManager.UnloadSceneAsync(nextScene);
    }
}
