using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFader : MonoBehaviour
{
    public CanvasGroup uiElement;
    public float fadeIn;
    public float fadeOut;

    public bool done { get; private set; }

    void Start()
    {
        done = false;
    }

    public void FadeIn()
    {
        done = false;
        StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 1, fadeIn));
    }

    public void FadeOut()
    {
        done = false;
        StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 0, fadeOut));
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup group, float start, float end, float time)
    {
        float timeStarted = Time.time;
        float timeSinceStarted = Time.time - timeStarted;
        float complete = timeSinceStarted / time;

        while (true)
        {
            timeSinceStarted = Time.time - timeStarted;
            complete = timeSinceStarted / time;
            float currentValue = Mathf.Lerp(start,end,complete);
            group.alpha = currentValue;
            if (complete >= 1)
            {
               // StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 0, fadeOut));
                //  group.alpha = 0f;
                done = true;
                break;

            }

            yield return new WaitForEndOfFrame();
        }
    }
    
}
