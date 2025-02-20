using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class HoverDone : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    public void OnHoverEnter()
    {
            StartCoroutine(FadeCanvasGroup(canvasGroup, 1f, 0.25f));
    }

    public void OnHoverExit()
    {
            StartCoroutine(FadeCanvasGroup(canvasGroup, 0f, 0.5f));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup group, float targetAlpha, float duration)
    {
        float startAlpha = group.alpha;
        float time = 0f;

        while (time < duration)
        {
            group.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        group.alpha = targetAlpha;
    }
}
