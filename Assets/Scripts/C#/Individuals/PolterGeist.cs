using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

enum PolterMode
{
    Switch,
    Rotate,
    Shake,
    Scale,
}

public class PolterGeist : MonoBehaviour
{


    [SerializeField]
    float playTime = 2;


    [SerializeField]
    float delayTime = 0.5f;

    [SerializeField]
    Transform[] allPolterObjects;

    float maxPlayTime;
    
    private void Start()
    {
        maxPlayTime = playTime + 5;

        if (allPolterObjects.Length > 0)
            Polter();
    }

    void Polter()
    {
        int rIndexR = Random.Range(0, allPolterObjects.Length);
        int rIndexG = Random.Range(0, allPolterObjects.Length);
        PolterMode rMode = (PolterMode)Random.Range(0, 2);

        
        switch(rMode)
        {
            case PolterMode.Switch:
                SwitchObjects(rIndexR, rIndexG);
                break;
            case PolterMode.Rotate:
                RotateObjects(rIndexR);
                ShakeObjects(rIndexG);
                break;
            case PolterMode.Shake:
                ShakeObjects(rIndexR);
                ScaleObjects(rIndexG);
                break;
            case PolterMode.Scale:
                ScaleObjects(rIndexR);
                RotateObjects(rIndexG);
                break;

        }

        Invoke(nameof(Polter), Random.Range(playTime, maxPlayTime) + delayTime);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Polter));
    }

    void SwitchObjects(int i, int l)
    {
        Sequence s = DOTween.Sequence();
        s.Append(allPolterObjects[i].DOJump(allPolterObjects[l].position, 1, 1, playTime));
        s.Join(allPolterObjects[i].DOShakeRotation(playTime, 45));
        if(i != l)
        {
            s.Join(allPolterObjects[l].DOJump(allPolterObjects[i].position, 1, 1, playTime));
            s.Join(allPolterObjects[l].DOShakeRotation(playTime, 45));
        }
        s.Play();
    }

    void RotateObjects(int i)
    {
        allPolterObjects[i].DOShakeRotation(playTime);
    }

    void ShakeObjects(int i)
    {
        allPolterObjects[i].DOShakePosition(playTime, new Vector3(0.5f, 0, 0.5f));
    }

    void ScaleObjects(int i)
    {
        allPolterObjects[i].DOShakeScale(playTime, 0.2f);
    }
}
