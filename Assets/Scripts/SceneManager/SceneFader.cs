using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    public CanvasGroup uiElement;
    public float fadeIn;
    public float fadeOut;

    public bool IsFadingOut = false;
    public bool done = false;

    void Start()
    {
        if (uiElement == null)
        {
            GameObject uiObject = GameObject.Find("SceneFade");
            if (uiObject.TryGetComponent(out CanvasGroup group))
            {
                uiElement = group;
            }
        }
    }
    private void Update()
    {
        if (uiElement == null)
        {
            GameObject uiObject = GameObject.Find("SceneFade");
            if (uiObject.TryGetComponent(out CanvasGroup group))
            {
                uiElement = group;
            }
        }
        if (IsFadingOut)
        {
            FadeOut();
        }
    }

    public void FadeIn()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && player.TryGetComponent(out PlayerControler pControler))
        {
            pControler.FreezeMovement(false);
        }
        done = false;
        StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 1, fadeIn));
    }

    public void FadeOut()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && player.TryGetComponent(out PlayerControler pControler))
        {
            pControler.FreezeMovement(true);
        }
        done = false;
        if (uiElement != null)
        {
            StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 0, fadeOut));
            IsFadingOut = false;
        }
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup group, float start, float end, float time)
    {
        float timeStarted = Time.time;
        float timeSinceStarted;
        float complete;

        while (true)
        {
            timeSinceStarted = Time.time - timeStarted;
            complete = timeSinceStarted / time;
            float currentValue = Mathf.Lerp(start, end, complete);
            group.alpha = currentValue;
            if (complete >= 1)
            {
                done = true;
                break;

            }

            yield return new WaitForEndOfFrame();
        }
    }
}
