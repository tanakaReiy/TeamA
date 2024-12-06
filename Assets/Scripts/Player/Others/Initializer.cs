using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Iinitialzable�����Q�[���I�u�W�F�N�g�������ɓo�^�����Start�ŏ���������܂�
/// </summary>
public class Initializer : MonoBehaviour
{
    [SerializeField] private List<GameObject> _initializeObjects;

    private void Start()
    {
        for (int i = 0; i < _initializeObjects.Count; i++)
        {
            IInitializable[] inits = _initializeObjects[i].GetComponents<IInitializable>();
            if(inits == null) { return; }
            for(int k = 0; k < inits.Length; k++)
            {
                inits[k].Initialize();
            }
        }
    }
}

/// <summary>
/// ��A�N�e�B�u�ł������������s������Object�̂��߂̃C���^�[�t�F�C�X�ł�
/// </summary>
public interface IInitializable
{
    void Initialize();
}