using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimation : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    string animationState;

    int currentstate = 0;

    AnimationClip[] allClips;

    private void Start()
    {
        if (animator && !string.IsNullOrEmpty(animationState))
        {

            allClips = animator.runtimeAnimatorController.animationClips;
            StartCoroutine(PlayAlwaysAnimation());
        }
    }

    IEnumerator PlayAlwaysAnimation()
    {
        while(gameObject.activeInHierarchy)
        {
            animator.CrossFadeInFixedTime(allClips[currentstate].name, 0.3f);

            yield return new WaitForSeconds(allClips[currentstate].length + 1);

            currentstate++;
            if (currentstate >= 3)
                currentstate = 0;
        }

    }
}
