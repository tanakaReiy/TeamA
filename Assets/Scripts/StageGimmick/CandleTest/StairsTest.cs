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
        CRIAudioManager.SE.Play3D(Vector3.zero, "CueSheet_0", "SE_up_stairs");
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
            Debug.LogError($"{sceneName} ‚Í–³Œø‚ÈƒV[ƒ“–¼‚Å‚·");
        }
    }
}
