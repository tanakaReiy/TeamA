using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeInController : MonoBehaviour
{
   private CanvasGroup _canvasGroup;
    [SerializeField]
    private float fadeDuration = 3f;
    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(FadeIn());
    }
    private IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(1, 0, timer / fadeDuration);
            yield return null;
        }
        _canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }
}
