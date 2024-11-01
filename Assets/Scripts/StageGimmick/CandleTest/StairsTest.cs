using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StairsTest : MonoBehaviour, IGimmick
{
    [SerializeField] float _delay;
    [SerializeField] string _sceneName;
    public void Activate()
    {
        Debug.Log("ŠK’i‚ð‚Ì‚Ú‚é");
        StartCoroutine(DelaySwitchScene(_delay, _sceneName));
    }
    private IEnumerator DelaySwitchScene(float delay, string sceneName)
    {
        yield return new WaitForSeconds(delay);
        SceneLoader.LoadSceneSimple(sceneName);
    }
}
