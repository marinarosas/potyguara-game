using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public bool fadeOnStart = true;
    public float fadeDuration = 2;
    public CanvasGroup cg;

    // Start is called before the first frame update
    void Start()
    {
        if (fadeOnStart)
            FadeIn();
    }

    public void FadeIn()
    {
        Fade(1, 0);
    }
    public void FadeOut(int index)
    {
        Fade(0, 1, index);
    }

    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeInRoutine(cg, alphaIn, alphaOut));
    }
    public void Fade(float alphaIn, float alphaOut, int index)
    {
        StartCoroutine(FadeOutRoutine(cg, alphaIn, alphaOut, index));
    }

    public IEnumerator FadeInRoutine(CanvasGroup canvasGroup, float alphaIn, float alphaOut)
    {
        float timer = 0;
        while (timer <= fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;
    }

    public IEnumerator FadeOutRoutine(CanvasGroup canvasGroup, float alphaIn,float alphaOut, int index)
    {
        float timer = 0;
        while(timer <= fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);
            Debug.Log(Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration));
            timer += Time.deltaTime;
            yield return null;
        }
        FindFirstObjectByType<TransitionController>().LoadSceneAsync(index);
    }
}
