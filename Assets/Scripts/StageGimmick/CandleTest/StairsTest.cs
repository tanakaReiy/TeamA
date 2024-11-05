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
        Debug.Log("階段をのぼる");
        StartCoroutine(DelaySwitchScene(_delay, _sceneName));
    }
    private IEnumerator DelaySwitchScene(float delay, string sceneName)
    {
        yield return new WaitForSeconds(delay);
        try
        {
            SceneLoader.LoadSceneSimple(sceneName);
        }
        catch
        {
            Debug.LogError($"{sceneName} は無効なシーン名です");
        }
    }
}
