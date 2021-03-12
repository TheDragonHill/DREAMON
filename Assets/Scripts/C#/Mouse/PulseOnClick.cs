using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Light))]
public class PulseOnClick : MonoBehaviour
{

    [SerializeField]
    float time = 0.5f;

    Light light;
    float maxIntensity;

    void Start()
    {
        light = GetComponent<Light>();
        maxIntensity = light.intensity * 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && !Cursor.visible)
        {
            CancelInvoke(nameof(Reverse));
            light.DOIntensity(maxIntensity, time);
            Invoke(nameof(Reverse), time);
        }
    }

    void Reverse()
    {
        light.DOIntensity(1, time);
    }
}
