using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    [SerializeField]
    private Image _fadeImage;
    private void Awake()
    {
        _fadeImage.color = new Color(0, 0, 0, 255);
    }
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        float fadeDuration = 3.0f;
        float timer = 0;
        while (timer < fadeDuration) 
        {
            float alpha = Mathf.Lerp(1, 0, timer / fadeDuration);
            _fadeImage.color = new Color(0, 0, 0, alpha);
            timer += Time.deltaTime;
            yield return null;
        }
        _fadeImage.enabled = false;
        SceneLoader.LoadSceneSimple(sceneName);
    }
}
