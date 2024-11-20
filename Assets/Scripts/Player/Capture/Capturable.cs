using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alchemy.Inspector;
using Ability;
using UnityEngine.Events;

/// <summary>
/// �L���v�`�������Ώۂɂ��Ă�������
/// �L���v�`�������ۂɂȂ�̃A�r���e�B�𓾂�邩��CapturableAbility�Ŏw�肵�Ă�������
/// </summary>
public class Capturable : MonoBehaviour,IDetectable
{
    [field: SerializeField, SerializeReference]
    public WandManager.CaptureAbility CapturableAbility { get; private set; }

    public UnityEvent OnCaptured;

    public bool IsEnableDetect => true;



    private void Awake()
    {
        if(!TryGetComponent<Collider>(out var c))
        {
            Debug.LogError("Colider�����݂��Ȃ����߂���Capturable�͌��o����܂���");
        }
    }


}


