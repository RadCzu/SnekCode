using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Events;

public class UIFadeAnimator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textElement;
    [SerializeField] private Image imageElement;
    [SerializeField] private UnityEvent onFadeComplete;

    /// <summary>
    /// Fades in the UI elements by gradually increasing their opacity.
    /// </summary>
    /// <param name="duration">The duration of the fade animation in seconds.</param>
    /// <param name="textEndOpacity">The target opacity for the TextMeshPro element (0 to 1).</param>
    /// <param name="imageEndOpacity">The target opacity for the Image element (0 to 1).</param>
    public void FadeInUI(float duration, float textEndOpacity, float imageEndOpacity)
    {
        StartCoroutine(FadeCoroutine(duration, textEndOpacity, imageEndOpacity));
    }

    private IEnumerator FadeCoroutine(float duration, float textEndOpacity, float imageEndOpacity)
    {
        if (textElement == null || imageElement == null)
        {
            yield break;
        }

        Color textColor = textElement.color;
        Color imageColor = imageElement.color;

        float elapsed = 0f;
        float textStartOpacity = textColor.a;
        float imageStartOpacity = imageColor.a;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            float newTextAlpha = Mathf.Lerp(textStartOpacity, textEndOpacity, t);
            float newImageAlpha = Mathf.Lerp(imageStartOpacity, imageEndOpacity, t);

            textColor.a = newTextAlpha;
            imageColor.a = newImageAlpha;

            textElement.color = textColor;
            imageElement.color = imageColor;

            yield return null;
        }

        textColor.a = textEndOpacity;
        imageColor.a = imageEndOpacity;

        textElement.color = textColor;
        imageElement.color = imageColor;

        onFadeComplete?.Invoke();
    }
}
