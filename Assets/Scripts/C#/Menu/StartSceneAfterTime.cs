using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class StartSceneAfterTime : MonoBehaviour
{
    [SerializeField]
    string nextScene;

    [SerializeField]
    float time = 0;

    void Start()
    {
        Invoke(nameof(StartScene), time);
    }

    void StartScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
