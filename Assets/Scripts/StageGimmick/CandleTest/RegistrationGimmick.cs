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
    public MonoBehaviour _gimmick; //�C���X�y�N�^�[���瑀��ł���悤�Ɉ�UMonoBehaviour�ɂ��Ă�
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
                Debug.Log("�M�~�b�N��IGimmick����������Ă��Ȃ�");
            }
        }
    }
}


