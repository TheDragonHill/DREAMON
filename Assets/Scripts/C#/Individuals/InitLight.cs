using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent(typeof(Light))]
public class InitLight : MonoBehaviour
{
    [SerializeField]
    float delay = 1;


    [SerializeField]
    float duration = 1;


    Light light;
    float targetIntensity;


    void Start()
    {
        light = GetComponent<Light>();
        targetIntensity = light.intensity;

        light.color = Color.black;
        light.intensity = 0;

        Invoke(nameof(InitSmoothLight), delay);
    }


    void InitSmoothLight()
    {
        light.DOColor(Color.white, duration);
        light.DOIntensity(targetIntensity, duration * 1.5f);
    }

}
