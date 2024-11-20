using System.Collections;
using System.Collections.Generic;
using UnityEngine;
interface IGimmick
{
    void Activate();
}


[System.Serializable]
public class GimmickPair
{
    public SwitchTest _switchTest;
    public MonoBehaviour _gimmick; //インスペクターから操作できるように一旦MonoBehaviourにしてる
}

public class RegistrationGimmick : MonoBehaviour
{
    [SerializeField] private List<GimmickPair> _gimmickPairs;
    private void Start()
    {
        foreach (var pair in _gimmickPairs)
        {
            if(pair._switchTest != null && pair._gimmick is IGimmick gimmick)
            {
                pair._switchTest.OnSwitchPressed += gimmick.Activate;
            }
            else
            {
                Debug.Log("ギミックにIGimmickが実装されていない");
            }
        }
    }
}


