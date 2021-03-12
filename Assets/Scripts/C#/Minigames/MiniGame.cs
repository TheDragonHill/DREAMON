using System.Collections;
using UnityEngine;

public class MiniGame : MonoBehaviour
{
    public virtual void StartMiniGame()
    {
        StartCoroutine(nameof(MiniGameUpdate));
    }

    protected virtual void EndMiniGame()
    {
        StopCoroutine(nameof(MiniGameUpdate));
        gameObject.SetActive(false);
    }

    protected virtual IEnumerator MiniGameUpdate()
    {
        yield return new WaitForEndOfFrame();
    }
}
