using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    [SerializeField] Color hideColor;
    [SerializeField] Color showColor;
    [SerializeField] float fadeDuration = 1f;

    public bool IsFading { get; set; }

    public void StartFade()
    {
        StartCoroutine(Fading());
    }

    public void EndFade()
    {
        StartCoroutine(FadeEnd());
    }

    IEnumerator Fading()
    {
        fadeImage.gameObject.SetActive(true);
        float timer = 0;

        IsFading = true;

        while (timer < 1)
        {
            timer += Time.deltaTime * (1 / fadeDuration);

            fadeImage.color = Color.Lerp(showColor, hideColor, timer);

            yield return null;
        }
        yield return null;

        IsFading = false;
    }
    IEnumerator FadeEnd()
    {
        float timer = 0;
        IsFading = true;

        while (timer < 1)
        {
            timer += Time.deltaTime * (1 / fadeDuration);

            fadeImage.color = Color.Lerp(hideColor, showColor, timer);

            yield return null;
        }
        yield return null;

        IsFading = false;
        fadeImage.gameObject.SetActive(false);
    }
}
